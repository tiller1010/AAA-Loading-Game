using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static void StartGame()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
