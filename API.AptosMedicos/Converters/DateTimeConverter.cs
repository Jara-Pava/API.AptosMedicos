using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.AptosMedicos.Converters;

/// <summary>
/// Serializa DateTime? como "yyyy-MM-dd" (sin hora) en el JSON.
/// </summary>
public class DateOnlyConverter : JsonConverter<DateTime?>
{
    private const string Formato = "yyyy-MM-dd";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is string s ? DateTime.Parse(s) : null;

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString(Formato));
        else
            writer.WriteNullValue();
    }
}

