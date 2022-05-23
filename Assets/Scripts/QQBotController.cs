using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class QQBotController : MonoBehaviour
{
    protected ConfigFile config;
    protected Text log;
    protected string logStr = "";
    protected bool logMessage = true;
    public void AddLog(string str)
    {
        if (!logMessage) return;
        if (str.Length > 100) str = str.Substring(0, 100) + "...";
        logStr += str + "\n";
        if (logStr.Length > 22 * 1024)
        {
            logStr = logStr.Substring(logStr.Length - 22 * 1024);
        }
        log.text = logStr;
    }
    public void ClearLog()
    {
        logStr = "";
        log.text = logStr;
    }
}
