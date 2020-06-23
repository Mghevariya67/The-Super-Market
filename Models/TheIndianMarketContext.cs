using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TheIndianSuperMarket.Models
{
    public partial class TheIndianMarketContext : DbContext
    {
        public TheIndianMarketContext()
        {
        }

        public TheIndianMarketContext(DbContextOptions<TheIndianMarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<AisleContains> AisleContains { get; set; }
        public virtual DbSet<Aisles> Aisles { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProvidedBy> ProvidedBy { get; set; }
        public virtual DbSet<ProvidesDelivery> ProvidesDelivery { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<ReceivedFrom> ReceivedFrom { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-D92K87\\SQLEXPRESS;Database=TheIndianMarket;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.AdminId)
                    .HasColumnName("AdminID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Contact)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AisleContains>(entity =>
            {
                entity.HasKey(e => new { e.AisleNumber, e.ProductId })
                    .HasName("PK__AisleCon__B069802044092984");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.AisleNumberNavigation)
                    .WithMany(p => p.AisleContains)
                    .HasForeignKey(d => d.AisleNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AisleCont__Aisle__32E0915F");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AisleContains)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AisleCont__Produ__31EC6D26");
            });

            modelBuilder.Entity<Aisles>(entity =>
            {
                entity.HasKey(e => e.AisleNumber)
                    .HasName("PK__Aisles__7B294C4E0D5FF2F3");

                entity.Property(e => e.AisleNumber).ValueGeneratedNever();

                entity.Property(e => e.AisleName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProductId).HasColumnName("productId");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Customer__C3905BCF73A07CF6");

                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__Customer__A4AE64B80E386F83");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerFirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerLastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DepartmentName)
                    .HasName("PK__Departme__D949CC352975109C");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04FF1F75E3BE4");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeePhone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Sinumber).HasColumnName("sinumber");

                entity.HasOne(d => d.DepartmentNameNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentName)
                    .HasConstraintName("FK__Employees__Depar__29572725");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId)
                    .HasColumnName("PaymentID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasColumnName("CVV")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Expiration)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameOnCard)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__CartID__0C85DE4D");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__Custome__0B91BA14");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Products__B40CC6ED9F6EDB2B");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Animal)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BestBeforeDate)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Brand)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CostUnit)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cut)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Plu).HasColumnName("PLU");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionDate)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Upc).HasColumnName("UPC");

                entity.HasOne(d => d.DepartmentNameNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.DepartmentName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__Depart__2C3393D0");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Products__Suppli__3A4CA8FD");
            });

            modelBuilder.Entity<ProvidedBy>(entity =>
            {
                entity.HasKey(e => new { e.SupplierId, e.ProductId })
                    .HasName("PK__Provided__80A6AAFA6FCE274D");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProvidedBy)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvidedB__Produ__36B12243");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ProvidedBy)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvidedB__Suppl__35BCFE0A");
            });

            modelBuilder.Entity<ProvidesDelivery>(entity =>
            {
                entity.HasKey(e => e.DeliveryId)
                    .HasName("PK__Provides__626D8FEE392377AD");

                entity.Property(e => e.DeliveryId)
                    .HasColumnName("DeliveryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ProvidesDelivery)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvidesD__Suppl__398D8EEE");
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.HasKey(e => new { e.TransactionId, e.ProductId })
                    .HasName("PK__Purchase__9E03F625F023D1CE");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PurchaseDate)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchases__Produ__403A8C7D");
            });

            modelBuilder.Entity<ReceivedFrom>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.DeliveryId })
                    .HasName("PK__Received__422A1E13B5520CE0");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.DeliveryId).HasColumnName("DeliveryID");

                entity.HasOne(d => d.Delivery)
                    .WithMany(p => p.ReceivedFrom)
                    .HasForeignKey(d => d.DeliveryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceivedF__Deliv__3D5E1FD2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReceivedFrom)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceivedF__Produ__3C69FB99");
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId)
                    .HasName("PK__Supplier__4BE6669413A6709B");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("SupplierID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Contact)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierPhone)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
