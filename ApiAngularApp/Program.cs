using ApiAngularApp;
using ApiAngularApp.Data;
using ApiAngularApp.Exception;
using ApiAngularApp.Respositories.Implementation;
using ApiAngularApp.Respositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using static Microsoft.Extensions.Http.Polly;
using  Polly;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.OpenApi.Models;
using ApiAngularApp.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://projectapi.gerasim.in");
                      });
});

builder.Services.AddHttpClient(); // Register IHttpClientFactory
//builder.Services.AddHttpClient<ApiService>();
builder.Services.AddHttpClient<ApiService>().AddTransientHttpErrorPolicy(policy =>
  policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt)));


// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionhandler>();
//builder.Services.AddExceptionHandler<TimeOutException>();

//builder.Services.AddExceptionHandler<DefaultException>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiAngularApp", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
    

});

builder.Services.AddDbContext<AppDBcontext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDbConnection"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DBCON"));

});
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDBcontext>();

builder.Services.AddRazorPages();




builder.Services.AddScoped<IcategoryRepository, CategoryRespository>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddTransient<tokenServices>();
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
           // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt"])),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jwt:Key")),

            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


//})
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience =builder. Configuration["Jwt:Issuer"],
//            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey))

//            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),

//        };
//    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
});

var app = builder.Build();
//IApplicationBuilder applicationBuilder = app.Usecusmiddleware();
app.Usecusmiddleware();

app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthCore API V1");
    });
}
app.UseMiddleware<RequestLoggingMiddlewares>();
app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapPost("/authenticate", (Userjwt users, tokenServices tokService)=> tokService.GenerateToken(users));
app.MapGet("/signin", () => "User Authenticated Successfully!").RequireAuthorization("Admin");
app.UseDeveloperExceptionPage(); // TEMPORARILY only

app.Run();
