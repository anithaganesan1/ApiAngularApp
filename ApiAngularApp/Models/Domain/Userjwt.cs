namespace ApiAngularApp.Models.Domain
{
    public record Userjwt(Guid Id, string Name, string Email, string Password, string[] Roles);

    
}
