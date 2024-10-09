using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class StorageContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Host=localhost;Username=postgres;Password=example;Database=Test").UseLazyLoadingProxies().LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.HasKey(pg => pg.Id).HasName("product_group_pkey");

                //entity.ToTable("productgroups");
                entity.ToTable("category");

                //entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(pg => pg.Name).HasColumnName("name").HasMaxLength(255);

                //entity.Property(pg => pg.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("product_pk");

                //entity.ToTable("products");
                //entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(p => p.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                //entity.Property(p => p.Description)
                //    .HasMaxLength(1024)
                //    .HasColumnName("description");

                entity.HasOne(p => p.ProductGroup).WithMany(p => p.Products).HasForeignKey(p => p.ProductGroupId);
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(s => s.Id).HasName("storage_pk");
                entity.HasOne(s => s.Product).WithMany(s => s.Storages).HasForeignKey(p=> p.ProductId);


                //entity.ToTable("storage");
                //entity.Property(e => e.Id).HasColumnName("id");
                //entity.Property(e => e.Count).HasColumnName("count");


            });


           // OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }


}
    
