using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWebSocket;

public class QQBotWebSocket : QQBotController
{
    private IWebSocket socket;
    private bool isConnected = false;
    private int receiveCount = 0;
    private int sequenceNum;
    private string session_id;
    private float heartbeat_interval;
    private float time;
    private PayloadController payloadController;

    private void Awake()
    {
        log = GameObject.Find("Canvas/WebSocketLog/Viewport/Content/Log").GetComponent<Text>();
        logStr = log.text;
        payloadController = new PayloadController();
    }
    public void Connect()
    {
        config = Config.BotConfig;
        socket = new WebSocket(config.BotWssAddr);
        socket.OnOpen += Socket_OnOpen;
        socket.OnMessage += Socket_OnMessage;
        socket.OnClose += Socket_OnClose;
        socket.OnError += Socket_OnError;
        AddLog(string.Format("Connecting..."));
        socket.ConnectAsync();
    }
    private void Socket_OnOpen(object sender, OpenEventArgs e)
    {
        AddLog(string.Format("Connected: {0}", config.BotWssAddr));
    }

    private void Socket_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            AddLog(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
        }
        else if (e.IsText)
        {
            // AddLog(string.Format("Receive: {0}", e.Data));
            Payload payload = JsonUtility.FromJson<Payload>(e.Data);
            if (payload.s != 0)
            {
                sequenceNum = payload.s;
            }

            switch (payload.op)
            {
                case 0:
                    AddLog("收到服务器消息推送");
                    StoreReadyEvent(payload);
                    payloadController.SolvePayloadByType(payload);
                    break;
                case 2:
                    AddLog($"心跳 d = {payload.d}");
                    break;
                case 10:
                    AddLog("连接到Gateway");
                    StoreGateway(payload);
                    Authorize(512);
                    break;
                case 11:
                    AddLog("收到Heartbeat ACK");
                    break;
                default:
                    AddLog($"无法解析的op: {payload.op}");
                    break;
            }
        }
        receiveCount += 1;
    }
    private void StoreReadyEvent(Payload payload)
    {
        session_id = payload.d.session_id;
        isConnected = true;
        AddLog("链接已建立");
    }
    private void StoreGateway(Payload payload)
    {
        heartbeat_interval = payload.d.heartbeat_interval / 1000;
        Debug.Log(heartbeat_interval);
        AddLog("保存心跳间隔");
    }
    private void Authorize(int intents)
    {
        Payload payload = new Payload();
        payload.op = 2;
        payload.d.token = config.Authorization();
        payload.d.intents = intents;

        string jsonPayload = JsonUtility.ToJson(payload);
        socket.SendAsync(jsonPayload);
        AddLog("发送鉴权信息完毕");
    }
    private void Socket_OnClose(object sender, CloseEventArgs e)
    {
        AddLog(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        isConnected = false;
    }

    private void Socket_OnError(object sender, ErrorEventArgs e)
    {
        AddLog(string.Format("Error: {0}", e.Message));
    }
    private void Update()
    {
        if (isConnected)
        {
            time += Time.deltaTime;
            if (time >= heartbeat_interval - 5)
            {
                SendHeartbeat();
                time = 0f;
            }
        }
    }
    private void SendHeartbeat()
    {
        Heartbeat heartbeat = new Heartbeat();
        heartbeat.d = sequenceNum;

        string jsonHeartbeat = JsonUtility.ToJson(heartbeat);
        socket.SendAsync(jsonHeartbeat);
    }
}
