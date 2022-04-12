using GetnetProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


ExecuteServicesConfiguration(services, configuration);
ExecuteHttpRequestPipelineConfiguration(builder);

static void ExecuteServicesConfiguration(IServiceCollection services, ConfigurationManager configuration)
{
    // Add services to the container.
    var connectionString = configuration.GetConnectionString("Development");

    services.AddDbContext<global::API.Data.DataContext>((DbContextOptionsBuilder options) => options.UseSqlite(connectionString));

    services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddTransient<AuthenticationService>();
    services.AddTransient<CustomerService>();
    services.AddTransient<PaymentService>();
}

static void ExecuteHttpRequestPipelineConfiguration(WebApplicationBuilder builder)
{
    // Configure the HTTP request pipeline.
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();

    app.MapGet("/", (context) => Task.Run(() => context.Response.Redirect("/swagger")));

    app.Run();
}