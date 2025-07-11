using Touhou1CCTracker.Infrastructure.Data;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Touhou1CCTracker.Application.Events;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Services;
using Touhou1CCTracker.Application.Validators;
using Touhou1CCTracker.Domain.Events;
using Touhou1CCTracker.Domain.Interfaces.Repositories;
using Touhou1CCTracker.Infrastructure.Events;
using Touhou1CCTracker.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<Touhou1CCTrackerDbContext>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IShotTypeRepository, ShotTypeRepository>();
builder.Services.AddScoped<IDifficultyRepository, DifficultyRepository>();
builder.Services.AddScoped<IReplayFileRepository, ReplayFileRepository>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IDifficultyService, DifficultyService>();
builder.Services.AddScoped<IShotTypeService, ShotTypeService>();
builder.Services.AddScoped<IReplayFileService, ReplayFileService>();
builder.Services.AddScoped<IRecordService, RecordService>();

builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEventHandler<RecordDeletedEvent>, DeleteReplayFileHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<GameValidator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Touhou1CCTracker API", Version = "v1.3" });
    c.EnableAnnotations();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition")
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();