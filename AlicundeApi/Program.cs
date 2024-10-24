
using AlicundeApi.DataBase;
using AlicundeApi.Interfaces;
using AlicundeApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AlicundeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            builder.Services.AddControllers();
       
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //// Agregar servicios para los controladores
            //builder.Services.AddControllers();

            builder.Services.AddHttpClient<IBankService, BankService>();
            builder.Services.AddHttpClient<IFeesService, FeesService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
