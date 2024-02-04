using System.Text;

namespace AgileActorsDemo.Services
{
    public sealed class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(System.Text.Json.JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        { }

        public JsonContent(object value, string mediaType)
            : base(System.Text.Json.JsonSerializer.Serialize(value), Encoding.UTF8, mediaType)
        { }
    }
}

