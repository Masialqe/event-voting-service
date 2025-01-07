using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EVS.App.Infrastructure.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Identity.Database;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : IdentityDbContext<VoterIdentity>(options){}