using ComicShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Annotation = System.ComponentModel.DataAnnotations;

namespace ComicShop
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ActOfSale> ActOfSales { get; set; }
        public DbSet<ComicStorage> ComicStorages { get; set; }
        public DbSet<Delivery> Deliverys { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Comic> Comics { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ComicSgop;Username=postgres;Password=333");
        }

        public static bool validData(Object args)
        {
            var results = new List<Annotation.ValidationResult>();
            var context = new ValidationContext(args);
            if (!Validator.TryValidateObject(args, context, results, true))
            {
                string message = "";
                foreach (var error in results)
                {
                    message += error.ErrorMessage + '\n';
                }
                //MessageBox.Show(message);
                return false;
            }
            return true;
        }
    }
}
