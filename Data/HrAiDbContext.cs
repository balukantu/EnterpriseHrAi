using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Data;

public class HrAiDbContext : DbContext
{
    public HrAiDbContext(DbContextOptions<HrAiDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();
    public DbSet<AiInteractionLog> AiInteractionLogs => Set<AiInteractionLog>();
    public DbSet<PromptVersion> PromptVersions => Set<PromptVersion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LeaveBalance>()
            .HasOne(lb => lb.Employee)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(lb => lb.EmployeeId);

        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId);

        modelBuilder.Entity<ChatSession>()
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(x => x.ChatSession)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatSessionId);

        modelBuilder.Entity<ChatMessage>()
            .HasIndex(x => new { x.ChatSessionId, x.CreatedAt });

        modelBuilder.Entity<Payroll>()
    .HasOne(x => x.Employee)
    .WithMany()
    .HasForeignKey(x => x.EmployeeId);

        modelBuilder.Entity<Payroll>()
            .HasIndex(x => new { x.EmployeeId, x.PayMonth });
        modelBuilder.Entity<DocumentChunk>()
    .HasOne(x => x.Document)
    .WithMany(x => x.Chunks)
    .HasForeignKey(x => x.DocumentId);

        modelBuilder.Entity<DocumentChunk>()
            .HasIndex(x => x.DocumentId);
    }
}