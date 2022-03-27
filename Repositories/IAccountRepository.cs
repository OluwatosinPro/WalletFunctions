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
    public interface IAccountRepository
    {
        Task<OperationResponse<Account>> CreateAsync(Account model);
        Task<OperationResponse<Account>> GetByIdAsync(int Id);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public AccountRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<OperationResponse<Account>> CreateAsync(Account model)
        {
            var result = await applicationDbContext.Accounts.AddAsync(model);
            await applicationDbContext.SaveChangesAsync();

            return new OperationResponse<Account>
            {
                IsSuccess = true,
                Message = "Account created successfully",
                Value = result.Entity
            };
        }

        public async Task<OperationResponse<Account>> GetByIdAsync(int Id)
        {
            var result = await applicationDbContext.Accounts
                            .FirstOrDefaultAsync(i => i.Id == Id);

            if (result == null)
                return new OperationResponse<Account> { IsSuccess = false, Message = "Account cannot be found" };

            return new OperationResponse<Account>
            {
                Message = "Account retrieved successfully!",
                IsSuccess = true,
                Value = result
            };
        }
    }
}
