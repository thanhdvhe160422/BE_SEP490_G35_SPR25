using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Planify_BackEnd.Models;

public partial class PlanifyDbContext : DbContext
{
    public PlanifyDbContext()
    {
    }

    public PlanifyDbContext(DbContextOptions<PlanifyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<CategoryEvent> CategoryEvents { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventMedium> EventMedia { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; }

    public virtual DbSet<InvoiceImagesTask> InvoiceImagesTasks { get; set; }

    public virtual DbSet<JoinProject> JoinProjects { get; set; }

    public virtual DbSet<MediaItem> MediaItems { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SendRequest> SendRequests { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC078C6BBC5B");

            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.AvatarNavigation).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Avatar)
                .HasConstraintName("FK__Admin__Avatar__48CFD27E");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campus__3214EC07699F6314");

            entity.ToTable("Campus");

            entity.Property(e => e.CampusName).HasMaxLength(255);
        });

        modelBuilder.Entity<CategoryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07B017394E");

            entity.ToTable("CategoryEvent");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CategoryEventName).HasMaxLength(255);

            entity.HasOne(d => d.Campus).WithMany(p => p.CategoryEvents)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK__CategoryE__Campu__3B75D760");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC07F051D900");

            entity.ToTable("District");

            entity.Property(e => e.DistrictName).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__District__Provin__403A8C7D");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC07C3C53BBD");

            entity.ToTable("Event");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.EndOfEvent).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.EventTitle).HasMaxLength(255);
            entity.Property(e => e.Placed).HasMaxLength(255);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TimeOfEvent).HasColumnType("datetime");
            entity.Property(e => e.TimePublic).HasColumnType("datetime");

            entity.HasOne(d => d.Campus).WithMany(p => p.Events)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK__Event__CampusId__59FA5E80");

            entity.HasOne(d => d.CategoryEvent).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryEventId)
                .HasConstraintName("FK__Event__CategoryE__5AEE82B9");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.EventCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Event__CreateBy__571DF1D5");

            entity.HasOne(d => d.Manager).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Event__ManagerId__59063A47");
        });

        modelBuilder.Entity<EventMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventMed__3214EC07C361BBF6");

            entity.HasOne(d => d.Event).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventMedi__Event__619B8048");

            entity.HasOne(d => d.Media).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("FK__EventMedi__Media__628FA481");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC073340868D");

            entity.ToTable("Group");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.GroupName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Group__CreateBy__6B24EA82");

            entity.HasOne(d => d.Event).WithMany(p => p.Groups)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Group__EventId__6C190EBB");
        });

        modelBuilder.Entity<InvoiceImagesSubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC0707FDF766");

            entity.ToTable("InvoiceImagesSubTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");
            entity.Property(e => e.InvoiceImageUrl)
                .HasMaxLength(500)
                .HasColumnName("InvoiceImageURL");

            entity.HasOne(d => d.SubTask).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.SubTaskId)
                .HasConstraintName("FK__InvoiceIm__SubTa__778AC167");
        });

        modelBuilder.Entity<InvoiceImagesTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC074D20B675");

            entity.ToTable("InvoiceImagesTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");
            entity.Property(e => e.InvoiceImageUrl)
                .HasMaxLength(500)
                .HasColumnName("InvoiceImageURL");

            entity.HasOne(d => d.Task).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__InvoiceIm__TaskI__7A672E12");
        });

        modelBuilder.Entity<JoinProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinProj__3214EC0730D721C5");

            entity.ToTable("JoinProject");

            entity.Property(e => e.TimeJoinProject).HasColumnType("datetime");
            entity.Property(e => e.TimeOutProject)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__JoinProje__Event__656C112C");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK__JoinProjec__Role__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__JoinProje__UserI__66603565");
        });

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MediaIte__3214EC07B5BC8118");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .HasColumnName("MediaURL");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__3214EC077E6D8019");

            entity.ToTable("Province");

            entity.Property(e => e.ProvinceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0791A7DB13");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<SendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SendRequ__3214EC07A267419B");

            entity.ToTable("SendRequest");

            entity.Property(e => e.Reason).HasMaxLength(1000);

            entity.HasOne(d => d.Event).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__SendReque__Event__5DCAEF64");

            entity.HasOne(d => d.Manager).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__SendReque__Manag__5EBF139D");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubTask__3214EC07F2A302FC");

            entity.ToTable("SubTask");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubTaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__SubTask__CreateB__74AE54BC");

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__SubTask__TaskId__73BA3083");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07522E353D");

            entity.ToTable("Task");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Task__CreateBy__6EF57B66");

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Task__GroupId__70DDC3D8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC079CA1F7F4");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.AvatarNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Avatar)
                .HasConstraintName("FK__Users__Avatar__4F7CD00D");

            entity.HasOne(d => d.Campus).WithMany(p => p.Users)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK__Users__CampusId__5441852A");

            entity.HasOne(d => d.CreateByAdminNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.CreateByAdmin)
                .HasConstraintName("FK__Users__CreateByA__52593CB8");

            entity.HasOne(d => d.CreateByManageNavigation).WithMany(p => p.InverseCreateByManageNavigation)
                .HasForeignKey(d => d.CreateByManage)
                .HasConstraintName("FK__Users__CreateByM__534D60F1");

            entity.HasOne(d => d.District).WithMany(p => p.Users)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Users__DistrictI__4D94879B");

            entity.HasOne(d => d.Province).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Users__ProvinceI__4E88ABD4");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK__Users__Role__5165187F");

            entity.HasOne(d => d.Ward).WithMany(p => p.Users)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK__Users__WardId__4CA06362");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__3214EC07CBA258CF");

            entity.ToTable("Ward");

            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.WardName).HasMaxLength(255);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Ward__DistrictID__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
