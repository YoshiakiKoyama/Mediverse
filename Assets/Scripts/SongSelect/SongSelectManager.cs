using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class SongSelectManager : MonoBehaviour
{
    [Header("Song List")]
    [SerializeField] private Transform songListContent;
    [SerializeField] private Button songButtonPrefab;

    private void Start()
    {
        LoadSongs();
    }

    private void LoadSongs()
    {
        string midiFolder = Path.Combine(Application.dataPath, "Midi");

        if (!Directory.Exists(midiFolder))
        {
            Debug.LogError("MIDI folder not found: " + midiFolder);
            return;
        }

        string[] midiFiles = Directory.GetFiles(midiFolder, "*.mid.bytes");

        foreach (string midiFile in midiFiles)
        {
            Button button = Instantiate(songButtonPrefab, songListContent);

            string songName = Path.GetFileNameWithoutExtension(midiFile)
            .Replace(".mid.bytes", "");

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text = songName;
            }

            string selectedPath = midiFile;

            button.onClick.AddListener(() => PlaySong(selectedPath));
        }

        Debug.Log("Loaded " + midiFiles.Length + " MIDI songs.");
    }

    private void PlaySong(string midiPath)
    {
        Debug.Log("Selected MIDI: " + midiPath);

        SongSelection.SelectedMidiPath = midiPath;

        SceneManager.LoadScene("GameplayScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}