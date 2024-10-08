using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Answers_API_Examen_KeirynSandi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//agregamos codigo que permite la inyecci�n de la cadena
//de conexi�n contida en appsettings.json

//1. Obtener el valor de la cadena de conexi�n en appsettings
var CnnStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("CnnStr"));

//2.Como en la cadena de conexi�n eliminamos el password, lo vamos
//a incluir directamente en este c�digo fuente.
CnnStrBuilder.Password = "123";

//3. Creamos un string con la informacion de la cadena de conexi�n.
string cnnStr = CnnStrBuilder.ConnectionString;

//4. Vamos asignar el valor de esta cadena de conexi�n al 
//BD Contex que esta en Models
builder.Services.AddDbContext<AnswersDbContext>(Options => Options.UseSqlServer(cnnStr));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
