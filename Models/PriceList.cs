using System.Collections.Generic;

namespace Lab4.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public string? CodeBroadcast { get; set; }
        public string? NameBroadcast { get; set; }
        public string? PricePerMinute { get; set; }
        List<Registration> Registrations { get; set; } = new();
    }
}
