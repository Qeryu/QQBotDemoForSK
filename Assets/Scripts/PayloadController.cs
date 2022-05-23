using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class PayloadController
{
    public void SolvePayloadByType(Payload payload)
    {
        switch (payload.t)
        {
            case "MESSAGE_CREATE":
                if (checkBotMentioned(payload.d.mentions))
                {
                    SolveAtPayloadByContent(payload.d.content);
                }
                break;
            default:
                break;
        }
    }

    private void SolveAtPayloadByContent(string content)
    {
        QQBotHttps qQBotHttps = GameObject.Find("QQBotHttps").GetComponent<QQBotHttps>();
        content = content.Substring(24);

        string sunny = "晴天";
        string rainy = "雨天";
        if (String.Compare(sunny, content) == 0)
        {
            qQBotHttps.AddLog("晴天，是sk要的晴天");
        }
        else if (String.Compare(rainy, content) == 0)
        {
            qQBotHttps.AddLog("雨天，是sk要的雨天");

        }
    }

    private bool checkBotMentioned(List<User> mentions)
    {
        foreach (User user in mentions)
        {
            if (user.id == Config.BotConfig.BotID)
            {
                return true;
            }
        }
        return false;
    }
}