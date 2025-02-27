using GymManagement.Api;
using GymManagement.Application;

using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Common.Middelware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.addInfrastructureMiddleware();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
