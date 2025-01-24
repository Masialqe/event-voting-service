using EVS.App.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVS.App.Infrastructure.Database.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Event_Id");

        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .HasColumnName("Event_Name")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(500)")
            .HasColumnName("Event_Description")
            .IsRequired(false);

        builder.HasMany(x => x.VoterEvents)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}