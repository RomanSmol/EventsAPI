using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Configurations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Application.Validators;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ���������� DbContext � ������������� ������ �����������
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

// ����������� ������������
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// ����������� ��������
//builder.Services.AddScoped<IEventService, EventService>();
//builder.Services.AddScoped<IParticipantService, ParticipantService>();

// ��������� AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ���������
builder.Services.AddValidatorsFromAssembly(typeof(EventValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ParticipantValidator).Assembly);

// ����������� � Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� Swagger � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
