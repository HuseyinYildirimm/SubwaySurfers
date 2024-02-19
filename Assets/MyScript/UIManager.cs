using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
        Debug.Log("SampleScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        Time.timeScale = 0f;
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene("StartScene");
        Debug.Log("StartScene");
    }

    public void ContinueButton()
    {
        Time.timeScale = 1f;
    }
    
}
