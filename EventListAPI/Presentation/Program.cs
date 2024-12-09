using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Configurations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Application.Validators;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Добавление DbContext с конфигурацией строки подключения
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

// Регистрация репозиториев
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Регистрация сервисов
//builder.Services.AddScoped<IEventService, EventService>();
//builder.Services.AddScoped<IParticipantService, ParticipantService>();

// Настройка AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Валидации
builder.Services.AddValidatorsFromAssembly(typeof(EventValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ParticipantValidator).Assembly);

// Контроллеры и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включение Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
