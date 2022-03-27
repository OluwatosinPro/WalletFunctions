using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletFunctions.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
