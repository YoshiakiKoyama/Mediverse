using UnityEngine;

namespace Just5Store.Modules.PianoGame
{
    public class PianoKey : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Transform visualRoot;

        [Header("Materials")]
        [SerializeField] private Material idleMaterial;
        [SerializeField] private Material pressedMaterial;

        [Header("Animation")]
        [SerializeField] private float pressDepth = 0.005f;

        [Header("Audio")]
        [SerializeField] private AudioClip pianoSound;

        private AudioSource audioSource;

        private Vector3 _defaultLocalPosition;
        private bool _pressed;
        private bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        public void Setup(Transform root, Material idle, Material pressed)
        {
            if (visualRoot == null)
                visualRoot = root;

            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>();

            idleMaterial = idle;
            pressedMaterial = pressed;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            if (visualRoot == null)
                visualRoot = transform;

            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>();

            // Find the shared AudioSource on the parent Piano
            audioSource = GetComponentInParent<AudioSource>();

            _defaultLocalPosition = visualRoot.localPosition;

            ApplyIdleState();

            _initialized = true;
        }

        public void Press()
        {
            Initialize();

            if (_pressed)
                return;

            _pressed = true;

            visualRoot.localPosition =
                _defaultLocalPosition + Vector3.down * pressDepth;

            if (pressedMaterial != null && targetRenderer != null)
                targetRenderer.material = pressedMaterial;

            // Play the assigned piano sound
            if (audioSource != null && pianoSound != null)
                audioSource.PlayOneShot(pianoSound);
        }

        public void Release()
        {
            Initialize();

            if (!_pressed)
                return;

            _pressed = false;

            visualRoot.localPosition = _defaultLocalPosition;

            ApplyIdleState();
        }

        public void SetIdleMaterial(Material material)
        {
            idleMaterial = material;

            if (!_pressed)
                ApplyIdleState();
        }

        public void SetPressedMaterial(Material material)
        {
            pressedMaterial = material;
        }

        private void ApplyIdleState()
        {
            if (idleMaterial != null && targetRenderer != null)
                targetRenderer.material = idleMaterial;
        }
    }
}