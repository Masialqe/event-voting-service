using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Identity.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<VoterIdentity>(options)
{
}