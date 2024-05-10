using Datateam.Foundation;
using Datateam.Infrastructure;
using Datateam.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDatateamServices(builder.Configuration);

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("CreateDbTenant", policy =>
        policy.RequireRole("SuperSecurityAdmin"));

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("GetTenant", policy =>
        policy.RequireRole("SuperSecurityAdmin", "SecurityAdmin"));

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("CreatTenantOrDbOrg", policy =>
        policy.RequireRole("SuperSecurityAdmin", "SuperTenantsAdmin"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/tenant", async(ITenantService tenantService) =>
{
	var x = await tenantService.GetAllTenant();
	if (x is not null)
	{
		return Results.Ok(x);
	}
	else 
	{
		return Results.NotFound();
	}
}).RequireAuthorization("GetTenant");

app.MapPost("/tenant", async (ITenantService tenantService, Tenant tenant) =>
{
    var x = await tenantService.AddTenant(tenant);
    return Results.Ok("Successfuly Done");
}).RequireAuthorization("CreatTenantOrDbOrg");

app.MapPost("/Org", async (IOrgService orgService, Guid tenantId) =>
{
    var x = await orgService.CreatOrgDataBase(tenantId);
    return Results.Ok("Successfuly Done");
}).RequireAuthorization("CreatTenantOrDbOrg");

app.MapGet("/init", async (EnterpriseDbContext dbContext) =>
{
	if(await dbContext.Database.CanConnectAsync())
	{
		return Results.Ok("Database Already Exist!");
	}
	else
	{
		var x = await dbContext.Database.EnsureCreatedAsync();
		return Results.Ok(x);
	}
}).RequireAuthorization("CreateDbTenant");

app.MapGet("/inits", async (SecurityDbContext dbContext) =>
{
    if (await dbContext.Database.CanConnectAsync())
    {
        return Results.Ok("Database Already Exist!");
    }
    else
    {
        var x = await dbContext.Database.EnsureCreatedAsync();
        return Results.Ok(x);
    }
});

app.MapPost("/Register", async (IIAMService _iamService) =>
{
    if (await _iamService.RegisterUser(null))
    {
        return Results.Ok("Successfuly Done");
    }
    return Results.BadRequest("Something went wrong");
});

app.MapPost("/auth/login", async (LoginUser user, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) =>
{
    var result = await signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

    if (result.Succeeded)
    {
        /*var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName ?? "no-email@example.com"), // User email claim
                new Claim(ClaimTypes.Name, user.UserName), // Username claim
                new Claim(ClaimTypes.Role, "User") // Default role claim (could be dynamic based on your role structure)
            };*/
        return Results.Ok(new { Message = "Login successful" });
    }

    return Results.Unauthorized();
});


/*app.MapPost("/Login", async (IIAMService _iamService, LoginUser user) =>
{
    if (await _iamService.Login(user))
    {
        var tokenString = _iamService.GenerateTokenString(user);
        return Results.Ok(tokenString);
    }
    return Results.BadRequest();
});*/

/*app.MapPost("/Login", async (IIAMService _iamService, LoginUser user) =>
{
    if (await _iamService.Login(user))
    {
        var cookie = _iamService.GenerateCookie(user);
        return Results.Ok(cookie);
    }
    return Results.BadRequest();
});*/

app.MapGroup("/account").MapIdentityApi<ApplicationUser>();

app.Run();

