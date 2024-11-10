using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Models.Utilites.Tariff
{
    public class Tariffs
    {
        public int? Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public Bundle Bundle { get; set; }

    }
}
