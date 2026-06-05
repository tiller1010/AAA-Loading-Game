using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private Slider healthSlider;

    void Awake()
    {
       if (healthLabel == null)
       {
         healthLabel = GameObject.Find("HealthLabel").GetComponent<Text>();
       }

       if (healthSlider == null)
       {
         healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
       }

        Messenger<int>.AddListener("PLAYER_HEALTH_UPDATED", OnHealthUpdate);
    }

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

    private void OnHealthUpdate(int newHealth)
    {
        string message = "Health: " + newHealth;
        healthLabel.text = message;
        healthSlider.value = newHealth;
    }
}
