using Microsoft.AspNetCore.Components.Authorization;
using ThreeSC.Net6Lib.BlazorTools.Models;
using ThreeSC.Net6Lib.BlazorTools.ServiceExtensions;
using UniANPR.Areas.Identity;
using UniANPR.Models.ThreeSCBaseImplementations;
using UniANPR.Services;
using static UniANPR.Models.ThreeSCBaseImplementations.TestAppUserAuditEnum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// -----------------------------
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ThreeSCBaseApplicationUser>>();

// add telerik service
// -------------------
builder.Services.AddTelerikBlazor();

// add ThreeSC Blazor Nuget services
// ---------------------------------
builder.Services.AddThreeSCBlazor<TestAppUser, _enmTestAppEnum>(builder.Configuration);

// add ABDS Specific services
// --------------------------
builder.Services.AddSingleton<ITestService, TestService>();

// Build the web application
// -------------------------
var app = builder.Build();

// Configure the ThreeSC Blazor Nuget services
// -------------------------------------------
app.ThreeSCBlazorInitiialise<TestAppUser>();

// Configure the HTTP request pipeline.
// ------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
