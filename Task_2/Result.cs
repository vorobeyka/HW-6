using System.Text.Json.Serialization;

namespace Task_2
{
    class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; }

        [JsonPropertyName("error")]
        public string Error { get; }

        [JsonPropertyName("duration")]
        public string Duration { get; }

        [JsonPropertyName("primes")]
        public int[] Primes { get; }

        public Result() {}

        public Result(bool success, string error, string duration, int[] primes)
        {
            Success = success;
            Error = error;
            Duration = duration;
            Primes = primes;
        }
    }
}
