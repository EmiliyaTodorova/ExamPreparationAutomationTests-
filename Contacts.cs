using System.Text.Json.Serialization;

namespace APITests
{
    internal class Contacts
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("firstName")]
        public int firstName { get; set; }

        [JsonPropertyName("lastName")]
        public int lastName { get; set; }

        [JsonPropertyName("email")]
        public int email { get; set; }

        [JsonPropertyName("phone")]
        public int phone { get; set; }

        [JsonPropertyName("dateCreate")]
        public int dateCreate { get; set; }

        [JsonPropertyName("comments")]
        public int comments { get; set; }

    }
}