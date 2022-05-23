using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Config : MonoBehaviour
{
    public static ConfigFile BotConfig;
    private void Awake()
    {
        GetJson();
    }
    public void GetJson()
    {
        BotConfig = LoadJsonFromFile();
    }
    private static ConfigFile LoadJsonFromFile()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/config.json");
        if (sr == null)
        {
            return null;
        }
        string json = sr.ReadToEnd();
        sr.Close();
        if (json.Length > 0)
        {
            return JsonUtility.FromJson<ConfigFile>(json);
        }
        return null;
    }
}


