using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMem"));

builder.Services.AddTransient<HttpClient>();

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddScoped<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

Console.WriteLine($"--> CommandService endpoint {app.Configuration["CommandService"]}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

PrepDb.PrepPopulation(app);

#region ENDPOINTS
app.MapGet("api/v1/platforms", async (IPlatformRepo repo, IMapper mapper) =>
{
    var platforms = await repo.GetAllAsync();

    return Results.Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
});

app.MapGet("api/v1/platforms/{id}", async (IPlatformRepo repo, IMapper mapper, int id) =>
{
    var platform = await repo.GetByIdAsync(id);

    if (platform is null)
        return Results.NotFound();

    return Results.Ok(mapper.Map<PlatformReadDto>(platform));
});

app.MapPost("api/v1/platforms", async
(
    IPlatformRepo repo,
    IMapper mapper, PlatformCreateDto createDto,
    ICommandDataClient commandDataClient
) =>
{
    var platform = mapper.Map<Platform>(createDto);

    await repo.CreateAsync(platform);
    repo.SaveChanges();

    var readDto = mapper.Map<PlatformReadDto>(platform);

    try
    {
        await commandDataClient.SendPlatformToCommand(readDto);
    }
    catch (Exception ex)
    {
        Console.Write($"--> Could not send Synchronously: {ex.Message}");
    }

    return Results.Created($"api/v1/platforms{readDto.Id}", readDto);
});
#endregion

app.Run();
