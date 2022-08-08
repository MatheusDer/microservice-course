var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PLATFORM
app.MapPost("api/v1/c/platforms", () =>
{
    Console.WriteLine("--> Inbound POST # Command Service");

    return Results.Ok("Inbound test from Platforms Endpoint");
});
#endregion

#region COMMAND
#endregion

//app.UseHttpsRedirection();

app.Run();