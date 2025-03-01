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

    public virtual DbSet<AssignTask> AssignTasks { get; set; }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<CategoryEvent> CategoryEvents { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventMedium> EventMedia { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; }

    public virtual DbSet<InvoiceImagesTask> InvoiceImagesTasks { get; set; }

    public virtual DbSet<JoinGroup> JoinGroups { get; set; }

    public virtual DbSet<JoinProject> JoinProjects { get; set; }

    public virtual DbSet<JoinTask> JoinTasks { get; set; }

    public virtual DbSet<MediaItem> MediaItems { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportMedium> ReportMedia { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SendRequest> SendRequests { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssignTa__3214EC07D4FEF93A");

            entity.ToTable("AssignTask");

            entity.Property(e => e.TimeJoin).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Assign).WithMany(p => p.AssignTasks)
                .HasForeignKey(d => d.AssignId)
                .HasConstraintName("FK__AssignTas__Assig__7C4F7684");

            entity.HasOne(d => d.SubTask).WithMany(p => p.AssignTasks)
                .HasForeignKey(d => d.SubTaskId)
                .HasConstraintName("FK__AssignTas__SubTa__7D439ABD");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campus__3214EC07A0547913");

            entity.ToTable("Campus");

            entity.Property(e => e.CampusName).HasMaxLength(255);
        });

        modelBuilder.Entity<CategoryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07313FA59A");

            entity.ToTable("CategoryEvent");

            entity.Property(e => e.CategoryEventName).HasMaxLength(255);

            entity.HasOne(d => d.Campus).WithMany(p => p.CategoryEvents)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK__CategoryE__Campu__44FF419A");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC074E4FB66C");

            entity.ToTable("District");

            entity.Property(e => e.DistrictName).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__District__Provin__3F466844");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC07879E40D1");

            entity.ToTable("Event");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreatedAt)
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
                .HasConstraintName("FK__Event__CampusId__5165187F");

            entity.HasOne(d => d.CategoryEvent).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryEventId)
                .HasConstraintName("FK__Event__CategoryE__52593CB8");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.EventCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Event__CreateBy__4F7CD00D");

            entity.HasOne(d => d.Manager).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Event__ManagerId__5070F446");
        });

        modelBuilder.Entity<EventMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventMed__3214EC078B36DE97");

            entity.HasOne(d => d.Event).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventMedi__Event__5535A963");

            entity.HasOne(d => d.Media).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("FK__EventMedi__Media__5629CD9C");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC079D0EF3F3");

            entity.ToTable("Group");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.GroupName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Group__CreateBy__619B8048");

            entity.HasOne(d => d.Event).WithMany(p => p.Groups)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Group__EventId__628FA481");
        });

        modelBuilder.Entity<InvoiceImagesSubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC079F7723FF");

            entity.ToTable("InvoiceImagesSubTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");
            entity.Property(e => e.InvoiceImageUrl)
                .HasMaxLength(500)
                .HasColumnName("InvoiceImageURL");

            entity.HasOne(d => d.SubTask).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.SubTaskId)
                .HasConstraintName("FK__InvoiceIm__SubTa__00200768");
        });

        modelBuilder.Entity<InvoiceImagesTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC07B5054C75");

            entity.ToTable("InvoiceImagesTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");
            entity.Property(e => e.InvoiceImageUrl)
                .HasMaxLength(500)
                .HasColumnName("InvoiceImageURL");

            entity.HasOne(d => d.Task).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__InvoiceIm__TaskI__02FC7413");
        });

        modelBuilder.Entity<JoinGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinGrou__3214EC07C7D52D4A");

            entity.ToTable("JoinGroup");

            entity.Property(e => e.TimeJoin).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.JoinGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__JoinGroup__Group__75A278F5");

            entity.HasOne(d => d.Implementer).WithMany(p => p.JoinGroups)
                .HasForeignKey(d => d.ImplementerId)
                .HasConstraintName("FK__JoinGroup__Imple__74AE54BC");
        });

        modelBuilder.Entity<JoinProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinProj__3214EC07659A18CC");

            entity.ToTable("JoinProject");

            entity.Property(e => e.TimeJoinProject).HasColumnType("datetime");
            entity.Property(e => e.TimeOutProject)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__JoinProje__Event__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__JoinProje__UserI__5EBF139D");
        });

        modelBuilder.Entity<JoinTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinTask__3214EC07C883397F");

            entity.ToTable("JoinTask");

            entity.Property(e => e.TimeJoin).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Implementer).WithMany(p => p.JoinTasks)
                .HasForeignKey(d => d.ImplementerId)
                .HasConstraintName("FK__JoinTask__Implem__797309D9");

            entity.HasOne(d => d.Task).WithMany(p => p.JoinTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__JoinTask__TaskId__787EE5A0");
        });

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MediaIte__3214EC074378424F");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .HasColumnName("MediaURL");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__3214EC0742D4AAED");

            entity.ToTable("Province");

            entity.Property(e => e.ProvinceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07305FD60E");

            entity.ToTable("Report");

            entity.Property(e => e.Reason).HasMaxLength(1000);
            entity.Property(e => e.SendTime).HasColumnType("datetime");

            entity.HasOne(d => d.ReportUser).WithMany(p => p.ReportReportUsers)
                .HasForeignKey(d => d.ReportUserId)
                .HasConstraintName("FK__Report__ReportUs__6EF57B66");

            entity.HasOne(d => d.SendFromNavigation).WithMany(p => p.ReportSendFromNavigations)
                .HasForeignKey(d => d.SendFrom)
                .HasConstraintName("FK__Report__SendFrom__6D0D32F4");

            entity.HasOne(d => d.Task).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Report__TaskId__6E01572D");
        });

        modelBuilder.Entity<ReportMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportMe__3214EC07636296D0");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .HasColumnName("MediaURL");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportMedia)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK__ReportMed__Repor__71D1E811");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07BAF9B1A7");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<SendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SendRequ__3214EC075EEE23D7");

            entity.ToTable("SendRequest");

            entity.Property(e => e.Reason).HasMaxLength(1000);

            entity.HasOne(d => d.Event).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__SendReque__Event__59063A47");

            entity.HasOne(d => d.Manager).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__SendReque__Manag__59FA5E80");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubTask__3214EC0768B35C66");

            entity.ToTable("SubTask");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubTaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__SubTask__CreateB__6A30C649");

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__SubTask__TaskId__693CA210");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC0750EFAB84");

            entity.ToTable("Task");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Task__CreateBy__656C112C");

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Task__GroupId__66603565");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07FEA36B21");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
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
                .HasConstraintName("FK__Users__Avatar__4AB81AF0");

            entity.HasOne(d => d.Campus).WithMany(p => p.Users)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK__Users__CampusId__4CA06362");

            entity.HasOne(d => d.District).WithMany(p => p.Users)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Users__DistrictI__48CFD27E");

            entity.HasOne(d => d.Province).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Users__ProvinceI__49C3F6B7");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK__Users__Role__4BAC3F29");

            entity.HasOne(d => d.Ward).WithMany(p => p.Users)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK__Users__WardId__47DBAE45");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__3214EC07B5ACCBA1");

            entity.ToTable("Ward");

            entity.Property(e => e.WardName).HasMaxLength(255);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Ward__DistrictId__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
