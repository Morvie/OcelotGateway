using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotGateway.Authentication;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicy = "cors-app-policy";

builder.Configuration.AddJsonFile("ocelot.json");

//Add services to the container.
//Add Ocelot configurations.

builder.Services.AddCors(c => c.AddPolicy(corsPolicy, corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

//Configure JWT
builder.Services.ConfigureJWT(builder.Environment.IsDevelopment(), "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwjoQKGGEUoOY3mUSzs3JrEOmSiLL2QvV94UIRMe71j0zwNvg1eFgNFHu4TYs9h/dZfC+2AVuAxiiHUolGvaAw4oZ4oLA4XmG7WomBWev+bIBF3Ois1CV/z9JwJVARh/rOuhETYmQ+bMXR3AgnQhtC6XKkwqxarVZeYmJutFBCpeagaJ7ptvoJYzjppITrrofx2gfdF4yKbJq1+7J6Dxb7iG41cUq+TndQsKgA4posHr5jWzkyhmIGiLkxXafVWKGnWvtcUqaBcLSK/EvrhQx+MnLT6dgVEYhCZBfMInc9yhVJ8A1qIU9qRJp/BTl6b0yk3QvxGwwTKw54aL63GH0YwIDAQAB");

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Morvie-Keycloak", Version = "v1" });

    //First we define the security scheme
    c.AddSecurityDefinition("Bearer", //Name the security scheme
    new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
        Scheme = JwtBearerDefaults.AuthenticationScheme //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme{
                    Reference = new OpenApiReference{
                    Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                    Type = ReferenceType.SecurityScheme
                }
            },new List<string>()
        }
    });
});

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    app.MapControllers();
});
app.UseOcelot().Wait();
app.Run();