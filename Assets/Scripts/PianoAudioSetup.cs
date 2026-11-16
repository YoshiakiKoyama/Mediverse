using UnityEngine;
using Just5Store.Modules.PianoGame;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PianoAudioSetup : MonoBehaviour
{
    [SerializeField] private Piano piano;

#if UNITY_EDITOR

    [ContextMenu("Assign All 88 Piano Sounds")]
    private void AssignSounds()
    {
        if (piano == null)
        {
            Debug.LogError("Piano reference is missing!");
            return;
        }

        int assigned = 0;

        for (int midi = 21; midi <= 108; midi++)
        {
            PianoKey key = piano.GetKeyByMidi(midi);

            if (key == null)
                continue;

            string path =
                $"Assets/Audio/PianoSounds/{midi:000}.mp3";

            AudioClip clip =
                AssetDatabase.LoadAssetAtPath<AudioClip>(path);

            if (clip == null)
            {
                Debug.LogWarning($"Missing audio: {path}");
                continue;
            }

            SerializedObject serialized = new SerializedObject(key);

            SerializedProperty sound =
                serialized.FindProperty("pianoSound");

            sound.objectReferenceValue = clip;

            serialized.ApplyModifiedProperties();

            assigned++;
        }

        Debug.Log($"Assigned {assigned}/88 piano sounds!");
    }

#endif
}