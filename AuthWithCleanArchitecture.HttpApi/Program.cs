using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AuthWithCleanArchitecture.Application;
using AuthWithCleanArchitecture.Application.Common.Options;
using AuthWithCleanArchitecture.HttpApi.Utils;
using AuthWithCleanArchitecture.Infrastructure;
using AuthWithCleanArchitecture.Infrastructure.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.BindAndValidateOptions<AppSecretOptions>(AppSecretOptions.SectionName);
builder.Services.BindAndValidateOptions<JwtOptions>(JwtOptions.SectionName);

await builder.Services.AddDatabaseConfigAsync(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();


builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(
        namingPolicy: JsonNamingPolicy.CamelCase,
        allowIntegerValues: false)
    );
});

builder.Services.AddSwaggerGen(o =>
{
    o.SupportNonNullableReferenceTypes();
    o.SchemaFilter<ApiResponseSchemaFilter>();
});

ValidatorOptions.Global.DisplayNameResolver = (_, member, _) =>
    member is not null ? JsonNamingPolicy.CamelCase.ConvertName(member.Name) : null;


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapApiEndpointsFromAssembly(Assembly.GetExecutingAssembly());

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.Run();