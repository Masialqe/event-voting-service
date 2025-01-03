using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Identity.Users;

internal sealed class VoterIdentityConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        throw new NotImplementedException();
    }
}