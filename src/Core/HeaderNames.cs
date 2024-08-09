// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

/// <summary>
/// 请求头Names
/// </summary>
public static class HeaderNames
{
    /// <summary>
    /// Accept
    /// </summary>
    public const string Accept = "Accept";
    /// <summary>
    /// Accept-Charset
    /// </summary>
    public const string AcceptCharset = "Accept-Charset";
    /// <summary>
    /// Accept-Encoding
    /// </summary>
    public const string AcceptEncoding = "Accept-Encoding";
    /// <summary>
    /// Accept-Language
    /// </summary>
    public const string AcceptLanguage = "Accept-Language";
    /// <summary>
    /// Accept-Ranges
    /// </summary>
    public const string AcceptRanges = "Accept-Ranges";
    /// <summary>
    /// Access-Control-Allow-Credentials
    /// </summary>
    public const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
    /// <summary>
    /// Access-Control-Allow-Headers
    /// </summary>
    public const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
    /// <summary>
    /// Access-Control-Allow-Methods
    /// </summary>
    public const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
    /// <summary>
    /// Access-Control-Allow-Origin
    /// </summary>
    public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
    /// <summary>
    /// Access-Control-Expose-Headers
    /// </summary>
    public const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";
    /// <summary>
    /// Access-Control-Max-Age
    /// </summary>
    public const string AccessControlMaxAge = "Access-Control-Max-Age";
    /// <summary>
    /// Access-Control-Request-Headers
    /// </summary>
    public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
    /// <summary>
    /// Access-Control-Request-Method
    /// </summary>
    public const string AccessControlRequestMethod = "Access-Control-Request-Method";
    /// <summary>
    /// Age
    /// </summary>
    public const string Age = "Age";
    /// <summary>
    /// Allow
    /// </summary>
    public const string Allow = "Allow";
    /// <summary>
    /// :authority
    /// </summary>
    public const string Authority = ":authority";
    /// <summary>
    /// Authorization
    /// </summary>
    public const string Authorization = "Authorization";
    /// <summary>
    /// Cache-Control
    /// </summary>
    public const string CacheControl = "Cache-Control";
    /// <summary>
    /// Connection
    /// </summary>
    public const string Connection = "Connection";
    /// <summary>
    /// Content-Disposition
    /// </summary>
    public const string ContentDisposition = "Content-Disposition";
    /// <summary>
    /// Content-Encoding
    /// </summary>
    public const string ContentEncoding = "Content-Encoding";
    /// <summary>
    /// Content-Language
    /// </summary>
    public const string ContentLanguage = "Content-Language";
    /// <summary>
    /// Content-Length
    /// </summary>
    public const string ContentLength = "Content-Length";
    /// <summary>
    /// Content-Location
    /// </summary>
    public const string ContentLocation = "Content-Location";
    /// <summary>
    /// Content-MD5
    /// </summary>
    public const string ContentMD5 = "Content-MD5";
    /// <summary>
    /// Content-Range
    /// </summary>
    public const string ContentRange = "Content-Range";
    /// <summary>
    /// Content-Security-Policy
    /// </summary>
    public const string ContentSecurityPolicy = "Content-Security-Policy";
    /// <summary>
    /// Content-Security-Policy-Report-Only
    /// </summary>
    public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";
    /// <summary>
    /// Content-Type
    /// </summary>
    public const string ContentType = "Content-Type";
    /// <summary>
    /// Cookie
    /// </summary>
    public const string Cookie = "Cookie";
    /// <summary>
    /// Date
    /// </summary>
    public const string Date = "Date";
    /// <summary>
    /// ETag
    /// </summary>
    public const string ETag = "ETag";
    /// <summary>
    /// Expires
    /// </summary>
    public const string Expires = "Expires";
    /// <summary>
    /// Expect
    /// </summary>
    public const string Expect = "Expect";
    /// <summary>
    /// From
    /// </summary>
    public const string From = "From";
    /// <summary>
    /// Host
    /// </summary>
    public const string Host = "Host";
    /// <summary>
    /// If-Match
    /// </summary>
    public const string IfMatch = "If-Match";
    /// <summary>
    /// If-Modified-Since
    /// </summary>
    public const string IfModifiedSince = "If-Modified-Since";
    /// <summary>
    /// If-None-Match
    /// </summary>
    public const string IfNoneMatch = "If-None-Match";
    /// <summary>
    /// If-Range
    /// </summary>
    public const string IfRange = "If-Range";
    /// <summary>
    /// If-Unmodified-Since
    /// </summary>
    public const string IfUnmodifiedSince = "If-Unmodified-Since";
    /// <summary>
    /// Last-Modified
    /// </summary>
    public const string LastModified = "Last-Modified";
    /// <summary>
    /// Location
    /// </summary>
    public const string Location = "Location";
    /// <summary>
    /// Max-Forwards
    /// </summary>
    public const string MaxForwards = "Max-Forwards";
    /// <summary>
    /// :method
    /// </summary>
    public const string Method = ":method";
    /// <summary>
    /// Origin
    /// </summary>
    public const string Origin = "Origin";
    /// <summary>
    /// :path
    /// </summary>
    public const string Path = ":path";
    /// <summary>
    /// Pragma
    /// </summary>
    public const string Pragma = "Pragma";
    /// <summary>
    /// Proxy-Authenticate
    /// </summary>
    public const string ProxyAuthenticate = "Proxy-Authenticate";
    /// <summary>
    /// Proxy-Authorization
    /// </summary>
    public const string ProxyAuthorization = "Proxy-Authorization";
    /// <summary>
    /// Range
    /// </summary>
    public const string Range = "Range";
    /// <summary>
    /// Referer
    /// </summary>
    public const string Referer = "Referer";
    /// <summary>
    /// Retry-After
    /// </summary>
    public const string RetryAfter = "Retry-After";
    /// <summary>
    /// :scheme
    /// </summary>
    public const string Scheme = ":scheme";
    /// <summary>
    /// Server
    /// </summary>
    public const string Server = "Server";
    /// <summary>
    /// Set-Cookie
    /// </summary>
    public const string SetCookie = "Set-Cookie";
    /// <summary>
    /// :status
    /// </summary>
    public const string Status = ":status";
    /// <summary>
    /// Strict-Transport-Security
    /// </summary>
    public const string StrictTransportSecurity = "Strict-Transport-Security";
    /// <summary>
    /// TE
    /// </summary>
    public const string TE = "TE";
    /// <summary>
    /// Trailer
    /// </summary>
    public const string Trailer = "Trailer";
    /// <summary>
    /// Transfer-Encoding
    /// </summary>
    public const string TransferEncoding = "Transfer-Encoding";
    /// <summary>
    /// Upgrade
    /// </summary>
    public const string Upgrade = "Upgrade";
    /// <summary>
    /// User-Agent
    /// </summary>
    public const string UserAgent = "User-Agent";
    /// <summary>
    /// Vary
    /// </summary>
    public const string Vary = "Vary";
    /// <summary>
    /// Via
    /// </summary>
    public const string Via = "Via";
    /// <summary>
    /// Warning
    /// </summary>
    public const string Warning = "Warning";
    /// <summary>
    /// Sec-WebSocket-Protocol
    /// </summary>
    public const string WebSocketSubProtocols = "Sec-WebSocket-Protocol";
    /// <summary>
    /// WWW-Authenticate
    /// </summary>
    public const string WWWAuthenticate = "WWW-Authenticate";
}
