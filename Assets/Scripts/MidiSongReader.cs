using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiSongReader : MonoBehaviour
{
    [Header("MIDI File")]
    [SerializeField] private TextAsset midiFile;

    [Header("Debug")]
    [SerializeField] private bool printNotes = true;

    // Stores all the notes from the MIDI song.
    public List<MidiNoteData> Notes { get; private set; }
        = new List<MidiNoteData>();

    [Serializable]
    public class MidiNoteData
    {
        public int noteNumber;
        public float startTime;
        public float duration;
        public float endTime;
        public int velocity;
    }

    private void Awake()
    {
        LoadMidi();
    }

    public void LoadMidi()
    {
        if (midiFile == null)
        {
            Debug.LogError("Please assign a MIDI file!");
            return;
        }

        Notes.Clear();

        // Read MIDI file.
        using (MemoryStream stream = new MemoryStream(midiFile.bytes))
        {
            MidiFile midi = MidiFile.Read(stream);

            // Get tempo information.
            TempoMap tempoMap = midi.GetTempoMap();

            // Read all notes.
            foreach (var note in midi.GetNotes())
            {
                // Convert MIDI ticks into seconds.
                MetricTimeSpan start =
                    TimeConverter.ConvertTo<MetricTimeSpan>(
                        note.Time,
                        tempoMap
                    );

                // Convert note duration into seconds.
                MetricTimeSpan length =
                    LengthConverter.ConvertTo<MetricTimeSpan>(
                        note.Length,
                        note.Time,
                        tempoMap
                    );

                float startSeconds =
                    (float)start.TotalMicroseconds / 1000000f;

                float durationSeconds =
                    (float)length.TotalMicroseconds / 1000000f;

                // Create note data.
                MidiNoteData data = new MidiNoteData
                {
                    noteNumber = (int)note.NoteNumber,
                    startTime = startSeconds,
                    duration = durationSeconds,
                    endTime = startSeconds + durationSeconds,
                    velocity = (int)note.Velocity
                };

                Notes.Add(data);
            }
        }

        // Sort notes by start time.
        Notes.Sort((a, b) =>
            a.startTime.CompareTo(b.startTime));

        Debug.Log("MIDI loaded! Total notes: " + Notes.Count);

        if (printNotes)
        {
            foreach (MidiNoteData note in Notes)
            {
                Debug.Log(
                    $"Note: {note.noteNumber} | " +
                    $"Start: {note.startTime:F2}s | " +
                    $"Duration: {note.duration:F2}s | " +
                    $"End: {note.endTime:F2}s | " +
                    $"Velocity: {note.velocity}"
                );
            }
        }
    }
}