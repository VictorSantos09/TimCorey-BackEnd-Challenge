using ChallengeCore.Services;
using ChallengeUI.EndPoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<UserService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    _ = app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tim Correy Challenge");
    c.RoutePrefix = string.Empty;
});

app.MapUserEndPoints();
app.MapProductsEndPoints();
app.Run();