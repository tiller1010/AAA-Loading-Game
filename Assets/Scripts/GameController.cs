using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text healthLabel;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gameOverText;

    private float hudVerticalCenter;
    private bool isGameOver = false;

    void Awake()
    {
        Messenger<int>.AddListener("PLAYER_HEALTH_UPDATED", OnHealthUpdate);
    }

    void Start()
    {
        InitializeHUD();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 59;

        hudVerticalCenter = Screen.height / 2 + 200;
    }

    void FixedUpdate()
    {
        if (isGameOver && gameOverText.transform.position.y != hudVerticalCenter)
        {
            float gameOverTextPositionY = gameOverText.transform.position.y - 5;
            if (gameOverTextPositionY < hudVerticalCenter)
            {
                gameOverTextPositionY = hudVerticalCenter;
            }
            Vector3 gameOverTextPosition = new Vector3(
                gameOverText.transform.position.x,
                gameOverTextPositionY,
                gameOverText.transform.position.z
            );
            gameOverText.transform.position = gameOverTextPosition;
        }
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
        InitializeHUD();
        string message = "Health: " + newHealth;
        healthLabel.text = message;
        healthSlider.value = newHealth;

        if (newHealth == 0)
        {
            isGameOver = true;
        }
    }

    void InitializeHUD()
    {
        if (healthLabel == null)
        {
            healthLabel = GameObject.Find("HealthText").GetComponent<Text>();
        }

        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
        }

        if (gameOverText == null)
        {
            gameOverText = GameObject.Find("GameOverText");
        }
    }
}
