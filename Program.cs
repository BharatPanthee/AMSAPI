using AMSAPI.Authentication;
using AMSAPI.Services;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const bool useSwagger = false;
builder.Services.AddControllers();
builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddScoped<IUserService, UserService>();
if(useSwagger)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });
    //.AddJwtBearer(options =>
    //{
    //    options.Authority = "https://securetoken.google.com/1:288681664925:web:45fc2ed508bc9396b9cf74";
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidIssuer = "https://securetoken.google.com/1:288681664925:web:45fc2ed508bc9396b9cf74",
    //        ValidateAudience = true,
    //        ValidAudience = "1:288681664925:web:45fc2ed508bc9396b9cf74",
    //        ValidateLifetime = true
    //    };
    //});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (useSwagger && app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
