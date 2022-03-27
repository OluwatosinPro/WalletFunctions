using WalletFunctions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
            //string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=ABOLUWARIN\\SQLEXPRESS;Initial Catalog=WalletDB;Integrated Security=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
