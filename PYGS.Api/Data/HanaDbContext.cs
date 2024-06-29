using Microsoft.EntityFrameworkCore;
using PYGS.Shared.Entities;
using PYGS.Shared.Entities.HANASQL;

namespace PYGS.Api.Data
{
    public class HanaDbContext : DbContext
    {
        public HanaDbContext(DbContextOptions<HanaDbContext> options) : base(options)
        {
        }

        public DbSet<ProgramacionCargaCerdos> ProgramacionCargaCerdos { get; set; }

        public DbSet<ProgramacionCerdoDetalle> programacionCerdoDetalles { get; set; }

        public DbSet<ProgramacionCerdoMensual> programacionCerdoMensuals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProgramacionCargaCerdos>().ToTable("programacion_carga_cerdos");
            modelBuilder.Entity<ProgramacionCerdoDetalle>().ToTable("programacion_cerdos_detalle");
            modelBuilder.Entity<ProgramacionCerdoMensual>().ToTable("programacion_cerdos_mensual");
        }
    }
}
