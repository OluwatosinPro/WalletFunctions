using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletFunctions.Data;
using WalletFunctions.Models;

namespace WalletFunctions.Repositories
{
    public interface ITransactionRepository
    {
        Task<OperationResponse<Transaction>> GetByAccountIdAsync(int accountId);
        Task<OperationResponse<Transaction>> CreateAsync(Transaction model);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public TransactionRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<OperationResponse<Transaction>> CreateAsync(Transaction model)
        {
            var result = await applicationDbContext.Transactions.AddAsync(model);
            await applicationDbContext.SaveChangesAsync();

            return new OperationResponse<Transaction>
            {
                IsSuccess = true,
                Message = "Transaction created successfully",
                Value = result.Entity
            };
        }

        public async Task<OperationResponse<Transaction>> GetByAccountIdAsync(int accountId)
        {
            var result = await applicationDbContext.Transactions
                            .FirstOrDefaultAsync(i => i.AccountId == accountId);

            if (result == null)
                return new OperationResponse<Transaction> { IsSuccess = false, Message = "Transaction with the account Id cannot be found" };

            return new OperationResponse<Transaction>
            {
                Message = "Transaction retrieved successfully!",
                IsSuccess = true,
                Value = result
            };
        }
    }
}
