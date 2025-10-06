using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace app_inventario2.Models;

public partial class AppInventarioContext : DbContext
{
    public AppInventarioContext()
    {
    }

    public AppInventarioContext(DbContextOptions<AppInventarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FacturaLinea> FacturaLineas { get; set; }

    public virtual DbSet<MovimientoStock> MovimientoStocks { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<VStock> VStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       // => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; database=app_inventario; integrated security=true;");//

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.AlmacenId).HasName("PK__Almacen__022A087640E64E3E");

            entity.ToTable("Almacen");

            entity.HasIndex(e => e.Codigo, "UQ__Almacen__06370DAC32E0B15B").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__71ABD087ECC2CA1D");

            entity.ToTable("Cliente");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Factura__5C02486561A79A65");

            entity.ToTable("Factura");

            entity.HasIndex(e => e.Numero, "UQ__Factura__7E532BC6890790D6").IsUnique();

            entity.Property(e => e.Estado)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasDefaultValue("ABIERTA");
            entity.Property(e => e.Fecha)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Numero)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.TotalBase).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.TotalImpuesto).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.TotalTotal).HasColumnType("decimal(19, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Factura__Cliente__403A8C7D");
        });

        modelBuilder.Entity<FacturaLinea>(entity =>
        {
            entity.HasKey(e => e.LineaId).HasName("PK__FacturaL__78106D316E36D71C");

            entity.ToTable("FacturaLinea");

            entity.HasIndex(e => new { e.FacturaId, e.Nlinea }, "UQ_FacturaLinea").IsUnique();

            entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.IvaPct)
                .HasDefaultValue(2100m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("IVA_Pct");
            entity.Property(e => e.Nlinea).HasColumnName("NLinea");
            entity.Property(e => e.PrecioUnit).HasColumnType("decimal(19, 2)");

            entity.HasOne(d => d.Factura).WithMany(p => p.FacturaLineas)
                .HasForeignKey(d => d.FacturaId)
                .HasConstraintName("FK__FacturaLi__Factu__48CFD27E");

            entity.HasOne(d => d.Producto).WithMany(p => p.FacturaLineas)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FacturaLi__Produ__49C3F6B7");
        });

        modelBuilder.Entity<MovimientoStock>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__Movimien__BF923C2CD2474EC1");

            entity.ToTable("MovimientoStock");

            entity.HasIndex(e => new { e.ProductoId, e.AlmacenId }, "IX_Stock_ProdAlm");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Fecha)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Origen)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Referencia)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Almacen).WithMany(p => p.MovimientoStocks)
                .HasForeignKey(d => d.AlmacenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__Almac__3A81B327");

            entity.HasOne(d => d.Producto).WithMany(p => p.MovimientoStocks)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__Produ__3B75D760");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AEA32B4FCC09");

            entity.ToTable("Producto");

            entity.HasIndex(e => e.Sku, "UQ__Producto__CA1ECF0D2B873486").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.EsInventariable).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("SKU");
        });

        modelBuilder.Entity<VStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Stock");

            entity.Property(e => e.Stock).HasColumnType("decimal(38, 4)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
