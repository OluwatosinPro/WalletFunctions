using WalletFunctions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using Microsoft.Extensions.Configuration;

namespace WalletFunctions.Data
{
    public class ApplicationDbContext : DbContext //Add database without Identity
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Very important for application to run/work(but can be placed anywhere within OnModelCreating)

            modelBuilder.Entity<Account>().Navigation(x => x.Wallet).AutoInclude();
            modelBuilder.Entity<Transaction>().Navigation(x => x.Account).AutoInclude();
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }


    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            /* Environment.GetEnvironmentVariable("SqlConnectionString") not working here, 
             * so the below gotten from https://www.tomfaltesek.com/azure-functions-local-settings-json-and-source-control/ is used
            */
            var config = new ConfigurationBuilder()
            //.SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
            var connectionString = config["Values:SqlConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
