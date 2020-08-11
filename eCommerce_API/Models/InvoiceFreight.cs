using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce_API.Models
{
    public partial class InvoiceFreight
    {
        public int Id { get; set; }
        [ForeignKey("orderId")]
        public Orders order { get; set; }
        public int orderId { get; set; }
        public int InvoiceNumber { get; set; }
        public string ShipName { get; set; }
        public string ShipDesc { get; set; }
        public string Ticket { get; set; }
        public decimal Price { get; set; }
        public int? ShipId { get; set; }

    }
}
