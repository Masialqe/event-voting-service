﻿using EVS.App.Domain.VoterEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVS.App.Infrastructure.Database.Configurations;

public class VoterEventConfiguration : IEntityTypeConfiguration<VoterEvent>
{
    public void Configure(EntityTypeBuilder<VoterEvent> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("VoterEvent_Id");
        
        builder.HasOne(x => x.Voter)
            .WithMany(x => x.VoterEvents)
            .HasForeignKey(x => x.VoterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Event)
            .WithMany(x => x.VoterEvents)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.Score)
            .IsRequired()
            .HasColumnName("VoterEvent_Score")
            .HasDefaultValue(0);

        builder.Property(x => x.HasVoted)
            .IsRequired()
            .HasColumnName("VoterEvent_HasVoted")
            .HasDefaultValue(false);
    }
}