using System;
using System.Collections.Generic;

[Serializable]
public class Payload
{
    public int op;
    public PayloadDetail d = new PayloadDetail();
    public int s;
    public string t;
}

[Serializable]
public class PayloadDetail
{
    public int heartbeat_interval;
    public string token = "";
    public int intents;
    public int[] shard;
    public int version;
    public string session_id;
    public User user;
    public List<User> mentions;
    public User author;
    public string content;
    public string id;
}

[Serializable]
public class Heartbeat
{
    public int op = 1;
    public int d;
}