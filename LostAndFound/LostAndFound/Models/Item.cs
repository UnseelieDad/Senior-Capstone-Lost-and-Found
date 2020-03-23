using System;

namespace LostAndFound.Models
{
    public class Item
    {
        public string id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime DateLost { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}