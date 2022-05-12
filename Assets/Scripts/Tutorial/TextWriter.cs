using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    private Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;
    Utils.ActionsController actionsController = new Utils.ActionsController();
    public void AddWriter(Text uiText, string textToWrite,float timePerCharacter)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
        TutorialManager.Instance.SetTextWritingOver(false);
    }
    private void Update()
    {
        if(uiText!= null)
        {
            
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                timer += timePerCharacter;
                characterIndex++;
                uiText.text = textToWrite.Substring(0, characterIndex);

                if (characterIndex >= textToWrite.Length)
                {
                    uiText = null;
                    TutorialManager.Instance.SetTextWritingOver(true);

                    actionsController.CallOnlyOnce(TutorialManager.Instance.complete);
                    return;
                }
            }
        }
    }
    
}
