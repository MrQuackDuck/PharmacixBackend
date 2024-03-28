
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pharmacix.DatabaseContexts;
using Pharmacix.Services;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<PharmacixDbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserAccessor>();
builder.Services.AddScoped<MedicamentRepository>();
builder.Services.AddScoped<MedicamentCategoryRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Configure the cookie name
                context.Token = context.Request.Cookies["jwt"];

                return Task.CompletedTask;
            }
        };
    });

// Adding CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenWithConventionalRoutes(options =>
{
    options.IgnoreTemplateFunc = (template) => template.StartsWith("API/");
    options.SkipDefaults = true;
});

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "Swagger";
        options.EnableDeepLinking();
    });
}

app.MapControllerRoute("default", "API/{controller}/{action}/");

app.Run();