namespace EVS.App.Application.UseCases.Voters.ConfirmVoterEmail;

public sealed record ConfirmVoterEmailRequest(string Email, string Code);
