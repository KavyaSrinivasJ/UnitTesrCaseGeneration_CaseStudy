var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyPolicy",
        builder =>
        {
            builder.WithOrigins("https://localhost:44328",
                                "http://localhost:4200", "http://localhost:80", "https://localhost:80", "http://localhost:5173/", "https:localhost:3000", "*")
                    .WithMethods("POST", "PUT", "DELETE", "GET")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowLocalhost3000");

app.Run();

