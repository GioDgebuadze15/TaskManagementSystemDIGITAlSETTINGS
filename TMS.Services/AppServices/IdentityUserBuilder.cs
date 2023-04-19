using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TMS.Services.AppServices;

public class IdentityUserBuilder
{
    private string? _name;
    private string? _password;
    private readonly List<string> _roles = new();
    private readonly List<Claim> _claims = new();

    public IdentityUserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public IdentityUserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public IdentityUserBuilder WithRole(string role)
    {
        _roles.Add(role);
        return this;
    }

    public IdentityUserBuilder WithClaim(string type, string value)
    {
        _claims.Add(new Claim(type, value));
        return this;
    }

    public IdentityUser Build(UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser(_name);
        userManager.CreateAsync(user, _password).GetAwaiter().GetResult();
        foreach (var role in _roles)
        {
            userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
        }

        foreach (var claim in _claims)
        {
            userManager.AddClaimAsync(user, claim).GetAwaiter().GetResult();
        }

        return user;
    }
}