using CommentManagement.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommentManagement.Infrastructure.EFCore.Mapping;

public class CommentMapping : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ParentId);
        builder.Property(x => x.IsCanceled);
        builder.Property(x => x.IsConfirmed);
        builder.Property(x => x.OwnerRecordId);
        builder.Property(x => x.Website).HasMaxLength(500);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.CreationDate).HasMaxLength(100).IsRequired();

        //builder.HasOne(x => x.Parent)
        //       .WithMany(x => x.Children)
        //       .HasForeignKey(x => x.ParentId)
        //       //.OnDelete(DeleteBehavior.Cascade)
        //       .OnDelete(DeleteBehavior.NoAction);
    }
}