using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletFunctions.Models
{
    public class OperationResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class OperationResponse<T> : OperationResponse
    {
        public T Value { get; set; }
    }
}
