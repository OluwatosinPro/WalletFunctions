using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletFunctions.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DirectionEnum Direction { get; set; }
        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
