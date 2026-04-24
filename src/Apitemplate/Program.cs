using Apitemplate.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseApiPipeline();

app.Run();
