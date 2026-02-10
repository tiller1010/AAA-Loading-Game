using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 59;
    }

    public static void NextLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string sceneNumberRegex = @"\d+";
        Match sceneNumberMatches = Regex.Match(activeScene.name, sceneNumberRegex);
        int activeSceneNumber = int.Parse(sceneNumberMatches.Value);
        int nextSceneNumber = activeSceneNumber + 1;
        SceneManager.LoadScene("Level" + nextSceneNumber);
    }

    public static void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
