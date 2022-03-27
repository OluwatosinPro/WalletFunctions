using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletFunctions.Data;
using WalletFunctions.Models;

namespace WalletFunctions.Repositories
{
    public interface IWalletRepository
    {
        Task<OperationResponse<Wallet>> CreateAsync(Wallet model);
    }

    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public WalletRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<OperationResponse<Wallet>> CreateAsync(Wallet model)
        {
            var result = await applicationDbContext.Wallets.AddAsync(model);
            await applicationDbContext.SaveChangesAsync();

            return new OperationResponse<Wallet>
            {
                IsSuccess = true,
                Message = "Wallet created successfully",
                Value = result.Entity
            };
        }

    }
}
