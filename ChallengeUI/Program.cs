using ChallengeCore;

using ChallengeUI;

var builder = WebApplication.CreateBuilder(args);
builder.AddCore();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tim Correy Challenge");
    c.RoutePrefix = string.Empty;
});

app.ConfigureUI();

app.Run();