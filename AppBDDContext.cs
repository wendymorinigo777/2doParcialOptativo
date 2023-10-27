using Microsoft.EntityFrameworkCore;

namespace OptativoSegundoParcial.Models
{
    public class AppBDDContext : BDDContext
    {
        public AppBDDContext(BDDContextOptions<AppBDDContext> options) : base(options)
        {
        }

        public BDDSet<Ciudad> Ciudades { get; set; }
        public BDDSet<Persona> Personas { get; set; }
        public BDDSet<Cliente> Clientes { get; set; }
        public BDDSet<Cuentas> Cuentas { get; set; }
        public BDDSet<Movimientos> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciudad>()
                .HasKey(c => c.id_Ciudad);

            modelBuilder.Entity<Persona>()
                .HasKey(p => p.id_Persona);

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.id_Cliente);

            modelBuilder.Entity<Cuentas>()
                .HasKey(c => c.id_Cuenta);

            modelBuilder.Entity<Movimientos>()
                .HasKey(m => m.id_Movimiento);
        }
    }

}
