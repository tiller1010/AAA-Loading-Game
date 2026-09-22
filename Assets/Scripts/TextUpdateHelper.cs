using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextUpdateHelper
{
    public string text;
    public List<TextData> additionalTexts = new List<TextData>();
    private int additionalTextIndex = 0;
    private float additionalTextDelay = 0;

    private string renderedText = "";
    private bool showText;

    public TMP_Text hudText;

    public async Awaitable FixedUpdate()
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
            await RemoveText();
          }
        }
    }

    async Awaitable RemoveText()
    {
        await Awaitable.WaitForSecondsAsync(5);
        renderedText = "";
        hudText.SetText("");
        if (additionalTexts.Count > additionalTextIndex)
        {
            TextData additionalText = additionalTexts[additionalTextIndex];
            text = additionalText.text;
            additionalTextDelay = additionalText.delay;
            additionalTextIndex++;
            await DelayedText();
        }
    }

    async Awaitable DelayedText()
    {
        await Awaitable.WaitForSecondsAsync(additionalTextDelay);
        showText = true;
    }

    public void SetText(string newText)
    {
        text = newText;
        renderedText = "";
        showText = true;
    }

}

