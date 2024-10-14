using CFRs.BE;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetEscapades.AspNetCore.SecurityHeaders;
using System.Text;
[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressConsumesConstraintForFormFileParameters = true;
        options.SuppressInferBindingSourcesForParameters = true;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
            "https://httpstatuses.com/404";
    });
var startup = new Startup(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(CFRs.DAL.Helper.GetSettingsHelper.CFRsConnectionString);
});

builder.Services.AddHangfireServer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "CFR Backend", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder =>
            builder
                .AllowAnyHeader()
                .WithOrigins("http://localhost:4204", "http://10.9.101.143:8089", "*")
                .WithMethods(
                    HttpMethod.Get.Method,
                    HttpMethod.Post.Method,
                    HttpMethod.Delete.Method,
                    HttpMethod.Options.Method
                )
                .AllowCredentials()
    );

    options.AddPolicy(
        "CorsPolicy",
        builder => builder.AllowAnyHeader().WithOrigins("http://localhost:4204", "http://10.9.101.143:8089", "*").AllowAnyMethod()
    );
});


var app = builder.Build();

app.UseHangfireDashboard();

startup.Configure(app);

//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(@"\\server\path"),
//    RequestPath = new PathString("/Export"),
//    EnableDirectoryBrowsing = true
//});

string fileProvider = Directory.GetCurrentDirectory() + "/FileUpload";
const string cacheMaxAge = "604800";
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".msg"] = "application/vnd.ms-outlook";
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(fileProvider),
    RequestPath = new PathString("/" + "FileUpload"),
    OnPrepareResponse = ctx =>
    {
        // using Microsoft.AspNetCore.Http;
        ctx.Context.Response.Headers.Append(
            "Cache-Control", $"public, max-age={cacheMaxAge}");
    }
        ,
    ContentTypeProvider = provider
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();

//    //app.UseSwaggerUI(c =>
//    //{
//    //    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
//    //    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "CFRs.BE v1.0.0");
//    //});
//}

//app.UseHttpsRedirection();

////app.UseAuthentication();
//app.UseAuthorization();

////app.MapControllers();

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//app.Run();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CFR Backend v1"));
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");
app.Run();