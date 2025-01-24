namespace EVS.App.Application.UseCases.Voters.ResetVoterPassword;

public sealed record ResetVoterPasswordRequest(string Email, string ConfirmationUrl)
{
    
}