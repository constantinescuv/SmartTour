using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartTour.Domain
{
    public class WeatherEntity
    {
        [JsonPropertyName("main")]
        public Dictionary<string, double> Main { get; set; }

    }
}
