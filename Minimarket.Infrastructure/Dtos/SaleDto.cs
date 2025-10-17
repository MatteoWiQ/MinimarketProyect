using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Dtos
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; } = 0;
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsDone { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
