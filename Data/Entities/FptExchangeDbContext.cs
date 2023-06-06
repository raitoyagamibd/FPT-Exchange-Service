using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

public partial class FptExchangeDbContext : DbContext
{
    public FptExchangeDbContext()
    {
    }

    public FptExchangeDbContext(DbContextOptions<FptExchangeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ImageProduct> ImageProducts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductActivy> ProductActivies { get; set; }

    public virtual DbSet<ProductTransfer> ProductTransfers { get; set; }

    public virtual DbSet<ProductTransferItem> ProductTransferItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07B53EB457");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ImageProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ImagePro__3214EC07F8C6A438");

            entity.ToTable("ImageProduct");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Product).WithMany(p => p.ImageProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImageProd__Produ__4E88ABD4");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07158B21DB");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SendTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__SendT__52593CB8");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07B66EBCB2");

            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AddById).HasColumnName("AddByID");
            entity.Property(e => e.BuyerId).HasColumnName("BuyerID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SellerId).HasColumnName("SellerID");
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");

            entity.HasOne(d => d.AddBy).WithMany(p => p.ProductAddBies)
                .HasForeignKey(d => d.AddById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__AddByID__36B12243");

            entity.HasOne(d => d.Buyer).WithMany(p => p.ProductBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__BuyerID__38996AB5");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__33D4B598");

            entity.HasOne(d => d.Seller).WithMany(p => p.ProductSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__SellerI__37A5467C");

            entity.HasOne(d => d.Station).WithMany(p => p.Products)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Station__35BCFE0A");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Status___34C8D9D1");
        });

        modelBuilder.Entity<ProductActivy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductA__3214EC0787DC9A90");

            entity.ToTable("ProductActivy");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StationsId).HasColumnName("Stations_ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.NewStatusNavigation).WithMany(p => p.ProductActivyNewStatusNavigations)
                .HasForeignKey(d => d.NewStatus)
                .HasConstraintName("FK__ProductAc__NewSt__403A8C7D");

            entity.HasOne(d => d.OldStatusNavigation).WithMany(p => p.ProductActivyOldStatusNavigations)
                .HasForeignKey(d => d.OldStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__OldSt__3F466844");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__Produ__3D5E1FD2");

            entity.HasOne(d => d.Stations).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.StationsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__Stati__3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__UserI__3C69FB99");
        });

        modelBuilder.Entity<ProductTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC07B06088EB");

            entity.ToTable("ProductTransfer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StationIdform).HasColumnName("StationIDForm");
            entity.Property(e => e.StationIdto).HasColumnName("StationIDTo");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.StationIdformNavigation).WithMany(p => p.ProductTransferStationIdformNavigations)
                .HasForeignKey(d => d.StationIdform)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Stati__440B1D61");

            entity.HasOne(d => d.StationIdtoNavigation).WithMany(p => p.ProductTransferStationIdtoNavigations)
                .HasForeignKey(d => d.StationIdto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Stati__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.ProductTransfers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__UserI__45F365D3");
        });

        modelBuilder.Entity<ProductTransferItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC07DD51E2A2");

            entity.ToTable("ProductTransfer_Item");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductTransferId).HasColumnName("ProductTransferID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTransferItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Produ__5535A963");

            entity.HasOne(d => d.ProductTransfer).WithMany(p => p.ProductTransferItems)
                .HasForeignKey(d => d.ProductTransferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Produ__5629CD9C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07E98797FF");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stations__3214EC077CC75EC0");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07182EB59A");

            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07641438E8");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Produ__49C3F6B7");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Walle__4AB81AF0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC077C028C9E");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccessToken).HasMaxLength(256);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.RefreshToken).HasMaxLength(256);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__2E1BDC42");

            entity.HasOne(d => d.Station).WithMany(p => p.Users)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__User__StationID__2F10007B");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Users)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__User__WalletID__300424B4");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC0712DDDE2E");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
