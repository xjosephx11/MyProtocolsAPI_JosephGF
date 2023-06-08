using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyProtocolsAPI_JosephGF.Models;

namespace MyProtocolsAPI_JosephGF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //vamos a leer la etiqueta cnnstr de appsettings.json 
            //para configurar la conexion a la base de datos.
            var CnnStrBuilder = new SqlConnectionStringBuilder
                (builder.Configuration.GetConnectionString("CNNSTR"));
            //eliminamos de la cnnstr el dato del passwork ya que seria muy sencillo
            //obtener la info de conexion del usuario de sql server del archivo de config appsettings.json

            CnnStrBuilder.Password = "123456";

            //cnnstrbuilder es un objeto que permite la construccion de cadenas de conexion a bd
            //se pueden modificar cada parte de la misma pero el final debemos extraer un string con la info final
            string cnnStr = CnnStrBuilder.ConnectionString;

            //ahora conectamos el proyecto a la base de datos usando cnnstr
            builder.Services.AddDbContext<MyProtocolsDBContext>(options => options.UseSqlServer(cnnStr));


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


        }
    }
}