using Microsoft.EntityFrameworkCore;
using PYGS.Shared.Entities;
using PYGS.Shared.Entities.AgriSQL;

namespace PYGS.Api.Data
{
    public class AgriSQLDbContext : DbContext
    {
        public AgriSQLDbContext(DbContextOptions<AgriSQLDbContext> options) : base(options)
        {
        }

        public DbSet<GraficoProduccionAgriness> graficoProduccionAgrinesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GraficoProduccionAgriness>().ToTable("graficos_produccion_agriness");
        }
    }
}
