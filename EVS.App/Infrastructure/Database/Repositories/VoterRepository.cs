using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class VoterRepository(
    ApplicationDbContext context) : IVoterRepository
{
    public async Task<Result> AddVoterAsync(Voter voter, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.Voters.AddAsync(voter, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            //TODO Infrasttructure errors
            return (Error)ex;
        }
    }
  
    public async Task<Result<Voter>> GetVoterByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await context.Voters.FirstOrDefaultAsync(x => x.Username == name, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            //TODO Infrasttructure errors
            return (Error)ex;
        }
    }

    public async Task<Result<Voter>> GetVoterByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await context.Voters.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            //TODO Infrasttructure errors
            return (Error)ex;
        }
    }
}