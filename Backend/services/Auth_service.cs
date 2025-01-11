using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public enum UserRole
{
    User = 1,
    EventOrganizer = 2,
    Admin = 3
}

public static class Policies
{
    public const string RequireUserRole = "RequireUserRole";
    public const string RequireEventOrganizerRole = "RequireEventOrganizerRole";
    public const string RequireAdminRole = "RequireAdminRole";
}

public static class AuthorizationSetup
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.RequireUserRole, policy => 
                policy.RequireRole(UserRole.User.ToString(), UserRole.EventOrganizer.ToString(), UserRole.Admin.ToString()));
            options.AddPolicy(Policies.RequireEventOrganizerRole, policy => 
                policy.RequireRole(UserRole.EventOrganizer.ToString(), UserRole.Admin.ToString()));
            options.AddPolicy(Policies.RequireAdminRole, policy => 
                policy.RequireRole(UserRole.Admin.ToString()));
        });
    }
}
