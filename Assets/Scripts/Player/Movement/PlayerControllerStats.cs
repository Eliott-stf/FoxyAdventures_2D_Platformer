namespace Player
{

   using UnityEngine;

   [CreateAssetMenu(menuName = "Player Controller")]
   public class PlayerControllerStats : ScriptableObject
   {
      #region Walk Properties

      [Header("Walk")] [Range(0f, 1f)] public float moveThreeshold = 0.25f;

      [Range(1f, 100f)] public float maxWalkSpeed = 12.5f;

      [Range(0.25f, 50f)] public float groundAcceleration = 5f;

      [Range(0.25f, 50f)] public float groundDeceleration = 20f;

      [Range(0.25f, 50f)] public float airAcceleration = 5f;

      [Range(0.25f, 50f)] public float airDeceleration = 5f;

      #endregion

      #region Run Properties

      [Header("Run")] [Range(1f, 100f)] public float maxRunSpeed = 20f;

      #endregion

      #region Jump Properties

      [Header("Jump")] public float jumpHeight = 6.5f;
      [Range(1f, 1.1f)] public float jumpHeightCompensationFactor = 1.054f;
      public float timeTillJumpApex = 0.35f;
      [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
      public float maxFallSpeed = 26f;
      public float maxRiseSpeed = 50f;
      [Range(1, 5)] public int numberOfJumpsAllowed = 1;

      [Header("Jump Cut")] [Range(0.02f, 0.3f)]
      public float timeForUpwardsCancel = 0.027f;

      [Header("Jump Apex")] [Range(0.5f, 1f)]
      public float apexThreshold = 0.97f;

      [Range(0.01f, 1f)] public float apexHangTime = 0.075f;

      [Header("Jump Buffer")] [Range(0f, 1f)]
      public float jumpBufferTime = 0.125f;

      [Header("Jump Coyote Time")] [Range(0f, 1f)]
      public float jumpCoyoteTime = 0.1f;

      [Header("Jump Visualization tool")] public bool showWalkJumpArc = false;
      public bool showRunJumpArc = false;
      public bool stopOnCollision = true;
      public bool drawRight = true;
      [Range(5, 100)] public int arcResolution = 20;
      [Range(0, 500)] public int visualizationSteps = 90;

      public float Gravity { get; private set; }
      public float InitialJumpVelocity { get; private set; }
      public float AdjustedJumpHeight { get; private set; }

      #endregion

      #region Dash

      [Header("Dash")] [Range(0f, 1f)] public float dashTime = 0.11f;

      [Range(1f, 200f)] public float dashSpeed = 40f;

      [Range(0f, 1f)] public float timeBtwDashesOnGround = 0.225f;

      [Range(0, 5)] public int numberOfDashes = 0;

      [Range(0f, 0.5f)] public float dashDiagonallyBias = 0.4f;

      [Header("Dash Cancel Time")] [Range(0.01f, 5f)]
      public float dashGravityOnReleaseMultiplier = 1f;

      [Range(0.2f, 0.3f)] public float dashTimeForUpwardsCancel = 0.027f;

      #endregion

      #region Collision Properties

      [Header("Grounded/Collision Checks")] public LayerMask groundLayer;
      public float GroundDetectionRayLength = 0.02f;
      public float HeadDetectionRayLength = 0.02f;
      [Range(0f, 1f)] public float HeadWidth = 0.75f;

      #endregion

      private void OnValidate()
      {
         CalculateValues();
      }

      private void OnEnable()
      {
         CalculateValues();
      }

      private void CalculateValues()
      {
         AdjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
         Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
         InitialJumpVelocity = Mathf.Abs(Gravity) * timeTillJumpApex;
      }

      public readonly Vector2[] DashDirections = new Vector2[]
      {
         new Vector2(0, 0), // Rien
         new Vector2(1, 0), //Droite
         new Vector2(1, 1).normalized, //Haut-Droite
         new Vector2(0, 1), // Haut
         new Vector2(-1, 1).normalized, //Haut-Gauche
         new Vector2(-1, 0), // Gauche
         new Vector2(-1, -1).normalized, //Bas-Gauche
         new Vector2(0, -1), // Bas
         new Vector2(1, -1).normalized, //Bas-Droite
      };
   }
}