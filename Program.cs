global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.HttpOverrides;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using AIIcsoftAPI.Models;
global using Swashbuckle.AspNetCore.Filters;
global using System.Net;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using AIIcsoftAPI;
using AIIcsoftAPI.Configurations;
using AIIcsoftAPI.MiddleWare;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using AIIcsoftAPI.Validators;
using AIIcsoftAPI.Validators.CMRR;
using AIIcsoftAPI.Validators.JobCardClosures;
using AIIcsoftAPI.Validators.JobCardCreation;



var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
///
//string? ledgerConnectionString = builder.Configuration.GetConnectionString("LedgerConnection");
//builder.Services.AddDbContext<NgicsoftledgerContext>(options => options.UseSqlServer(ledgerConnectionString));


string? IcsoftConnectionString = builder.Configuration.GetConnectionString("IcsoftConnection");
builder.Services.AddDbContext<SMDBIcsoftMainContext>(options => options.UseSqlServer(IcsoftConnectionString));


//string? bizsoftConnectionString = builder.Configuration.GetConnectionString("BizsoftConnection");
//builder.Services.AddDbContext<NgbizsoftContext>(options => options.UseSqlServer(bizsoftConnectionString, ServerVersion.AutoDetect(bizsoftConnectionString)));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(Options => Options.LowercaseUrls = true);
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. API Key should be provided in the 'API-KEY' header.",
        In = ParameterLocation.Header,
        Name = "API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        Scheme = "ApiKeyScheme",
        Name = "ApiKey",
        In = ParameterLocation.Header
        }, new List<string>() }
    });

    //c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //{
    //    Description = """ Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
    //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    //    Name = "Authorization",
    //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    //});

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

//setup CORS in AuthServer project, to allow WebApi make a requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
          builder =>
          {
              builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod().Build();
          });
});


//to get logged in user by injecting IHttpContextAccessor directly in Service class
builder.Services.AddHttpContextAccessor();
builder.Services.AddAllServices();
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<PdosPutModelValidator>());
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<CustomerMaterialReturnsPostModelValidator>());
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<CustomerMaterialReturnsPutModelValidator>());
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<JobCardCreationsPostModelValidator>());
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<JobCardClosuresPostModelValidator>());
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<JobCardCreationsPutModelValidator>());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/unauthorized";
        options.AccessDeniedPath = "/auth/forbidden";
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,

            //IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Secretkeyclient").Value!)),
            //ValidIssuer = builder.Configuration.GetSection("AppSettings:jwtIssuerclient").Value,

            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Secretkey").Value!)),
            ValidIssuer = builder.Configuration.GetSection("AppSettings:jwtIssuer").Value,

            //ValidAudience = builder.Configuration.GetSection("AppSettings:jwtAudience").Value
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    })
    .AddJwtBearer("JWTSchemeForClient", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Secretkeyclient").Value!)),
            ValidIssuer = builder.Configuration.GetSection("AppSettings:jwtIssuerclient").Value,

            //ValidAudience = builder.Configuration.GetSection("AppSettings:jwtAudience").Value
        };
        //options.Authority = "icsoft";

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddAuthorization(options =>
{
    //var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
    //    JwtBearerDefaults.AuthenticationScheme,
    //    CookieAuthenticationDefaults.AuthenticationScheme,
    //    "JWTSchemeForClient");

    //defaultAuthorizationPolicyBuilder =
    //    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

    //options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

    var onlySecondJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder("JWTSchemeForClient");

    options.AddPolicy("OnlyJWTSchemeForClient", onlySecondJwtSchemePolicyBuilder
        .RequireAuthenticatedUser()
        .Build());

    var onlyCookieSchemePolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);

    options.AddPolicy("OnlyCookieScheme", onlyCookieSchemePolicyBuilder
        .RequireAuthenticatedUser()
        .Build());
});



///////// start: added for testing -- webapi 
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.KnownProxies.Add(IPAddress.Parse("192.168.15.10"));
//});
///////// end: added for testing -- webapi 

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddExceptionHandler<AppExceptionHandler>();
// builder.Services.AddHttpClient<JvcosService>(client =>
// {
//     client.BaseAddress = new Uri("https://smartnetwork.azure-api.net/"); 
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.Urls.Add("http://localhost:8081");
//app.Urls.Add("http://127.0.0.1:8081");
//app.Urls.Add("http://192.168.15.10:8081");

app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();

app.UseSession();

app.UseCors("AllowAllHeaders");

///////// start: added for testing -- webapi 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

///////// end: added for testing -- webapi 
//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
// app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
