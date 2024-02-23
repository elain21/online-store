using Core.Users.Commands;
using Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var databaseConnectionString = builder.Configuration.GetConnectionString("OnlineStoreContext");
builder.Services.AddDbContext<OnlineStoreContext>(options =>
{
    options.UseNpgsql(
        databaseConnectionString,
        optionsBuilder =>
        {
            optionsBuilder.CommandTimeout(120);
            optionsBuilder.MigrationsAssembly("Database");
        }
    );
}, ServiceLifetime.Transient);

builder.Services.AddTransient<OnlineStoreContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCmd).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OnlineStoreContext>();
    var env = services.GetRequiredService<IHostEnvironment>();

    if (env.IsProduction())
        return;
    
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();