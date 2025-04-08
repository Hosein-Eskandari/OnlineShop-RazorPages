using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CommentManagement.Infrastructure.EFCore;

public class CommentContext : DbContext
{
    public CommentContext(DbContextOptions<CommentContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(CommentMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasIndex(e => e.ParentId);

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId)
                //.OnDelete(DeleteBehavior.Cascade)
                .OnDelete(DeleteBehavior.NoAction);
        });


        base.OnModelCreating(modelBuilder);
    }
}