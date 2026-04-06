using App1.Models;
using Microsoft.EntityFrameworkCore;

namespace App1.Data
{
    public class App1DbContext : DbContext
    {
        public App1DbContext(DbContextOptions<App1DbContext> options)
            : base(options) { }

        public DbSet<Expense> Expenses { get; set; }
    }
}