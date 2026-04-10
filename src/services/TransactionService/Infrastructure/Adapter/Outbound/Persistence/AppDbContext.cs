using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Models;

namespace TransactionService.Infrastructure.Adapter.Outbound.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Media> Medias => Set<Media>();
    }
}
