using System.Collections.Generic;
using UnityEngine;
using Just5Store.Modules.PianoGame;

public class MidiAutoPlayer : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] private Piano piano;
    [SerializeField] private MidiSongReader midiReader;
    [SerializeField] private SongClock songClock;

    [Header("Settings")]
    [SerializeField] private bool autoplay = true;

    // MIDI notes and playback progress
    private List<MidiSongReader.MidiNoteData> notes;
    private int nextNoteIndex = 0;

    // Currently held notes
    private List<MidiSongReader.MidiNoteData> activeNotes =
        new List<MidiSongReader.MidiNoteData>();

    private void Start()
    {
        // Validate references
        if (piano == null || midiReader == null || songClock == null)
        {
            Debug.LogError("MidiAutoPlayer: Missing references!");
            enabled = false;
            return;
        }

        // Get notes from MIDI reader
        notes = midiReader.Notes;
    }

    private void Update()
    {
        if (!autoplay)
            return;

        double currentTime = songClock.SongTime;

        // Release finished notes
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            var note = activeNotes[i];

            if (currentTime >= note.endTime)
            {
                piano.Release(note.noteNumber);
                activeNotes.RemoveAt(i);
            }
        }

        // Play notes when their start time arrives
        while (nextNoteIndex < notes.Count &&
               currentTime >= notes[nextNoteIndex].startTime)
        {
            var note = notes[nextNoteIndex];

            piano.Press(note.noteNumber);
            activeNotes.Add(note);

            nextNoteIndex++;
        }
    }

    private void OnDisable()
    {
        // Release held notes when disabled
        if (piano != null)
        {
            foreach (var note in activeNotes)
            {
                piano.Release(note.noteNumber);
            }
        }

        activeNotes.Clear();
    }
}