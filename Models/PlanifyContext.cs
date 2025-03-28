using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Planify_BackEnd.Models;

public partial class PlanifyContext : DbContext
{
    public PlanifyContext()
    {
    }

    public PlanifyContext(DbContextOptions<PlanifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<CategoryEvent> CategoryEvents { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventMedium> EventMedia { get; set; }

    public virtual DbSet<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; }

    public virtual DbSet<InvoiceImagesTask> InvoiceImagesTasks { get; set; }

    public virtual DbSet<JoinProject> JoinProjects { get; set; }

    public virtual DbSet<JoinTask> JoinTasks { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportMedium> ReportMedia { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SendRequest> SendRequests { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC07B0AB2338");

            entity.ToTable("Address");

            entity.Property(e => e.AddressDetail).HasMaxLength(255);

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Address__WardId__29221CFB");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campus__3214EC0773E647DE");

            entity.ToTable("Campus");

            entity.Property(e => e.CampusName).HasMaxLength(255);
        });

        modelBuilder.Entity<CategoryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07DD5F1F51");

            entity.ToTable("CategoryEvent");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CategoryEventName).HasMaxLength(255);

            entity.HasOne(d => d.Campus).WithMany(p => p.CategoryEvents)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryE__Campu__2A164134");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC07EDA8543A");

            entity.ToTable("District");

            entity.Property(e => e.DistrictName).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__District__Provin__2B0A656D");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC07F04798FE");

            entity.ToTable("Event");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.EventTitle).HasMaxLength(255);
            entity.Property(e => e.Placed).HasMaxLength(255);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TimePublic).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Campus).WithMany(p => p.Events)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__CampusId__2BFE89A6");

            entity.HasOne(d => d.CategoryEvent).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__CategoryE__2CF2ADDF");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.EventCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__CreateBy__2DE6D218");

            entity.HasOne(d => d.Manager).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Event__ManagerId__2EDAF651");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.EventUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Event__UpdateBy__2FCF1A8A");
        });

        modelBuilder.Entity<EventMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventMed__3214EC074DB46BA9");

            entity.HasOne(d => d.Event).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventMedi__Event__30C33EC3");

            entity.HasOne(d => d.Media).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventMedi__Media__31B762FC");
        });

        modelBuilder.Entity<InvoiceImagesSubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC07E4B4A7BB");

            entity.ToTable("InvoiceImagesSubTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");

            entity.HasOne(d => d.Media).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__Media__3493CFA7");

            entity.HasOne(d => d.SubTask).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.SubTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__SubTa__3587F3E0");
        });

        modelBuilder.Entity<InvoiceImagesTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC079F0F1DA1");

            entity.ToTable("InvoiceImagesTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");

            entity.HasOne(d => d.Media).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__Media__00200768");

            entity.HasOne(d => d.Task).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__TaskI__37703C52");
        });

        modelBuilder.Entity<JoinProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinProj__3214EC07FB0FD2A3");

            entity.ToTable("JoinProject");

            entity.Property(e => e.TimeJoinProject).HasColumnType("datetime");
            entity.Property(e => e.TimeOutProject)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinProje__Event__3A4CA8FD");

            entity.HasOne(d => d.User).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinProje__UserI__3C34F16F");
        });

        modelBuilder.Entity<JoinTask>(entity =>
        {
            entity.HasKey(e => e.JoinTaskId).HasName("PK__JoinTask__FE88AED4E2FFA127");

            entity.ToTable("JoinTask");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Task).WithMany(p => p.JoinTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_JoinTask_Task");

            entity.HasOne(d => d.User).WithMany(p => p.JoinTasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_JoinTask_User");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Media__3214EC07A53CB768");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .HasColumnName("MediaURL");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__3214EC07BEC5875A");

            entity.ToTable("Province");

            entity.Property(e => e.ProvinceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07270C41DE");

            entity.ToTable("Report");

            entity.Property(e => e.Reason).HasMaxLength(1000);
            entity.Property(e => e.SendTime).HasColumnType("datetime");

            entity.HasOne(d => d.SendFromNavigation).WithMany(p => p.ReportSendFromNavigations)
                .HasForeignKey(d => d.SendFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__SendFrom__3D2915A8");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.ReportSendToNavigations)
                .HasForeignKey(d => d.SendTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__SendTo__3E1D39E1");

            entity.HasOne(d => d.Task).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__TaskId__3F115E1A");
        });

        modelBuilder.Entity<ReportMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportMe__3214EC077E460D6D");

            entity.HasOne(d => d.Media).WithMany(p => p.ReportMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportMed__Media__40058253");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportMedia)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportMed__Repor__40F9A68C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070FA2515F");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<SendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SendRequ__3214EC07A2AE8E62");

            entity.ToTable("SendRequest");

            entity.Property(e => e.Reason).HasMaxLength(1000);

            entity.HasOne(d => d.Event).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SendReque__Event__41EDCAC5");

            entity.HasOne(d => d.Manager).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__SendReque__Manag__42E1EEFE");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubTask__3214EC07AC10CF57");

            entity.ToTable("SubTask");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubTaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubTask__CreateB__43D61337");

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubTask__TaskId__44CA3770");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07BBDCA440");

            entity.ToTable("Task");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__CreateBy__45BE5BA9");

            entity.HasOne(d => d.Event).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Task_Event");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07FDD068E8");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.Users)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__User__AddressId__47A6A41B");

            entity.HasOne(d => d.Avatar).WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarId)
                .HasConstraintName("FK__User__AvatarId__489AC854");

            entity.HasOne(d => d.Campus).WithMany(p => p.Users)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__CampusId__498EEC8D");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC0766E9DDD1");

            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__RoleId__4A8310C6");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__UserId__4B7734FF");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__3214EC07504C72E0");

            entity.ToTable("Ward");

            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.WardName).HasMaxLength(255);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ward__DistrictID__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
