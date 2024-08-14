// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Xunet.WinFormium.Core;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Json设置
/// </summary>
public class JsonSettings
{
    /// <summary>
    /// NewtonsoftJsonOptions
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static Action<MvcNewtonsoftJsonOptions> NewtonsoftJsonOptions(string format = "yyyy-MM-dd HH:mm:ss")
    {
        return new Action<MvcNewtonsoftJsonOptions>(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            options.SerializerSettings.DateFormatString = format;
            options.SerializerSettings.Formatting = Formatting.Indented;
        });
    }

    /// <summary>
    /// SerializerSettings
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static JsonSerializerSettings SerializerSettings(string format = "yyyy-MM-dd HH:mm:ss")
    {
        var options = new MvcNewtonsoftJsonOptions();
        NewtonsoftJsonOptions(format).Invoke(options);
        return options.SerializerSettings;
    }
}
