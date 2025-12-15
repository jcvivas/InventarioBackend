using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<ProductoProveedorLote> ProductoProveedorLotes => Set<ProductoProveedorLote>();
        public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
        public DbSet<Deseado> Deseados => Set<Deseado>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<PedidoDetalle> PedidoDetalles => Set<PedidoDetalle>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapearUsuarios(modelBuilder);
            MapearCategorias(modelBuilder);
            MapearProductos(modelBuilder);
            MapearProveedores(modelBuilder);
            MapearProductoProveedorLote(modelBuilder);
            MapearMovimientosInventario(modelBuilder);
            MapearDeseados(modelBuilder);
            MapearPedidos(modelBuilder);
            MapearPedidoDetalles(modelBuilder);
            modelBuilder.Entity<SpProcesarCompraResultado>().HasNoKey();

            modelBuilder.Entity<ResultadoOperacionSp>(e =>
            {
                e.HasNoKey();
            });
        }

        private static void MapearUsuarios(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Usuario>();
            e.ToTable("usuarios", "seguridad");
            e.HasKey(x => x.IdUsuario);

            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
            e.Property(x => x.Correo).HasColumnName("correo").HasMaxLength(120).IsRequired();
            e.Property(x => x.HashContrasena).HasColumnName("hash_contrasena").HasMaxLength(255).IsRequired();
            e.Property(x => x.Rol).HasColumnName("rol").HasMaxLength(20).IsRequired();
            e.Property(x => x.Activo).HasColumnName("activo").IsRequired();

            e.Property(x => x.UsuarioCreacion).HasColumnName("usuario_creacion").HasMaxLength(60);
            e.Property(x => x.FechaCreacionUtc).HasColumnName("fecha_creacion_utc").IsRequired();
            e.Property(x => x.UsuarioModificacion).HasColumnName("usuario_modificacion").HasMaxLength(60);
            e.Property(x => x.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");

            e.Property(x => x.VersionFila)
                .HasColumnName("version_fila")
                .IsRowVersion()
                .IsConcurrencyToken();

            e.HasIndex(x => x.Correo).IsUnique().HasDatabaseName("UX_usuarios_correo");
        }

        private static void MapearCategorias(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Categoria>();
            e.ToTable("categorias", "catalogo");
            e.HasKey(x => x.IdCategoria);

            e.Property(x => x.IdCategoria).HasColumnName("id_categoria");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            e.Property(x => x.Activo).HasColumnName("activo").IsRequired();

            e.HasIndex(x => x.Nombre).IsUnique().HasDatabaseName("UX_categorias_nombre");
        }

        private static void MapearProductos(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Producto>();
            e.ToTable("productos", "inventario");
            e.HasKey(x => x.IdProducto);

            e.Property(x => x.IdProducto).HasColumnName("id_producto");
            e.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(50).IsRequired();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(600);
            e.Property(x => x.Marca).HasColumnName("marca").HasMaxLength(120);
            e.Property(x => x.IdCategoria).HasColumnName("id_categoria");
            e.Property(x => x.UrlImagen).HasColumnName("url_imagen").HasMaxLength(350);

            e.Property(x => x.Activo).HasColumnName("activo").IsRequired();

            e.Property(x => x.UsuarioCreacion).HasColumnName("usuario_creacion").HasMaxLength(60);
            e.Property(x => x.FechaCreacionUtc).HasColumnName("fecha_creacion_utc").IsRequired();
            e.Property(x => x.UsuarioModificacion).HasColumnName("usuario_modificacion").HasMaxLength(60);
            e.Property(x => x.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");

            e.Property(x => x.VersionFila)
                .HasColumnName("version_fila")
                .IsRowVersion()
                .IsConcurrencyToken();

            e.HasIndex(x => x.Codigo).IsUnique().HasDatabaseName("UX_productos_codigo");
            e.HasIndex(x => x.Nombre).HasDatabaseName("IX_productos_nombre");
            e.HasIndex(x => x.IdCategoria).HasDatabaseName("IX_productos_categoria");

            e.HasOne(x => x.Categoria)
                .WithMany(x => x.Productos)
                .HasForeignKey(x => x.IdCategoria)
                .HasConstraintName("FK_productos_categoria")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void MapearProveedores(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Proveedor>();
            e.ToTable("proveedores", "inventario");
            e.HasKey(x => x.IdProveedor);

            e.Property(x => x.IdProveedor).HasColumnName("id_proveedor");
            e.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(50).IsRequired();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.Identificacion).HasColumnName("identificacion").HasMaxLength(30);
            e.Property(x => x.Correo).HasColumnName("correo").HasMaxLength(120);
            e.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(30);

            e.Property(x => x.Activo).HasColumnName("activo").IsRequired();

            e.Property(x => x.UsuarioCreacion).HasColumnName("usuario_creacion").HasMaxLength(60);
            e.Property(x => x.FechaCreacionUtc).HasColumnName("fecha_creacion_utc").IsRequired();
            e.Property(x => x.UsuarioModificacion).HasColumnName("usuario_modificacion").HasMaxLength(60);
            e.Property(x => x.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");

            e.Property(x => x.VersionFila)
                .HasColumnName("version_fila")
                .IsRowVersion()
                .IsConcurrencyToken();

            e.HasIndex(x => x.Codigo).IsUnique().HasDatabaseName("UX_proveedores_codigo");
            e.HasIndex(x => x.Nombre).HasDatabaseName("IX_proveedores_nombre");
        }

        private static void MapearProductoProveedorLote(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<ProductoProveedorLote>();
            e.ToTable("producto_proveedor_lote", "inventario");
            e.HasKey(x => x.IdProductoProveedorLote);

            e.Property(x => x.IdProductoProveedorLote).HasColumnName("id_producto_proveedor_lote");
            e.Property(x => x.IdProducto).HasColumnName("id_producto").IsRequired();
            e.Property(x => x.IdProveedor).HasColumnName("id_proveedor").IsRequired();

            e.Property(x => x.NumeroLote).HasColumnName("numero_lote").HasMaxLength(60);

            e.Property(x => x.NumeroLoteNormalizado)
                .HasColumnName("numero_lote_normalizado")
                .HasMaxLength(60)
                .ValueGeneratedOnAddOrUpdate();

            e.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(18, 2).IsRequired();
            e.Property(x => x.StockDisponible).HasColumnName("stock_disponible").IsRequired();
            e.Property(x => x.StockReservado).HasColumnName("stock_reservado").IsRequired();
            e.Property(x => x.Moneda).HasColumnName("moneda").HasMaxLength(10).IsRequired();
            e.Property(x => x.FechaVencimiento).HasColumnName("fecha_vencimiento");

            e.Property(x => x.Activo).HasColumnName("activo").IsRequired();

            e.Property(x => x.UsuarioCreacion).HasColumnName("usuario_creacion").HasMaxLength(60);
            e.Property(x => x.FechaCreacionUtc).HasColumnName("fecha_creacion_utc").IsRequired();
            e.Property(x => x.UsuarioModificacion).HasColumnName("usuario_modificacion").HasMaxLength(60);
            e.Property(x => x.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");

            e.Property(x => x.VersionFila)
                .HasColumnName("version_fila")
                .IsRowVersion()
                .IsConcurrencyToken();

            e.HasIndex(x => x.IdProducto).HasDatabaseName("IX_ppl_producto");
            e.HasIndex(x => x.IdProveedor).HasDatabaseName("IX_ppl_proveedor");

            e.HasOne(x => x.Producto)
                .WithMany(x => x.Ofertas)
                .HasForeignKey(x => x.IdProducto)
                .HasConstraintName("FK_ppl_producto")
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Proveedor)
                .WithMany(x => x.Ofertas)
                .HasForeignKey(x => x.IdProveedor)
                .HasConstraintName("FK_ppl_proveedor")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void MapearMovimientosInventario(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<MovimientoInventario>();
            e.ToTable("movimientos_inventario", "inventario");
            e.HasKey(x => x.IdMovimiento);

            e.Property(x => x.IdMovimiento).HasColumnName("id_movimiento");
            e.Property(x => x.IdProductoProveedorLote).HasColumnName("id_producto_proveedor_lote").IsRequired();
            e.Property(x => x.TipoMovimiento).HasColumnName("tipo_movimiento").HasMaxLength(20).IsRequired();
            e.Property(x => x.Cantidad).HasColumnName("cantidad").IsRequired();
            e.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(200);
            e.Property(x => x.Referencia).HasColumnName("referencia").HasMaxLength(120);
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
            e.Property(x => x.FechaMovimientoUtc).HasColumnName("fecha_movimiento_utc").IsRequired();

            e.HasIndex(x => new { x.IdProductoProveedorLote, x.FechaMovimientoUtc })
                .HasDatabaseName("IX_mov_ppl_fecha");

            e.HasOne(x => x.ProductoProveedorLote)
                .WithMany(x => x.Movimientos)
                .HasForeignKey(x => x.IdProductoProveedorLote)
                .HasConstraintName("FK_mov_ppl")
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Movimientos)
                .HasForeignKey(x => x.IdUsuario)
                .HasConstraintName("FK_mov_usuario")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void MapearDeseados(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Deseado>();
            e.ToTable("deseados", "ventas");
            e.HasKey(x => new { x.IdUsuario, x.IdProducto });

            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
            e.Property(x => x.IdProducto).HasColumnName("id_producto");
            e.Property(x => x.FechaAgregadoUtc).HasColumnName("fecha_agregado_utc").IsRequired();

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Deseados)
                .HasForeignKey(x => x.IdUsuario)
                .HasConstraintName("FK_deseados_usuario")
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.Producto)
                .WithMany(x => x.Deseados)
                .HasForeignKey(x => x.IdProducto)
                .HasConstraintName("FK_deseados_producto")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void MapearPedidos(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Pedido>();
            e.ToTable("pedidos", "ventas");
            e.HasKey(x => x.IdPedido);

            e.Property(x => x.IdPedido).HasColumnName("id_pedido");
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario").IsRequired();
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();
            e.Property(x => x.Total).HasColumnName("total").HasPrecision(18, 2).IsRequired();
            e.Property(x => x.Moneda).HasColumnName("moneda").HasMaxLength(10).IsRequired();
            e.Property(x => x.FechaCreacionUtc).HasColumnName("fecha_creacion_utc").IsRequired();

            e.HasIndex(x => new { x.IdUsuario, x.FechaCreacionUtc }).HasDatabaseName("IX_ped_usuario_fecha");

            e.HasOne(x => x.Usuario)
                .WithMany(x => x.Pedidos)
                .HasForeignKey(x => x.IdUsuario)
                .HasConstraintName("FK_ped_usuario")
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void MapearPedidoDetalles(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<PedidoDetalle>();
            e.ToTable("pedido_detalles", "ventas");
            e.HasKey(x => x.IdPedidoDetalle);

            e.Property(x => x.IdPedidoDetalle).HasColumnName("id_pedido_detalle");
            e.Property(x => x.IdPedido).HasColumnName("id_pedido").IsRequired();
            e.Property(x => x.IdProductoProveedorLote).HasColumnName("id_producto_proveedor_lote").IsRequired();
            e.Property(x => x.Cantidad).HasColumnName("cantidad").IsRequired();
            e.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(18, 2).IsRequired();

            e.Property(x => x.Subtotal)
                .HasColumnName("subtotal")
                .HasPrecision(18, 2)
                .ValueGeneratedOnAddOrUpdate();

            e.HasIndex(x => x.IdPedido).HasDatabaseName("IX_peddet_pedido");
            e.HasIndex(x => x.IdProductoProveedorLote).HasDatabaseName("IX_peddet_ppl");

            e.HasOne(x => x.Pedido)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdPedido)
                .HasConstraintName("FK_peddet_pedido")
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(x => x.ProductoProveedorLote)
                .WithMany(x => x.DetallesPedido)
                .HasForeignKey(x => x.IdProductoProveedorLote)
                .HasConstraintName("FK_peddet_ppl")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
