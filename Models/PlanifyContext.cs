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

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; }

    public virtual DbSet<InvoiceImagesTask> InvoiceImagesTasks { get; set; }

    public virtual DbSet<JoinGroup> JoinGroups { get; set; }

    public virtual DbSet<JoinProject> JoinProjects { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC074BE65F67");

            entity.ToTable("Address");

            entity.Property(e => e.AddressDetail).HasMaxLength(255);

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Address__WardId__619B8048");
        });

        modelBuilder.Entity<Campus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campus__3214EC07FE14ED64");

            entity.ToTable("Campus");

            entity.Property(e => e.CampusName).HasMaxLength(255);
        });

        modelBuilder.Entity<CategoryEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07C2C4BC9B");

            entity.ToTable("CategoryEvent");

            entity.Property(e => e.CampusId).HasColumnName("CampusID");
            entity.Property(e => e.CategoryEventName).HasMaxLength(255);

            entity.HasOne(d => d.Campus).WithMany(p => p.CategoryEvents)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryE__Campu__6383C8BA");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__3214EC076D4EBBB7");

            entity.ToTable("District");

            entity.Property(e => e.DistrictName).HasMaxLength(255);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__District__Provin__656C112C");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC0778B2F0DC");

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
                .HasConstraintName("FK__Event__CampusId__6754599E");

            entity.HasOne(d => d.CategoryEvent).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__CategoryE__693CA210");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.EventCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__CreateBy__6B24EA82");

            entity.HasOne(d => d.Manager).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Event__ManagerId__6D0D32F4");

            entity.HasOne(d => d.UpdateByNavigation).WithMany(p => p.EventUpdateByNavigations)
                .HasForeignKey(d => d.UpdateBy)
                .HasConstraintName("FK__Event__UpdateBy__6EF57B66");
        });

        modelBuilder.Entity<EventMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventMed__3214EC0773D4708A");

            entity.HasOne(d => d.Event).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventMedi__Event__70DDC3D8");

            entity.HasOne(d => d.Media).WithMany(p => p.EventMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventMedi__Media__72C60C4A");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC0705CC4DBD");

            entity.ToTable("Group");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.GroupName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Group__CreateBy__74AE54BC");

            entity.HasOne(d => d.Event).WithMany(p => p.Groups)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Group__EventId__76969D2E");
        });

        modelBuilder.Entity<InvoiceImagesSubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC07913DC55B");

            entity.ToTable("InvoiceImagesSubTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");

            entity.HasOne(d => d.Media).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__Media__787EE5A0");

            entity.HasOne(d => d.SubTask).WithMany(p => p.InvoiceImagesSubTasks)
                .HasForeignKey(d => d.SubTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__SubTa__7A672E12");
        });

        modelBuilder.Entity<InvoiceImagesTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC077620075E");

            entity.ToTable("InvoiceImagesTask");

            entity.Property(e => e.ActualBudgetAmount).HasColumnType("money");

            entity.HasOne(d => d.Media).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__Media__7C4F7684");

            entity.HasOne(d => d.Task).WithMany(p => p.InvoiceImagesTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIm__TaskI__7E37BEF6");
        });

        modelBuilder.Entity<JoinGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinGrou__3214EC07C5D9612B");

            entity.ToTable("JoinGroup");

            entity.Property(e => e.TimeJoin).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.JoinGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinGroup__Group__00200768");

            entity.HasOne(d => d.Implementer).WithMany(p => p.JoinGroups)
                .HasForeignKey(d => d.ImplementerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinGroup__Imple__02084FDA");
        });

        modelBuilder.Entity<JoinProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JoinProj__3214EC07797AC8DD");

            entity.ToTable("JoinProject");

            entity.Property(e => e.TimeJoinProject).HasColumnType("datetime");
            entity.Property(e => e.TimeOutProject)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinProje__Event__03F0984C");

            entity.HasOne(d => d.Role).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinProje__RoleI__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.JoinProjects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JoinProje__UserI__07C12930");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Media__3214EC0797EB161A");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .HasColumnName("MediaURL");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__3214EC073FDC12C3");

            entity.ToTable("Province");

            entity.Property(e => e.ProvinceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07A666102D");

            entity.ToTable("Report");

            entity.Property(e => e.Reason).HasMaxLength(1000);
            entity.Property(e => e.SendTime).HasColumnType("datetime");

            entity.HasOne(d => d.SendFromNavigation).WithMany(p => p.ReportSendFromNavigations)
                .HasForeignKey(d => d.SendFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__SendFrom__09A971A2");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.ReportSendToNavigations)
                .HasForeignKey(d => d.SendTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__SendTo__0B91BA14");

            entity.HasOne(d => d.Task).WithMany(p => p.Reports)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__TaskId__0D7A0286");
        });

        modelBuilder.Entity<ReportMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportMe__3214EC07C2321FC1");

            entity.HasOne(d => d.Media).WithMany(p => p.ReportMedia)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportMed__Media__0F624AF8");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportMedia)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportMed__Repor__114A936A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07D70769ED");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<SendRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SendRequ__3214EC07D7C0EDC2");

            entity.ToTable("SendRequest");

            entity.Property(e => e.Reason).HasMaxLength(1000);

            entity.HasOne(d => d.Event).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SendReque__Event__1332DBDC");

            entity.HasOne(d => d.Manager).WithMany(p => p.SendRequests)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__SendReque__Manag__151B244E");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubTask__3214EC07309C87B4");

            entity.ToTable("SubTask");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubTaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubTask__CreateB__17036CC0");

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubTask__TaskId__18EBB532");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC07947E819F");

            entity.ToTable("Task");

            entity.Property(e => e.AmountBudget).HasColumnType("money");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TaskName).HasMaxLength(255);

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__CreateBy__1AD3FDA4");

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__GroupId__1CBC4616");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07BA8F3E40");

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
                .HasConstraintName("FK__User__AddressId__1EA48E88");

            entity.HasOne(d => d.Avatar).WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarId)
                .HasConstraintName("FK__User__AvatarId__208CD6FA");

            entity.HasOne(d => d.Campus).WithMany(p => p.Users)
                .HasForeignKey(d => d.CampusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__CampusId__22751F6C");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC0790785854");

            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__RoleId__245D67DE");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__UserId__2645B050");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__3214EC078ABF4832");

            entity.ToTable("Ward");

            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.WardName).HasMaxLength(255);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ward__DistrictID__282DF8C2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
