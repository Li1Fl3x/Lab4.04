using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public DateTime? DateBroadcast { get; set; }
        public string? CodeBroadcast { get; set; }
        public string? NameBroadcast { get; set; }
        public string? Regularity { get; set; }
        public string? TimeOnBroadcast { get; set; }
        public string? CostBroadcast { get; set; }
        public int PriceListId { get; set; }
        public PriceList? PriceList { get; set; }
    }
}
