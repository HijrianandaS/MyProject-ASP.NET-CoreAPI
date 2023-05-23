using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(options =>
options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MyProjectContext")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        /*ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],*/
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<AccountRepository>();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

//Memanggil CORS
app.UseCors(options => options.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod());

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) -> if wrapper commented on 17-4-2023
//{
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); // options added on 17-4-2023
    o.RoutePrefix = string.Empty;
});
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseDeveloperExceptionPage();
app.Run();
