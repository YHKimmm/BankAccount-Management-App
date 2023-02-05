using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ASP.Net_MVC_Assignment.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ASP.Net_MVC_Assignment.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.ClientID, ca.AccountNum });

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.Client)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.ClientID })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.BankAccount)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.AccountNum })
                .OnDelete(DeleteBehavior.Restrict);

            // Create BankAccountType table -> takes AccountType property as primary key and it is referred to BankAccount's AccountType property

            modelBuilder.Entity<BankAccountType>()
                   .Property(p => p.AccountType)
                   .HasColumnType("varchar(50)");

            modelBuilder.Entity<BankAccountType>()
                      .HasData(new BankAccountType { AccountType = "Chequing" }
                              , new BankAccountType { AccountType = "Saving" }
                              , new BankAccountType { AccountType = "Investment" }
                              , new BankAccountType { AccountType = "RRSP" }
                              , new BankAccountType { AccountType = "RESP" }
                              , new BankAccountType { AccountType = "Tax Free Savings" });

            modelBuilder.Entity<BankAccount>()
                 .HasOne<BankAccountType>(b => b.BankAccountType)
                 .WithMany(b => b.BankAccounts)
                 .HasForeignKey(fk => fk.AccountType)
                 .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BankAccount>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(9,2)");

            modelBuilder.Entity<Client>()
                .Property(p => p.FirstName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.LastName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.Email)
                .HasColumnType("varchar(50)");
        }

    }

}