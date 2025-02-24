using GymManagement.Application;

using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Common.Middelware;

var builder = WebApplication.CreateBuilder(args);

// إضافة دعم الـ Controllers
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication().AddInfrastructure();
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
