using System.Text.Json.Serialization;

namespace Task_3
{
    class Result
    {
        [JsonPropertyName("successful")]
        public int Successful { get; }
        [JsonPropertyName("failed")]
        public int Failed { get; }

        public Result(int successful, int failed) 
        {
            Successful = successful;
            Failed = failed;
        }
    }
}
