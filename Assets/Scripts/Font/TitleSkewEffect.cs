
    using UnityEngine;
    using TMPro;

    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public class TitleSkewEffect : MonoBehaviour
    {
        // élargissement du haut
        [Range(-0.5f, 0.5f)] public float skewTop = 0.3f; 
        public Gradient colorGradient;
        private TMP_Text _tmp;

        void OnEnable() => _tmp = GetComponent<TMP_Text>();

        void Update() => ApplyEffect();

        void ApplyEffect()
        {
            _tmp.ForceMeshUpdate();
            var mesh = _tmp.mesh;
            Vector3[] verts = mesh.vertices;
            Color[] colors = new Color[verts.Length];

            Bounds b = mesh.bounds;

            for (int i = 0; i < verts.Length; i++)
            {
                // Gradient vertical (bas = 0, haut = 1)
                float t = Mathf.InverseLerp(b.min.y, b.max.y, verts[i].y);
                colors[i] = colorGradient.Evaluate(t);

                // Skew : élargir les vertices du haut vers l'extérieur
                float skewAmount = Mathf.Lerp(0f, skewTop, t);
                verts[i].x += verts[i].x * skewAmount;
            }

            mesh.vertices = verts;
            mesh.colors = colors;
            _tmp.canvasRenderer.SetMesh(mesh);
        }
    }
