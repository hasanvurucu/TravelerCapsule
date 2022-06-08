using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CapsuleBot;
    [SerializeField]
    private Text stateButtonText;

    public void ChangeState()
    {
        var botScript = CapsuleBot.GetComponent<CommandControlBot>();
        if (botScript.botState == BotState.QUEUE)
        {
            botScript.botState = BotState.STANDART;
            stateButtonText.text = "Current State:\nSTANDART";
        }
        else if (botScript.botState == BotState.STANDART)
        {
            botScript.botState = BotState.QUEUE;
            stateButtonText.text = "Current State:\nQUEUE";
        }
    }
}
