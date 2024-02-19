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
        }
    );
}, ServiceLifetime.Transient);

builder.Services.AddTransient<OnlineStoreContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();