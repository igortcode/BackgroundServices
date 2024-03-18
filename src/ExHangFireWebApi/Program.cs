using ExHangFireWebApi.Context;
using ExHangFireWebApi.Servicos;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(a => a.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DbConnection"]));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IImportarProdutoServices, ImportaProdutoServices>();
builder.Services.AddScoped<IJobServices, JobServices>();


//Configuração básica Hangfire
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetSection("ConnectionStrings")["DbConnection"]));

builder.Services.AddHangfireServer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.UseHangfireDashboard();

ApplyMigrations(); //Aplica as migrações pendentes.

BackgroundServices(); //Registra o RecurringJob ao iniciar a aplicação

app.MapControllers();

app.Run();

void BackgroundServices()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IJobServices>();
        dbInitializer.ProcessarArquivoProduto();

    }
}

void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        context.Database.Migrate();
    }
}
