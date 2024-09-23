using System.Net.WebSockets;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context
            .WebSockets
            .AcceptWebSocketAsync();
        await HandleWebSocketAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

async Task HandleWebSocketAsync(WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket
        .ReceiveAsync(new ArraySegment<byte>(buffer),
            CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        // Echo the data back to the client
        await webSocket.SendAsync(
            new ArraySegment<byte>(buffer, 0,
                result.Count),
                result.MessageType,
                result.EndOfMessage,
                CancellationToken.None);

        result = await webSocket
            .ReceiveAsync(new ArraySegment<byte>(buffer),
                CancellationToken.None);
    }

    await webSocket.CloseAsync(
        result.CloseStatus.Value,
        result.CloseStatusDescription,
        CancellationToken.None);
}



app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
