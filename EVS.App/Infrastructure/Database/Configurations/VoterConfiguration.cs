using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EVS.App.Domain.Voters;

namespace EVS.App.Infrastructure.Database.Configurations;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasColumnType("varchar(50)")
            .HasColumnName("Voter_Nickname")
            .IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
        
        builder.Property(x => x.Email)
            .HasColumnType("varchar(254)")
            .HasColumnName("Voter_Email")
            .IsRequired();
        
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.UserId)
            .HasColumnType("varchar(100)")
            .HasColumnName("Voter_UserId")
            .IsRequired();
        
        builder.HasIndex(x => x.UserId);
        
        builder
            .HasMany(x => x.VoterEvents)
            .WithOne(x => x.Voter)
            .HasForeignKey(x => x.VoterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}