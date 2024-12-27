using DemoChat;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddHostedService<Worker>();


var host = builder.Build();
await host.RunAsync();
