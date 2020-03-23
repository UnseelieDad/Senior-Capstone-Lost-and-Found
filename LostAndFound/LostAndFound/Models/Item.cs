using System;
using Newtonsoft.Json;

namespace LostAndFound.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
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