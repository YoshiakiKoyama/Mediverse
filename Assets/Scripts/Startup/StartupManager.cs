using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartupManager : MonoBehaviour
{
    public CanvasGroup logo;

    private IEnumerator Start()
    {
        logo.alpha = 0f;

        // Fade in
        yield return FadeLogo(0f, 1f, 1f);

        // Display logo
        yield return new WaitForSeconds(2f);

        // Fade out
        yield return FadeLogo(1f, 0f, 1f);

        // Open main menu
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator FadeLogo(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            logo.alpha = Mathf.Lerp(
                start,
                end,
                elapsed / duration
            );

            yield return null;
        }

        logo.alpha = end;
    }
}