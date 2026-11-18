using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // PLAY button
    public void PlayGame()
    {

        Debug.Log("Starting game...");
    }

    // SETTINGS button
    public void OpenSettings()
    {
        Debug.Log("Settings button clicked!");
    }

    // EXIT button
    public void ExitGame()
    {
        Debug.Log("Exiting Mediverse...");
        Application.Quit();
    }
}