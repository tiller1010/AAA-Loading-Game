using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            Debug.Log("Player entered exit");
            //GameObject controller = gameObject.Find("GameController");
            //if (controller != null)
            //{
            //    //controller.NextLevel();
            //}

            // It looks like managers would just call the script component directly
            GameController.NextLevel();
        }
    }
}
