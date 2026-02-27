using UnityEngine;
using TMPro;
using System.Collections;

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
        if (showText)
        {
          if (renderedText.Length != text.Length)
          {
            renderedText = text.Substring(0, renderedText.Length + 1);
            hudText.SetText(renderedText);
          }
          else
          {
              StartCoroutine("RemoveText");
          }
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(5);
        showText = false;
        renderedText = "";
        hudText.SetText("");
    }
}
