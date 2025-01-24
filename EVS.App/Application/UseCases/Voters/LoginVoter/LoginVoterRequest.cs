namespace EVS.App.Application.UseCases.Voters.LoginVoter;

public sealed record LoginVoterRequest(string Email, string Password, bool RememberMe);
