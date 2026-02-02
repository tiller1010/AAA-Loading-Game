using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 59;
    }
}
