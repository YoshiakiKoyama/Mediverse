using UnityEngine;
using UnityEngine.InputSystem;

namespace Just5Store.Modules.PianoGame
{
    public class PianoDemoInput : MonoBehaviour
    {
        [Header("Piano")]
        [SerializeField] private Piano piano;

        [Header("Rhythm Game")]
        [SerializeField] private RhythmJudge rhythmJudge;

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            // When Spacebar is pressed
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // Activate C4 (MIDI note 60)
                piano.Press(60);

                // Check timing accuracy
                if (rhythmJudge != null)
                {
                    rhythmJudge.JudgeNote(60);
                }
            }

            // When Spacebar is released
            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                // Release C4
                piano.Release(60);
            }
        }
    }
}