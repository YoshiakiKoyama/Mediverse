using System.Collections.Generic;
using UnityEngine;

public class RhythmJudge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MidiSongReader midiReader;
    [SerializeField] private SongClock songClock;

    [Header("Judgment Settings")]
    [SerializeField] private float perfectWindow = 0.05f;
    [SerializeField] private float greatWindow = 0.10f;
    [SerializeField] private float goodWindow = 0.15f;

    // Tracks notes that have already been judged
    private HashSet<int> judgedNotes = new HashSet<int>();

    private void Update()
    {
        CheckMissedNotes();
    }

    // Called when the player presses a piano key
    public void JudgeNote(int midiNote)
    {
        if (midiReader == null || songClock == null)
            return;

        double currentTime = songClock.SongTime;

        List<MidiSongReader.MidiNoteData> notes = midiReader.Notes;

        int closestIndex = -1;
        float closestDifference = float.MaxValue;

        // Find the closest unjudged note
        for (int i = 0; i < notes.Count; i++)
        {
            if (judgedNotes.Contains(i))
                continue;

            if (notes[i].noteNumber != midiNote)
                continue;

            float difference = Mathf.Abs(
                (float)(currentTime - notes[i].startTime)
            );

            if (difference < closestDifference)
            {
                closestDifference = difference;
                closestIndex = i;
            }
        }

        // If no note is close enough
        if (closestIndex == -1 || closestDifference > goodWindow)
        {
            Debug.Log("MISS! Wrong timing.");
            return;
        }

        // Mark the note as judged
        judgedNotes.Add(closestIndex);

        // Determine accuracy
        if (closestDifference <= perfectWindow)
        {
            Debug.Log("PERFECT!");
        }
        else if (closestDifference <= greatWindow)
        {
            Debug.Log("GREAT!");
        }
        else
        {
            Debug.Log("GOOD!");
        }
    }

    // Automatically detect notes that were not played
    private void CheckMissedNotes()
    {
        if (midiReader == null || songClock == null)
            return;

        double currentTime = songClock.SongTime;

        List<MidiSongReader.MidiNoteData> notes = midiReader.Notes;

        for (int i = 0; i < notes.Count; i++)
        {
            if (judgedNotes.Contains(i))
                continue;

            // Only judge C4 for now
            if (notes[i].noteNumber != 60)
                continue;

            // Check whether the note's timing window has expired
            if (currentTime > notes[i].startTime + goodWindow)
            {
                judgedNotes.Add(i);

                Debug.Log("MISS! C4 was not pressed.");
            }
        }
    }
}