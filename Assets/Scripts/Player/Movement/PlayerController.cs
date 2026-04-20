namespace Player
{
    using UnityEngine;
    using Manager;
    using Manager.Audio;

    public class PlayerController : MonoBehaviour
    {
        [Header("References")] public PlayerControllerStats MoveStats;

        [SerializeField] private Collider2D _feetColl;
        [SerializeField] private Collider2D _bodyColl;

        private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        public SpriteRenderer SpriteRenderer;
        public ParticleSystem dust;
        public ParticleSystem dashDust;
        
        //Variables de mouvement
        public float HorizontalVelocity { get; set; }
        private bool _isFacingRight;

        //Variables pour check les collisions
        private RaycastHit2D _groundHit;
        private RaycastHit2D _headHit;
        public bool _isGrounded;
        private bool _bumpedHead;

        //Variables de saut
        public float VerticalVelocity { get; set; }
        private bool _isJumping;
        private bool _isFastFalling;
        private bool _isFalling;
        private float _fastFallTime;
        private float _fastFallReleaseSpeed;
        private int _numberOfJumpUsed;

        //Variables apex (Sommet du saut ou vélocity ver = 0)
        private float _apexPoint;
        private float _timePastApexThreshold;
        private bool _isPastApexThreshold;

        //Variables de Jump Buffer  (Tampon de saut)
        private float _jumpBufferTimer;
        private bool _jumpReleasedDuringBuffer;

        //Variable de Coyotte Time
        private float _coyotteTimer;

        //Variables du Dash
        private bool _isDashing;
        private bool _isAirDashing;
        private float _dashTimer;
        private float _dashOnGroundTimer;
        private int _numberOfDashesUsed;
        private Vector2 _dashDirection;
        private bool _isDashFastFalling;
        private float _dashFastFallTime;
        private float _dashFastFallReleaseSpeed;

        private void Awake()
        {
            //On récupère le component 
            _rb = GetComponent<Rigidbody2D>();

            //on met le player en position 'droite' au debut 
            _isFacingRight = true;
            
            MoveStats = Instantiate(MoveStats);

            // On set les capacités avec celle sauvegardées dans le state 
            MoveStats.numberOfJumpsAllowed = PlayerState.numberOfJumpsAllowed;
            MoveStats.numberOfDashes = PlayerState.numberOfDashes;
        }

        private void Update()
        {
            CountTimers();
            JumpChecks();
            LandCheck();
            DashCheck();
        }

        private void FixedUpdate()
        {
            CollisionChecks();
            Jump();
            Fall();
            Dash();

            //Vélocité si le joueur est en l'air ou au sol
            if (_isGrounded)
            {
                Move(MoveStats.groundAcceleration, MoveStats.groundDeceleration, InputManager.Movement);
            }
            else
            {
                Move(MoveStats.airAcceleration, MoveStats.airDeceleration, InputManager.Movement);
            }

            //On applique la vélocity au rigidbody
            ApplyVelocity();
        }

        private void ApplyVelocity()
        {
            //On plafonne la vitesse de chute et de montée
            VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.maxFallSpeed, MoveStats.maxRiseSpeed);
            _rb.linearVelocity = new Vector2(HorizontalVelocity, VerticalVelocity);
            
        }

        #region Movement

        private void Move(float acceleration, float deceleration, Vector2 moveInput)
        {
            //Si le joueur est en mouvement
            if (Mathf.Abs(moveInput.x) >= MoveStats.moveThreeshold)
            {
                //On tourne le joueur en fonction de si il va a droite ou gauche
                TurnCheck(moveInput);

                //On initialise la vélocité
                float targetVelocity = 0f;

                //Si le joueur cours 
                if (InputManager.RunIsHeld)
                {
                    _animator.SetBool("isWalking", false);
                    _animator.SetBool("isRunning", true);
                    //On modifie sa vélocité avec la propriété 'maxRunSpeed' issue de 'PlayerMovementStats'
                    targetVelocity = moveInput.x * MoveStats.maxRunSpeed;
                }
                //Sinon c'est qu'il marche
                else
                {
                    _animator.SetBool("isRunning", false);
                    _animator.SetBool("isWalking", true);
                    //On modifie sa vélocité avec la propriété 'maxWalkSpeed' issue de 'PlayerMovementStats'
                    targetVelocity = moveInput.x * MoveStats.maxWalkSpeed;
                }

                //Créer un mouvement de décéleration smooth avec la formule d'interpolation linéraire (lerp)
                //Plus l'accéleration est importante, plus sa 'freine' vite 
                //https://www.youtube.com/watch?v=kAKPOVBAFtI (vidéo cool pour comprendre facilement son utilisation)
                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

            }

            //Si il est immobile 
            else if (Mathf.Abs(moveInput.x) < MoveStats.moveThreeshold)
            {
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isWalking", false);
                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
            }
        }

        private void TurnCheck(Vector2 moveInput)
        {
            if (_isFacingRight && moveInput.x < 0)
            {
                Turn(false); 
            }
            else if (!_isFacingRight && moveInput.x > 0)
            {
                Turn(true);
            }
        }

        private void Turn(bool turnRight)
        {
            if (turnRight)
            {
                _isFacingRight = true;
                transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                _isFacingRight = false;
                transform.Rotate(0f, -180f, 0f);
            }
        }

        #endregion

        #region Atterisage/Chute

        private void LandCheck()
        {
            //A l'attérissage 
            if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0.1f)
            {
                //On remet toutes les variables a 0
                _isJumping = false;
                _isFalling = false;
                _isFastFalling = false;
                _fastFallTime = 0f;
                _isPastApexThreshold = false;
                _numberOfJumpUsed = 0;

                ResetDashValues();
                ResetDashes();

                VerticalVelocity = Physics2D.gravity.y;
                _animator.SetBool("isJumping", false);
                dust.Play();
            }
        }

        private void Fall()
        {
            //Gravité normale quand on tombe 
            if (!_isGrounded && !_isJumping)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }

                VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
            }
        }

        #endregion

        #region Jump

        private void JumpChecks()
        {
            //Check quand le boutton de jump est appuyé 
            if (InputManager.JumpWasPressed)
            {
                _jumpBufferTimer = MoveStats.jumpBufferTime;
                _jumpReleasedDuringBuffer = false;
            }

            //Cheks quand le boutton de jump est relaché 
            if (InputManager.JumpWasReleased)
            {
                // Si on relâche pendant que le buffer est actif, on mémorise pour faire un saut court GAMEFEEL
                if (_jumpBufferTimer > 0f)
                {
                    _jumpReleasedDuringBuffer = true;
                }

                // Si on est en train de monter, on coupe la montée
                if (_isJumping && VerticalVelocity > 0f)
                {
                    // On a dépassé le point apex, donc chute rapide
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                        _isFastFalling = true;
                        _fastFallTime = MoveStats.timeForUpwardsCancel;
                        VerticalVelocity = 0f;
                    }
                    //On a pas dépassé le point apex, donc on maintient la velocité
                    else
                    {
                        _isFastFalling = true;
                        _fastFallReleaseSpeed = VerticalVelocity;
                    }
                }
            }

            //Init du jump avec le Buffer ET le coyotte time
            if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyotteTimer > 0f))
            {
                InitiateJump(1, dust);

                // Si le bouton a été relâché pendant le buffer alors saut court immédiat
                if (_jumpReleasedDuringBuffer)
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }

            }
            
            //Double Jump
            else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpUsed < MoveStats.numberOfJumpsAllowed)
            {
                _isFastFalling = false;
                InitiateJump(1, dust);
            }

            //Air Jump apres coyotte time 
            else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpUsed < MoveStats.numberOfJumpsAllowed ) //-1
            {
                InitiateJump(1, dust); //2
                _isFastFalling = false;
            }
        }

        private void InitiateJump(int numberOfJumpsUsed, ParticleSystem particle)
        {
            if (!_isJumping)
            {
                _isJumping = true;
            }

            _jumpBufferTimer = 0f;
            _numberOfJumpUsed += numberOfJumpsUsed;
            VerticalVelocity = MoveStats.InitialJumpVelocity;
            ResetDashValues();
            _animator.SetBool("isJumping", true);
            particle?.Play();
            SoundManager.Instance.PlaySound3D("Jump", transform.position);
        }

        private void Jump()
        {
            //On aplique la gravité au saut 
            if (_isJumping)
            {
                //Colision avec le plafond on met le flag de chute rapide
                if (_bumpedHead)
                {
                    _isFastFalling = true;
                }

                //Gravité sur la phase croissante du saut 
                if (VerticalVelocity >= 0f)
                {
                    //Control du point apex
                    _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                    if (_apexPoint > MoveStats.apexThreshold)
                    {
                        if (!_isPastApexThreshold)
                        {
                            _isPastApexThreshold = true;
                            _timePastApexThreshold = 0f;
                        }

                        if (_isPastApexThreshold)
                        {
                            _timePastApexThreshold += Time.fixedDeltaTime;
                            if (_timePastApexThreshold < MoveStats.apexHangTime)
                            {
                                // Pendant le hang time
                                VerticalVelocity = 0f;
                            }
                            else
                            {
                                //On amorce la descente
                                VerticalVelocity = -0.01f;
                            }
                        }
                    }
                    //Gravité sur la phase croissante du saut mais pas au seuil du point apex
                    else if (!_isFastFalling)
                    {
                        VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                        if (_isPastApexThreshold)
                        {
                            _isPastApexThreshold = false;
                        }
                    }
                }
                //Gravité sur la phase décroissante du saut (aprés l'apex point)
                else if (!_isFastFalling)
                {
                    VerticalVelocity += MoveStats.Gravity * MoveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }

                else if (VerticalVelocity < 0f)
                {
                    if (!_isFalling)
                    {
                        _isFalling = true;
                    }
                }
            }

            //Jump cut (moment ou on relcahe le saut AVANT l'apex)
            if (_isFastFalling)
            {
                //Accélération de la chute
                if (_fastFallTime >= MoveStats.timeForUpwardsCancel)
                {
                    VerticalVelocity += MoveStats.Gravity * MoveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_fastFallTime < MoveStats.timeForUpwardsCancel)
                {
                    //Freine smooth la montée 
                    VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f,
                        (_fastFallTime / MoveStats.timeForUpwardsCancel));
                }

                _fastFallTime += Time.fixedDeltaTime;
            }
        }


        #endregion

        #region Dash

        private void DashCheck()
        {
            //On vérifie que l'input de dash a éte appuyé 
            if (InputManager.DashWasPressed)
            {
                //Dash au sol (si on a des dashs)
                if (_isGrounded && _dashOnGroundTimer < 0 && !_isDashing && _numberOfDashesUsed < MoveStats.numberOfDashes)
                {
                    InitiateDash();
                }
                //Dash en l'air
                else if (!_isGrounded && !_isDashing && _numberOfDashesUsed < MoveStats.numberOfDashes)
                {
                    _isAirDashing = true;
                    InitiateDash();
                }
            }
        }

        private void InitiateDash()
        {
            // Récupère la direction au moment du dash
            _dashDirection = InputManager.Movement;

            //Cherche la direction prédéfinie la plus proche de l'input
            Vector2 closestDirection = Vector2.zero;
            float minDistance = Vector2.Distance(_dashDirection, MoveStats.DashDirections[0]);

            for (int i = 0; i < MoveStats.DashDirections.Length; i++)
            {
                // On skip si ca correspond a une direction prédefinie
                if (_dashDirection == MoveStats.DashDirections[i])
                {
                    closestDirection = _dashDirection;
                    break;
                }

                float distance = Vector2.Distance(_dashDirection, MoveStats.DashDirections[i]);

                // On regarde si le dash est diagonale et on aplique le bias 
                bool isDiagonal = (Mathf.Abs(MoveStats.DashDirections[i].x) == 1 &&
                                   Mathf.Abs(MoveStats.DashDirections[i].y) == 1);

                if (isDiagonal)
                {
                    distance -= MoveStats.dashDiagonallyBias;
                }

                else if (distance < minDistance)
                {
                    //La direction la plus proche trouvée
                    minDistance = distance;
                    closestDirection = MoveStats.DashDirections[i];
                }
            }

            //Aucune direction trouvée, on dash dans le sens du player
            if (closestDirection == Vector2.zero)
            {
                if (_isFacingRight)
                {
                    closestDirection = Vector2.right;
                }
                else
                {
                    closestDirection = Vector2.left;
                }
            }

            //On applique la diretion et on initialise les propriétés du dash
            _dashDirection = closestDirection;
            _numberOfDashesUsed++;
            _isDashing = true;
            _dashTimer = 0f;
            _dashOnGroundTimer = MoveStats.timeBtwDashesOnGround;
            dashDust.Play();
            SoundManager.Instance.PlaySound2D("Dash");

            // ResetJumpValue();

        }

        private void Dash()
        {
            if (_isDashing)
            {
                //Stop le dash avant le timer
                _dashTimer += Time.fixedDeltaTime;
                if (_dashTimer >= MoveStats.dashTime)
                {
                    if (_isGrounded)
                    {
                        ResetDashes();
                    }

                    _isAirDashing = false;
                    _isDashing = false;

                    if (!_isJumping)
                    {
                        _dashFastFallTime = 0f;
                        _dashFastFallReleaseSpeed = VerticalVelocity;

                        if (!_isGrounded)
                        {
                            _isDashFastFalling = true;
                        }
                    }

                    return;
                }

                HorizontalVelocity = MoveStats.dashSpeed * _dashDirection.x;

                if (_dashDirection.y != 0f || _isAirDashing)
                {
                    VerticalVelocity = MoveStats.dashSpeed * _dashDirection.y;
                }
            }

            //Dash cut 
            else if (_isDashFastFalling)
            {
                if (VerticalVelocity > 0f)
                {
                    if (_dashFastFallTime < MoveStats.dashTimeForUpwardsCancel)
                    {
                        VerticalVelocity = Mathf.Lerp(_dashFastFallReleaseSpeed, 0f,
                            (_dashFastFallTime / MoveStats.dashTimeForUpwardsCancel));
                    }
                    else if (_dashFastFallTime >= MoveStats.dashTimeForUpwardsCancel)
                    {
                        VerticalVelocity += MoveStats.Gravity * MoveStats.dashGravityOnReleaseMultiplier *
                                            Time.fixedDeltaTime;
                    }

                    _dashFastFallTime += Time.fixedDeltaTime;
                }
                else
                {
                    VerticalVelocity += MoveStats.Gravity * MoveStats.dashGravityOnReleaseMultiplier *
                                        Time.fixedDeltaTime;
                }
            }
        }

        private void ResetDashValues()
        {
            _isDashFastFalling = false;
            _dashOnGroundTimer = -0.01f;
        }

        private void ResetDashes()
        {
            _numberOfDashesUsed = 0;
        }

        #endregion

        #region Collisions Checks

        private void IsGrounded()
        {
            Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
            Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

            _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,
                MoveStats.GroundDetectionRayLength, MoveStats.groundLayer);
            if (_groundHit.collider != null)
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
        }

        private void BumpedHead()
        {
            Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
            Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth,
                MoveStats.HeadDetectionRayLength);

            _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength,
                MoveStats.groundLayer);
            if (_headHit.collider != null)
            {
                _bumpedHead = true;
            }
            else
            {
                _bumpedHead = false;
            }
        }

        private void CollisionChecks()
        {
            IsGrounded();
            BumpedHead();
        }

        #endregion

        #region Timers

        private void CountTimers()
        {
            //Timer du Jump buffer
            _jumpBufferTimer -= Time.deltaTime;

            //Timer du Coyotte time
            if (!_isGrounded)
            {
                _coyotteTimer -= Time.deltaTime;
            }
            else
            {
                _coyotteTimer = MoveStats.jumpCoyoteTime;
            }

            //Timer du dash 
            if (_isGrounded)
            {
                _dashOnGroundTimer -= Time.deltaTime;
            }
        }

        #endregion
    }
}