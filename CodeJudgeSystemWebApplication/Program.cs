using Microsoft.EntityFrameworkCore;
using CodeJudgeSystemWebApplication.Models;
using CodeJudgeSystemWebApplication.Options;
using Microsoft.AspNetCore.Http.Features;
using CodeJudgeSystemWebApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connetionString = builder.Configuration.GetConnectionString("DBConn");

builder.Services.AddDbContextPool<StudentContext>(
        options => options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString)));

builder.Services.AddDbContextPool<FileContext>(
        options => options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString)));

builder.Services.AddDbContextPool<AssignmentContext>(
        options => options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString)));

builder.Services.AddSingleton<IFileService, FileService>();

// Options patern?
builder.Services.Configure<AppOptions>(
    builder.Configuration.GetSection("App"));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
