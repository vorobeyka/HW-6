using System.Text.Json.Serialization;

namespace Task_2
{
    class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("primes")]
        public int[] Primes { get; set; }

        public Result() {}
    }
}
