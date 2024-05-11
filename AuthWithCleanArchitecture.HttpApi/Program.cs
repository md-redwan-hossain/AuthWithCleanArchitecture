using System.Text.Json;
using AuthWithCleanArchitecture.Application;
using AuthWithCleanArchitecture.Application.Common.Options;
using AuthWithCleanArchitecture.HttpApi.Controllers;
using AuthWithCleanArchitecture.HttpApi.Utils;
using AuthWithCleanArchitecture.Infrastructure;
using AuthWithCleanArchitecture.Infrastructure.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

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
builder.Services.AddAuthPolicies();


builder.Services.AddControllers(opts =>
    {
        opts.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        opts.OutputFormatters.RemoveType<StringOutputFormatter>();
        opts.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    })
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value is { Errors.Count: > 0 })
                .Select(e => new
                {
                    Field = JsonNamingPolicy.CamelCase.ConvertName(e.Key),
                    Errors = e.Value?.Errors.Select(er => er.ErrorMessage)
                })
                .ToList();

            return context.MakeResponse(
                StatusCodes.Status400BadRequest,
                errors,
                "One or more validation errors occurred."
            );
        };
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


app.MapControllers();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.Run();