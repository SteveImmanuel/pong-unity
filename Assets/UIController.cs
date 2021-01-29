using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject controlInfo;
    public Text p1AIStatus;
    public Text p2AIStatus;
    public Text p1powerStatus;
    public Text p2powerStatus;

    void Start()
    {
        SetAIStatus("Player1", false);
        SetAIStatus("Player2", false);
    }
    public void ShowControlInfo()
    {
        controlInfo.SetActive(false);
        controlInfo.SetActive(true);
    }

    public void SetPowerStatus(string tag, string status)
    {
        if (tag == "Player1")
        {
            p1powerStatus.text = "Power Up:\n" + status;
        }
        else
        {
            p2powerStatus.text = "Power Up:\n" + status;
        }
    }

    public void SetAIStatus(string tag, bool status)
    {
        Text obj;
        if (tag == "Player1")
        {
            obj = p1AIStatus;
        }
        else
        {
            obj = p2AIStatus;
        }

        if (status)
        {
            obj.text = "AI Status: ON";
        }
        else
        {
            obj.text = "AI Status: OFF";
        }
    }
}
