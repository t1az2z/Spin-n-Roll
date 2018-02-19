using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool IsGamePaused = false;

    public GameObject PauseMenuUI;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 15)
            Destroy(gameObject);
	}
    public void Resume()
    {
        Cursor.visible = false;
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause()
    {
        Cursor.visible = true;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("0 Menu");
        Time.timeScale = 1f;
        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
