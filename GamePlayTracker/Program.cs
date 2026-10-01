using GamePlayTracker.Components;
using Microsoft.EntityFrameworkCore;
using GamePlayTracker.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<GamePlayTrackerContext>(o =>
    o.UseSqlite("Data Source=gameplaytracker.db"));

builder.Services.AddQuickGridEntityFrameworkAdapter();

// Add services to the container.
builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

app.Run();
