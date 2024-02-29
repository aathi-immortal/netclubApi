
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using NetClubApi.Model;
using NetClubApi.Comman;
using NetClubApi.Helper;
using NetClubApi.Modules.TeamModule;
using NetClubApi.Modules.LeagueModule;
using NetClubApi.Modules.ClubModule;
using NetClubApi.Modules.MatchModule;
using NetClubApi.Modules.UserModule;
using NetClubApi.Modules.CourtModule;

var builder = WebApplication.CreateBuilder(args);

sqlHelper.conStr = ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection");
#region dependency injection

builder.Services.AddTransient<IUserDataAccess, UserDataAccess>();
builder.Services.AddTransient<IHelper, Helper>();
builder.Services.AddTransient<IClubBussinessLogics, ClubBussinessLogic>();
builder.Services.AddTransient<IMatchBusinessLogic, MatchBusinessLogic>();
builder.Services.AddTransient<ITeamBusinessLogic, TeamBusinessLogic>();
builder.Services.AddTransient<IClubDataAccess,ClubDataAccess>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ILeagueBussinessLayer, LeagueBussinessLayer>();
builder.Services.AddTransient<ILeagueDataAccess, LeagueDataAccess>();
builder.Services.AddTransient<ICourtBussinessLayer, CourtBusinessLayer>();
builder.Services.AddTransient<ICourtDataAccess, CourtDataAccess>();
builder.Services.AddTransient<IMatchDataAccess, MatchDataAccess>();
builder.Services.AddTransient<ITeamDataAccess, TeamDataAccess>();


#endregion


builder.Services.AddCors(options => {
    options.AddPolicy("customPolicy", x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();

#region swagger conf
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

#endregion

#region Database Connectivity

builder.Services.AddDbContext<NetClubDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services.AddSignalR();

#region authentication config


// token validation middleware 
// excuted for every  http request 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("apwmdlliendaddnetknz=3mlkd652341")) // Use the same key as in generatetoken

});



#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

#region swagger middleware
app.UseSwagger();

app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = String.Empty;
        options.DocumentTitle = "My Swagger";
        options.ConfigObject.AdditionalItems["presets"] = new[] { "Bearer" };
    });

#endregion


app.UseCors("customPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
