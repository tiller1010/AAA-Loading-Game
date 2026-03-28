using UnityEngine;

public class Invisible : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }
}
