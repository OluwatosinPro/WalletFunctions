using WalletFunctions.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WalletFunctions.Repositories;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(WalletFunctions.Startup))]
namespace WalletFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer("Data Source=ABOLUWARIN\\SQLEXPRESS;Initial Catalog=WalletDB;Integrated Security=True"));

            
            builder.Services.AddTransient<IWalletRepository, WalletRepository>();
            builder.Services.AddTransient<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
