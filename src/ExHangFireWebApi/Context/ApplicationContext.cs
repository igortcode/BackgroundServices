using ExHangFireWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ExHangFireWebApi.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arquivo>().Property(a => a.Data).HasColumnType("varbinary(max)");
            modelBuilder.Entity<Produto>().Property(a => a.Preco).HasPrecision(14,2);

            base.OnModelCreating(modelBuilder);
        }

    }
}
