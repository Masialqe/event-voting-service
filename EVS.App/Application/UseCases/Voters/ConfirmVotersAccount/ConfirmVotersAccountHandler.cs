using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.UseCases.Voters.ConfirmVotersAccount;

public class ConfirmVotersAccountHandler : IHandler<Result, ConfirmVotersAccountRequest>
{
    public Task<Result> Handle(ConfirmVotersAccountRequest request, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}