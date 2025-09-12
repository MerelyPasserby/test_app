using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test_app.Models;

public partial class AutoWorkshopContext : DbContext
{
    public AutoWorkshopContext()
    {
    }

    public AutoWorkshopContext(DbContextOptions<AutoWorkshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientCar> ClientCars { get; set; }

    public virtual DbSet<Inspection> Inspections { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartCompatibility> PartCompatibilities { get; set; }

    public virtual DbSet<PartSupplier> PartSuppliers { get; set; }

    public virtual DbSet<PartsToTask> PartsToTasks { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskType> TaskTypes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=ALEX-DESKTOP;Database=AutoWorkshopTest1;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3213E83F98FA3EF0");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(30)
                .HasColumnName("brand");
            entity.Property(e => e.Engine)
                .HasMaxLength(50)
                .HasColumnName("engine");
            entity.Property(e => e.Model)
                .HasMaxLength(30)
                .HasColumnName("model");
            entity.Property(e => e.Transmission)
                .HasMaxLength(50)
                .HasColumnName("transmission");
            entity.Property(e => e.YearFrom).HasColumnName("year_from");
            entity.Property(e => e.YearTo).HasColumnName("year_to");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3213E83FDE034261");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<ClientCar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientCa__3213E83F89507706");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.RegistrationNum)
                .HasMaxLength(10)
                .HasColumnName("registration_num");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("vin");

            entity.HasOne(d => d.Car).WithMany(p => p.ClientCars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientCar__car_i__3C69FB99");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientCars)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientCar__clien__3B75D760");
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inspecti__3213E83F4DE7DC47");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientCarId).HasColumnName("client_car_id");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.InspectionDate)
                .HasColumnType("datetime")
                .HasColumnName("inspection_date");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.Results).HasColumnName("results");

            entity.HasOne(d => d.ClientCar).WithMany(p => p.Inspections)
                .HasForeignKey(d => d.ClientCarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspectio__clien__3F466844");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Parts__3213E83FCE3BFC1F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Article)
                .HasMaxLength(50)
                .HasColumnName("article");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("price");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
        });

        modelBuilder.Entity<PartCompatibility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PartComp__3213E83FCFBFFE31");

            entity.ToTable("PartCompatibility");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.PartId).HasColumnName("part_id");

            entity.HasOne(d => d.Car).WithMany(p => p.PartCompatibilities)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartCompa__car_i__5441852A");

            entity.HasOne(d => d.Part).WithMany(p => p.PartCompatibilities)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartCompa__part___534D60F1");
        });

        modelBuilder.Entity<PartSupplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PartSupp__3213E83F2193A711");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Availability)
                .HasMaxLength(30)
                .HasColumnName("availability");
            entity.Property(e => e.DeliveryTimeDays).HasColumnName("delivery_time_days");
            entity.Property(e => e.PartId).HasColumnName("part_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

            entity.HasOne(d => d.Part).WithMany(p => p.PartSuppliers)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartSuppl__part___4F7CD00D");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PartSuppliers)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartSuppl__suppl__5070F446");
        });

        modelBuilder.Entity<PartsToTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PartsToT__3213E83FE6623AD3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartId).HasColumnName("part_id");
            entity.Property(e => e.PriceAtUse)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("price_at_use");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Part).WithMany(p => p.PartsToTasks)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartsToTa__part___4AB81AF0");

            entity.HasOne(d => d.Task).WithMany(p => p.PartsToTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PartsToTa__task___49C3F6B7");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3213E83F4189E3A7");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3213E83FA0424BCA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("datetime")
                .HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.InspectionId).HasColumnName("inspection_id");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.TaskTypeId).HasColumnName("task_type_id");
            entity.Property(e => e.TotalCost)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("total_cost");

            entity.HasOne(d => d.Inspection).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.InspectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__inspectio__440B1D61");

            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__task_type__44FF419A");
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskType__3213E83FC34BEDAD");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LaborCost).HasColumnName("labor_cost");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NormTime).HasColumnName("norm_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
