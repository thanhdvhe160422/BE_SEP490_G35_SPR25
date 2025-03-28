﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Planify_BackEnd.Models;

#nullable disable

namespace Planify_BackEnd.Migrations
{
    [DbContext(typeof(PlanifyContext))]
    [Migration("20250227142455_v0")]
    partial class v0
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Planify_BackEnd.Models.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int?>("Avatar")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Admin__3214EC078C6BBC5B");

                    b.HasIndex("Avatar");

                    b.ToTable("Admin", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Campus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CampusName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Campus__3214EC07699F6314");

                    b.ToTable("Campus", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.CategoryEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CampusId")
                        .HasColumnType("int")
                        .HasColumnName("CampusID");

                    b.Property<string>("CategoryEventName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Category__3214EC07B017394E");

                    b.HasIndex("CampusId");

                    b.ToTable("CategoryEvent", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__District__3214EC07F051D900");

                    b.HasIndex("ProvinceId");

                    b.ToTable("District", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountBudget")
                        .HasColumnType("money");

                    b.Property<int?>("CampusId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryEventId")
                        .HasColumnType("int");

                    b.Property<Guid?>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("Created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime>("EndOfEvent")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime");

                    b.Property<string>("EventDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventTitle")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("IsPublic")
                        .HasColumnType("int");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Placed")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfEvent")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TimePublic")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK__Event__3214EC07C3C53BBD");

                    b.HasIndex("CampusId");

                    b.HasIndex("CategoryEventId");

                    b.HasIndex("CreateBy");

                    b.HasIndex("ManagerId");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.EventMedium", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<int?>("MediaId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__EventMed__3214EC07C361BBF6");

                    b.HasIndex("EventId");

                    b.HasIndex("MediaId");

                    b.ToTable("EventMedia");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountBudget")
                        .HasColumnType("money");

                    b.Property<Guid?>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Group__3214EC073340868D");

                    b.HasIndex("CreateBy");

                    b.HasIndex("EventId");

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.InvoiceImagesSubTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ActualBudgetAmount")
                        .HasColumnType("money");

                    b.Property<string>("InvoiceImageUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("InvoiceImageURL");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("SubTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__InvoiceI__3214EC0707FDF766");

                    b.HasIndex("SubTaskId");

                    b.ToTable("InvoiceImagesSubTask", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.InvoiceImagesTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ActualBudgetAmount")
                        .HasColumnType("money");

                    b.Property<string>("InvoiceImageUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("InvoiceImageURL");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__InvoiceI__3214EC074D20B675");

                    b.HasIndex("TaskId");

                    b.ToTable("InvoiceImagesTask", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.JoinProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeJoinProject")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("TimeOutProject")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(NULL)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK__JoinProj__3214EC0730D721C5");

                    b.HasIndex("EventId");

                    b.HasIndex("Role");

                    b.HasIndex("UserId");

                    b.ToTable("JoinProject", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.MediaItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MediaUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("MediaURL");

                    b.HasKey("Id")
                        .HasName("PK__MediaIte__3214EC07B5BC8118");

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ProvinceName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Province__3214EC077E6D8019");

                    b.ToTable("Province", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Role__3214EC0791A7DB13");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.SendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reason")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__SendRequ__3214EC07A267419B");

                    b.HasIndex("EventId");

                    b.HasIndex("ManagerId");

                    b.ToTable("SendRequest", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.SubTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountBudget")
                        .HasColumnType("money");

                    b.Property<Guid?>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SubTaskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubTaskName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__SubTask__3214EC07F2A302FC");

                    b.HasIndex("CreateBy");

                    b.HasIndex("TaskId");

                    b.ToTable("SubTask", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountBudget")
                        .HasColumnType("money");

                    b.Property<Guid?>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<double>("Progress")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TaskDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Task__3214EC07522E353D");

                    b.HasIndex("CreateBy");

                    b.HasIndex("GroupId");

                    b.ToTable("Task", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int?>("Avatar")
                        .HasColumnType("int");

                    b.Property<int?>("CampusId")
                        .HasColumnType("int");

                    b.Property<Guid?>("CreateByAdmin")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreateByManage")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("Created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int?>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("WardId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Users__3214EC079CA1F7F4");

                    b.HasIndex("Avatar");

                    b.HasIndex("CampusId");

                    b.HasIndex("CreateByAdmin");

                    b.HasIndex("CreateByManage");

                    b.HasIndex("DistrictId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("Role");

                    b.HasIndex("WardId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Ward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DistrictId")
                        .HasColumnType("int")
                        .HasColumnName("DistrictID");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("WardName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Ward__3214EC07CBA258CF");

                    b.HasIndex("DistrictId");

                    b.ToTable("Ward", (string)null);
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Admin", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.MediaItem", "AvatarNavigation")
                        .WithMany("Admins")
                        .HasForeignKey("Avatar")
                        .HasConstraintName("FK__Admin__Avatar__48CFD27E");

                    b.Navigation("AvatarNavigation");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.CategoryEvent", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Campus", "Campus")
                        .WithMany("CategoryEvents")
                        .HasForeignKey("CampusId")
                        .HasConstraintName("FK__CategoryE__Campu__3B75D760");

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.District", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .HasConstraintName("FK__District__Provin__403A8C7D");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Event", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Campus", "Campus")
                        .WithMany("Events")
                        .HasForeignKey("CampusId")
                        .HasConstraintName("FK__Event__CampusId__59FA5E80");

                    b.HasOne("Planify_BackEnd.Models.CategoryEvent", "CategoryEvent")
                        .WithMany("Events")
                        .HasForeignKey("CategoryEventId")
                        .HasConstraintName("FK__Event__CategoryE__5AEE82B9");

                    b.HasOne("Planify_BackEnd.Models.User", "CreateByNavigation")
                        .WithMany("EventCreateByNavigations")
                        .HasForeignKey("CreateBy")
                        .HasConstraintName("FK__Event__CreateBy__571DF1D5");

                    b.HasOne("Planify_BackEnd.Models.User", "Manager")
                        .WithMany("EventManagers")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK__Event__ManagerId__59063A47");

                    b.Navigation("Campus");

                    b.Navigation("CategoryEvent");

                    b.Navigation("CreateByNavigation");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.EventMedium", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Event", "Event")
                        .WithMany("EventMedia")
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__EventMedi__Event__619B8048");

                    b.HasOne("Planify_BackEnd.Models.MediaItem", "Media")
                        .WithMany("EventMedia")
                        .HasForeignKey("MediaId")
                        .HasConstraintName("FK__EventMedi__Media__628FA481");

                    b.Navigation("Event");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Group", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.User", "CreateByNavigation")
                        .WithMany("Groups")
                        .HasForeignKey("CreateBy")
                        .HasConstraintName("FK__Group__CreateBy__6B24EA82");

                    b.HasOne("Planify_BackEnd.Models.Event", "Event")
                        .WithMany("Groups")
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__Group__EventId__6C190EBB");

                    b.Navigation("CreateByNavigation");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.InvoiceImagesSubTask", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.SubTask", "SubTask")
                        .WithMany("InvoiceImagesSubTasks")
                        .HasForeignKey("SubTaskId")
                        .HasConstraintName("FK__InvoiceIm__SubTa__778AC167");

                    b.Navigation("SubTask");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.InvoiceImagesTask", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Task", "Task")
                        .WithMany("InvoiceImagesTasks")
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK__InvoiceIm__TaskI__7A672E12");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.JoinProject", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Event", "Event")
                        .WithMany("JoinProjects")
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__JoinProje__Event__656C112C");

                    b.HasOne("Planify_BackEnd.Models.Role", "RoleNavigation")
                        .WithMany("JoinProjects")
                        .HasForeignKey("Role")
                        .HasConstraintName("FK__JoinProjec__Role__68487DD7");

                    b.HasOne("Planify_BackEnd.Models.User", "User")
                        .WithMany("JoinProjects")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__JoinProje__UserI__66603565");

                    b.Navigation("Event");

                    b.Navigation("RoleNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.SendRequest", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.Event", "Event")
                        .WithMany("SendRequests")
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__SendReque__Event__5DCAEF64");

                    b.HasOne("Planify_BackEnd.Models.User", "Manager")
                        .WithMany("SendRequests")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK__SendReque__Manag__5EBF139D");

                    b.Navigation("Event");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.SubTask", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.User", "CreateByNavigation")
                        .WithMany("SubTasks")
                        .HasForeignKey("CreateBy")
                        .HasConstraintName("FK__SubTask__CreateB__74AE54BC");

                    b.HasOne("Planify_BackEnd.Models.Task", "Task")
                        .WithMany("SubTasks")
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK__SubTask__TaskId__73BA3083");

                    b.Navigation("CreateByNavigation");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Task", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.User", "CreateByNavigation")
                        .WithMany("Tasks")
                        .HasForeignKey("CreateBy")
                        .HasConstraintName("FK__Task__CreateBy__6EF57B66");

                    b.HasOne("Planify_BackEnd.Models.Group", "Group")
                        .WithMany("Tasks")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK__Task__GroupId__70DDC3D8");

                    b.Navigation("CreateByNavigation");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.User", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.MediaItem", "AvatarNavigation")
                        .WithMany("Users")
                        .HasForeignKey("Avatar")
                        .HasConstraintName("FK__Users__Avatar__4F7CD00D");

                    b.HasOne("Planify_BackEnd.Models.Campus", "Campus")
                        .WithMany("Users")
                        .HasForeignKey("CampusId")
                        .HasConstraintName("FK__Users__CampusId__5441852A");

                    b.HasOne("Planify_BackEnd.Models.Admin", "CreateByAdminNavigation")
                        .WithMany("Users")
                        .HasForeignKey("CreateByAdmin")
                        .HasConstraintName("FK__Users__CreateByA__52593CB8");

                    b.HasOne("Planify_BackEnd.Models.User", "CreateByManageNavigation")
                        .WithMany("InverseCreateByManageNavigation")
                        .HasForeignKey("CreateByManage")
                        .HasConstraintName("FK__Users__CreateByM__534D60F1");

                    b.HasOne("Planify_BackEnd.Models.District", "District")
                        .WithMany("Users")
                        .HasForeignKey("DistrictId")
                        .HasConstraintName("FK__Users__DistrictI__4D94879B");

                    b.HasOne("Planify_BackEnd.Models.Province", "Province")
                        .WithMany("Users")
                        .HasForeignKey("ProvinceId")
                        .HasConstraintName("FK__Users__ProvinceI__4E88ABD4");

                    b.HasOne("Planify_BackEnd.Models.Role", "RoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("Role")
                        .HasConstraintName("FK__Users__Role__5165187F");

                    b.HasOne("Planify_BackEnd.Models.Ward", "Ward")
                        .WithMany("Users")
                        .HasForeignKey("WardId")
                        .HasConstraintName("FK__Users__WardId__4CA06362");

                    b.Navigation("AvatarNavigation");

                    b.Navigation("Campus");

                    b.Navigation("CreateByAdminNavigation");

                    b.Navigation("CreateByManageNavigation");

                    b.Navigation("District");

                    b.Navigation("Province");

                    b.Navigation("RoleNavigation");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Ward", b =>
                {
                    b.HasOne("Planify_BackEnd.Models.District", "District")
                        .WithMany("Wards")
                        .HasForeignKey("DistrictId")
                        .HasConstraintName("FK__Ward__DistrictID__4316F928");

                    b.Navigation("District");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Admin", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Campus", b =>
                {
                    b.Navigation("CategoryEvents");

                    b.Navigation("Events");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.CategoryEvent", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.District", b =>
                {
                    b.Navigation("Users");

                    b.Navigation("Wards");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Event", b =>
                {
                    b.Navigation("EventMedia");

                    b.Navigation("Groups");

                    b.Navigation("JoinProjects");

                    b.Navigation("SendRequests");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Group", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.MediaItem", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("EventMedia");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Province", b =>
                {
                    b.Navigation("Districts");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Role", b =>
                {
                    b.Navigation("JoinProjects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.SubTask", b =>
                {
                    b.Navigation("InvoiceImagesSubTasks");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Task", b =>
                {
                    b.Navigation("InvoiceImagesTasks");

                    b.Navigation("SubTasks");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.User", b =>
                {
                    b.Navigation("EventCreateByNavigations");

                    b.Navigation("EventManagers");

                    b.Navigation("Groups");

                    b.Navigation("InverseCreateByManageNavigation");

                    b.Navigation("JoinProjects");

                    b.Navigation("SendRequests");

                    b.Navigation("SubTasks");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Planify_BackEnd.Models.Ward", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
