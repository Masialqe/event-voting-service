namespace EVS.App.Application.UseCases.Voters.ResendVoterConfirmation;

public sealed record ResendVoterConfirmationRequest(string Email, string ConfirmationUrl);
