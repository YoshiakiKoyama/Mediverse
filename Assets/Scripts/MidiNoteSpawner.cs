using UnityEngine;
using Just5Store.Modules.PianoGame;

public class MidiNoteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Piano piano;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private MidiSongReader midiReader;
    [SerializeField] private SongClock songClock;

    [Header("Note Settings")]
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float hitLineY = 0f;

    private void Start()
    {
        SpawnNotes();
    }

    private void SpawnNotes()
    {
        if (piano == null || notePrefab == null ||
            midiReader == null || songClock == null)
        {
            Debug.LogError("MidiNoteSpawner: Missing references!");
            return;
        }

        foreach (var note in midiReader.Notes)
        {
            // Find the corresponding piano key
            PianoKey key = piano.GetKeyByMidi(note.noteNumber);

            if (key == null)
                continue;

            // Create falling note
            GameObject obj = Instantiate(notePrefab);

            // Calculate note height from MIDI duration
            float height = Mathf.Max(0.1f, note.duration * fallSpeed);

            obj.transform.localScale = new Vector3(1f, height, 5f);

            // Place note at its current position on the song timeline
            float y = hitLineY
                + (note.startTime - (float)songClock.SongTime) * fallSpeed
                + height / 2f;

            obj.transform.position = new Vector3(
                key.transform.position.x,
                y,
                key.transform.position.z
            );

            // Add falling movement
            FallingNote falling = obj.GetComponent<FallingNote>();

            if (falling == null)
                falling = obj.AddComponent<FallingNote>();

            falling.Initialize(
                note.startTime,
                fallSpeed,
                hitLineY,
                songClock
            );
        }
    }
}