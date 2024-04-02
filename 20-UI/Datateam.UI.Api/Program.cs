using Datateam.Foundation;
using Datateam.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDatateamServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

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
});

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
});

app.Run();

