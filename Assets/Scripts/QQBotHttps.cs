using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class QQBotHttps : QQBotController
{
    private void Awake()
    {
        log = GameObject.Find("Canvas/HttpsLog/Viewport/Content/Log").GetComponent<Text>();
        logStr = log.text;
    }
    public async void GetMembers()
    {
        config = Config.BotConfig;
        string url = config.BotHttpsAddr;
        url += "/guilds";
        url += "/" + config.guild_id;
        url += "/members";

        List<HttpsQuery> queries = new List<HttpsQuery>();
        HttpsQuery q1 = new HttpsQuery("limit", "20");
        queries.Add(q1);
        url = AddGetHttpsQuery(url, queries);

        using UnityWebRequest www = UnityWebRequest.Get(url);

        www.SetRequestHeader("Authorization", config.Authorization());

        var operations = www.SendWebRequest();

        while (!operations.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string wrapedJson = WrapMembers(www.downloadHandler.text);
            MemberList memberList = JsonUtility.FromJson<MemberList>(wrapedJson);
            AddLog($"Success: {memberList.members.Count}");

            foreach (Member member in memberList.members)
            {
                AddLog($"username: {member.nick}");
                if (member.user.bot == true)
                {
                    AddLog("\t这是个Bot");
                }
            }
        }
        else
        {
            AddLog($"Failed: {www.error}");
        }
    }
    private string AddGetHttpsQuery(string url, List<HttpsQuery> queries)
    {
        url += "?";
        foreach (HttpsQuery httpsQuery in queries)
            url += httpsQuery.name + "=" + httpsQuery.value;
        return url;
    }
    private string WrapMembers(string Members)
    {
        return "{" + "\"members\":" + Members + "}";
    }

    public async void SendMessages(string messageContent)
    {
        config = Config.BotConfig;
        string url = config.BotHttpsAddr;
        url += "/channels";
        url += "/" + config.channel_id;
        url += "/messages";
        Debug.Log(url);

        BotMessage message = new BotMessage();
        message.content = messageContent.Length == 0 ? "skkkkk我是你的粉丝呀" : messageContent;
        string jsonMessage = JsonUtility.ToJson(message);
        Debug.Log(jsonMessage);
        byte[] byteJsonMessage = System.Text.Encoding.UTF8.GetBytes(jsonMessage);

        var www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(byteJsonMessage);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SetRequestHeader("Authorization", config.Authorization());
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");

        var operations = www.SendWebRequest();

        while (!operations.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AddLog($"Send Message: {message.content}");
            AddLog($"Success: {www.downloadHandler.text}");
        }
        else
        {
            AddLog($"Failed: {www.error}");
            AddLog($"Failed Detail: {www.downloadHandler.text}");
        }
    }
}
