﻿namespace Xunet.WinFormium.Core;

/// <summary>
/// 请求头
/// </summary>
public class RequestHeaders : Dictionary<string, string>
{
    /// <summary>
    /// UserAgent
    /// </summary>
    public string UserAgent
    {
        get => GetHeader(HeaderNames.UserAgent);
        set => this[HeaderNames.UserAgent] = value;
    }

    /// <summary>
    /// Referrer
    /// </summary>
    public string Referrer
    {
        get => GetHeader(HeaderNames.Referer);
        set => this[HeaderNames.Referer] = value;
    }

    /// <summary>
    /// Authorization
    /// </summary>
    public string Authorization
    {
        get => GetHeader(HeaderNames.Authorization);
        set => this[HeaderNames.Authorization] = value;
    }

    /// <summary>
    /// Cookie
    /// </summary>
    public string Cookie
    {
        get => GetHeader(HeaderNames.Cookie);
        set => this[HeaderNames.Cookie] = value;
    }

    string GetHeader(string name) => ContainsKey(name) ? this[name] : string.Empty;
}