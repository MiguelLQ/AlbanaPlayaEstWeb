using Microsoft.EntityFrameworkCore;
using albanaPlayaEst.Models;

namespace albanaPlayaEst.Data
{
    public class AlbanaDBcontext : DbContext
    {
        public AlbanaDBcontext(DbContextOptions<AlbanaDBcontext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Metodpag> Metodpags { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<Tipovehic> Tipovehics { get; set; }
        public DbSet<Vehículo> Vehículos { get; set; }
        public DbSet<Espacio> Espacios { get; set; } // Agregado DbSet para Espacio
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Cliente
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Cod_cliente);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nom_cliente)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Apell_cliente)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Tel_cliente)
                .IsRequired()
                .HasMaxLength(15);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Dni_cliente)
                .IsRequired()
                .HasMaxLength(8);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Vehículo)
                .WithOne(v => v.CodCliNavigation)
                .HasForeignKey(v => v.Cod_cliente);

            // Configuración de la entidad Metodpag
            modelBuilder.Entity<Metodpag>()
                .HasKey(m => m.CodMetd);

            modelBuilder.Entity<Metodpag>()
                .Property(m => m.DescrMetd)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Metodpag>()
                .HasMany(m => m.Pagos)
                .WithOne(p => p.CodMetdNavigation)
                .HasForeignKey(p => p.CodMetd);

            // Configuración de la entidad Pago
            modelBuilder.Entity<Pago>()
                .HasKey(p => p.cod_Pag);

            modelBuilder.Entity<Pago>()
                .Property(p => p.MontPag)
                .IsRequired();

            modelBuilder.Entity<Pago>()
                .HasMany(p => p.Registros)
                .WithOne(r => r.CodPagNavigation)
                .HasForeignKey(r => r.cod_Pag);

            // Configuración de la entidad Registro
            modelBuilder.Entity<Registro>()
                .HasKey(r => r.CodReg);

            modelBuilder.Entity<Registro>()
                .Property(r => r.FechaEntrada)
                .IsRequired(false); // Fecha de entrada es opcional

            modelBuilder.Entity<Registro>()
                .Property(r => r.FechaHoraSalida)
                .IsRequired(false); // Fecha de salida es opcional

            modelBuilder.Entity<Registro>()
                .HasOne(r => r.CodVNavigation)
                .WithMany(v => v.Registros)
                .HasForeignKey(r => r.CodV);

            modelBuilder.Entity<Registro>()
                .HasOne(r => r.CodEspNavigation)
                .WithMany(e => e.Registros)
                .HasForeignKey(r => r.CodEsp);

            modelBuilder.Entity<Registro>()
                .HasOne(r => r.CodPagNavigation)
                .WithMany(p => p.Registros)
                .HasForeignKey(r => r.cod_Pag);

            // Configuración de la entidad Tipovehic
            modelBuilder.Entity<Tipovehic>()
                .HasKey(t => t.CodTipV);

            modelBuilder.Entity<Tipovehic>()
                .Property(t => t.DescrTipV)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Tipovehic>()
                .HasMany(t => t.Vehículos)
                .WithOne(v => v.CodTipVNavigation)
                .HasForeignKey(v => v.CodTipV);

            // Configuración de la entidad Vehículo
            modelBuilder.Entity<Vehículo>()
                .HasKey(v => v.CodV);

            modelBuilder.Entity<Vehículo>()
                .Property(v => v.MarcV)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Vehículo>()
                .Property(v => v.ModelV)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Vehículo>()
                .Property(v => v.ColorV)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Vehículo>()
                .Property(v => v.PlacaV)
                .IsRequired()
                .HasMaxLength(8);

            modelBuilder.Entity<Vehículo>()
                .HasOne(v => v.CodTipVNavigation)
                .WithMany(t => t.Vehículos)
                .HasForeignKey(v => v.CodTipV);

            modelBuilder.Entity<Vehículo>()
                .HasOne(v => v.CodCliNavigation)
                .WithMany(c => c.Vehículo)
                .HasForeignKey(v => v.Cod_cliente);

            modelBuilder.Entity<Vehículo>()
                .HasMany(v => v.Registros)
                .WithOne(r => r.CodVNavigation)
                .HasForeignKey(r => r.CodV);

            // Configuración de la entidad Espacio
            modelBuilder.Entity<Espacio>()
                .HasKey(e => e.Cod_esp);

            modelBuilder.Entity<Espacio>()
                .Property(e => e.Ubi_esp)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Espacio>()
                .Property(e => e.Cost_esp)
                .IsRequired();

            modelBuilder.Entity<Espacio>()
                .HasMany(e => e.Registros)
                .WithOne(r => r.CodEspNavigation)
                .HasForeignKey(r => r.CodEsp);
        }
    }
}
