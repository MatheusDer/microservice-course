using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("inMem"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICommandRepo, CommandRepo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PLATFORM

app.MapGet("api/v1/c/platforms", (ICommandRepo repo, IMapper mapper) =>
{
    Console.WriteLine("--> Getting platforms from CommandService");

    var platforms = repo.GetAllPlatforms();

    return Results.Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
});

app.MapPost("api/v1/c/platforms", () =>
{
    Console.WriteLine("--> Inbound POST # Command Service");

    return Results.Ok("Inbound test from Platforms Endpoint");
});

#endregion

#region COMMAND

app.MapGet("api/v1/c/platforms/{platformId}/commands", (ICommandRepo repo, IMapper mapper, int platformId) =>
{
    Console.WriteLine("--> Hit GET platforms/{platformId}/commands: " + platformId);

    if (!repo.PlatformExists(platformId))
        return Results.NotFound();

    var commands = repo.GetByPlatformId(platformId);

    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

app.MapGet("api/v1/c/platforms/{platformId}/commands/{commandId}", (ICommandRepo repo, IMapper mapper, int platformId, int commandId) =>
{
    Console.WriteLine("--> Hit GET platforms/{platformId}/commands/{commandId}: " + platformId + " " + commandId);

    if (!repo.PlatformExists(platformId))
        return Results.NotFound();

    var command = repo.GetById(platformId, commandId);
    if (command is null)
        return Results.NotFound();

    return Results.Ok(mapper.Map<CommandReadDto>(command));
});

app.MapPost("api/v1/c/platforms/{platformId}/commands", (ICommandRepo repo, IMapper mapper, CommandCreateDto createDto, int platformId) =>
{
    Console.WriteLine("--> Hit POST platforms/{platformId}/commands: " + platformId);

    if (!repo.PlatformExists(platformId))
        return Results.NotFound();

    var command = mapper.Map<Command>(createDto);

    repo.Create(platformId, command);
    repo.SaveChanges();

    var commandRead = mapper.Map<CommandReadDto>(command);

    return Results.Created($"api/v1/c/platforms/{platformId}/{commandRead.Id}", commandRead);
});

#endregion

//app.UseHttpsRedirection();

app.Run();