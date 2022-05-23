using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Member
{
    public User user;
    public string nick;
    public string[] roles;
    public string joined_at;
}

[Serializable]
public class User
{
    public string id;
    public string username;
    public string avatar;
    public bool bot;
    public string union_openid;
    public string union_user_account;
}

[Serializable]
public class MemberList
{
    public List<Member> members;
}