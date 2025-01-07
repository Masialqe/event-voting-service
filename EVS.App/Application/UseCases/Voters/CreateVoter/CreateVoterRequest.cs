namespace EVS.App.Application.UseCases.Voters.CreateUser;

public sealed record CreateVoterRequest(
    string Username, 
    string Email, 
    string Password);