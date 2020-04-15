using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LostAndFound.Models
{
    public class MatchedItem
    {
        [JsonProperty("found_id")]
        public int FoundId { get; set; }

        [JsonProperty("found_email")]
        public string FoundEmail { get; set; }

        [JsonProperty("found_description")]
        public string FoundDescription { get; set; }

        [JsonProperty("found_create_date")]
        public DateTime FoundCreateDate { get; set; }

        [JsonProperty("found_color")]
        public string FoundColor { get; set; }

        [JsonProperty("found_type")]
        public string FoundType { get; set; }

        [JsonProperty("found_location")]
        public string FoundLocation { get; set; }

        [JsonProperty("date_found")]
        public DateTime DateFound { get; set; }

        [JsonProperty("lost_id")]
        public int LostId { get; set; }

        [JsonProperty("lost_email")]
        public string LostEmail { get; set; }

        [JsonProperty("lost_description")]
        public string LostDescription { get; set; }

        [JsonProperty("lost_create_date")]
        public DateTime LostCreateDate { get; set; }

        [JsonProperty("lost_color")]
        public string LostColor { get; set; }

        [JsonProperty("lost_type")]
        public string LostType { get; set; }

        [JsonProperty("lost_location")]
        public string LostLocation { get; set; }

        [JsonProperty("date_lost")]
        public DateTime DateLost { get; set; }
    }
}
