using System;
using Newtonsoft.Json;

namespace LostAndFound.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonIgnore]
        public string Text { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("date_lost")]
        public DateTime DateLost { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }
    }
}