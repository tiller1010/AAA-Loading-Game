using UnityEngine;
using TMPro;

public class TextUpdate : MonoBehaviour
{
    public string text;
    private string renderedText = "";
    private bool showText;

    [SerializeField] TMP_Text hudText;

    private void OnTriggerEnter(Collider other)
    {
        showText = true;
    }

    void FixedUpdate()
    {
        if (showText && renderedText.Length != text.Length)
        {
            renderedText = text.Substring(0, renderedText.Length + 1);
            hudText.SetText(renderedText);
        }
    }
}
