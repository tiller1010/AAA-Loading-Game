using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextUpdate : MonoBehaviour
{
    public string text;
    public List<TextData> additionalTexts = new List<TextData>();
    private int additionalTextIndex = 0;
    private float additionalTextDelay = 0;

    private string renderedText = "";
    private bool showText;

    [SerializeField] TMP_Text hudText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            showText = true;

            // Disable the collider to prevent multiple triggers
            GetComponent<Collider>().enabled = false;
        }
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
            showText = false;
            StartCoroutine("RemoveText");
          }
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(5);
        renderedText = "";
        hudText.SetText("");
        if (additionalTexts.Count > additionalTextIndex)
        {
            TextData additionalText = additionalTexts[additionalTextIndex];
            text = additionalText.text;
            additionalTextDelay = additionalText.delay;
            additionalTextIndex++;
            StartCoroutine("DelayedText");
        }
    }

    IEnumerator DelayedText()
    {
        yield return new WaitForSeconds(additionalTextDelay);
        showText = true;
    }
}
