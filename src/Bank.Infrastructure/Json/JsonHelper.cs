using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bank.Infrastructure.Json;

public static class JsonHelper
{
    /// <summary>
    /// Case insensitive, camel case, because JsonSerializerDefaults.Web set this
    /// </summary>
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, DefaultSerializerOptions);
    }

    public static string Serialize(object value, JsonSerializerOptions serializerOptions)
    {
        return JsonSerializer.Serialize(value, serializerOptions);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, DefaultSerializerOptions)!;
    }

    public static T Deserialize<T>(string json, JsonSerializerOptions serializerOptions)
    {
        return JsonSerializer.Deserialize<T>(json, serializerOptions)!;
    }
}
