using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlotMachine.Models
{
    public class GameModel
    {
        public decimal Balance { get; set; }
        public decimal CurrentBet { get; set; }
        public List<string> SpinResult { get; set; }

        [JsonConverter(typeof(PayoutTableConverter))]
        public Dictionary<string, decimal> PayoutTable { get; set; }

        public Dictionary<string, double> SymbolWeights { get; } = new Dictionary<string, double>
        {
            {"D", 50},
            {"C", 30},
            {"B", 15},
            {"A", 5}
        };

        public GameModel()
        {
            Balance = 1000;
            CurrentBet = 10;
            SpinResult = new List<string>();
            PayoutTable = new Dictionary<string, decimal>
            {
                {"AAA", 1000},
                {"BBB", 500},
                {"CCC", 100},
                {"DDD", 10}
            };
        }
    }

    public class PayoutTableConverter : JsonConverter<Dictionary<string, decimal>>
    {
        public override Dictionary<string, decimal> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;
                var dictionary = new Dictionary<string, decimal>();
                foreach (var property in root.EnumerateObject())
                {
                    dictionary.Add(property.Name, property.Value.GetDecimal());
                }
                return dictionary;
            }
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, decimal> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WriteNumber(kvp.Key, kvp.Value);
            }
            writer.WriteEndObject();
        }
    }
}