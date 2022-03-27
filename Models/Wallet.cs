using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletFunctions.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        [Required]
        public double Balance { get; set; } = 0;
    }
}
