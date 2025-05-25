using Microsoft.OpenApi.Models;
using CalendarService.Data;
using Microsoft.EntityFrameworkCore;

// Tips  & Trix video och felsökt med chatgpt

var builder = WebApplication.CreateBuilder(args);


var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CalendarDbContext>(opts =>
    opts.UseSqlServer(conn)
);


builder.Services.AddCors(o =>
    o.AddPolicy("AllowFrontend", p =>
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader()
    )
);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CalendarService API",
        Version = "v1",
        Description = "Calendar microservice with EF Core & Azure SQL"
    });
});
Console.WriteLine("Trigger build"); // temporär rad


// Ändrat för kolla om det funkar i Azure
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalendarService v1"));


app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();
