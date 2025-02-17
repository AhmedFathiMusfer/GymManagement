using GymManagement.Application;

using GymManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// إضافة دعم الـ Controllers
builder.Services.AddControllers();

// إضافة دعم Swagger (اختياري)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication().AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// استخدام الـ Controllers بدلاً من Minimal APIs
app.MapControllers();

app.Run();
