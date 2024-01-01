using System.Text;
using final_project_be.Helpers;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ScheduleRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<UserClassRepository>();



// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy =>
//                       {
//                           policy.WithOrigins("http://localhost:5173");
//                       });
// });



//add swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//add auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    string jwtSecret = JWTHelper.KEY;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors(MyAllowSpecificOrigins);

//ini tambahan
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// ini tambahan
app.Use((context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
    return next();
});

app.UseStaticFiles();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
