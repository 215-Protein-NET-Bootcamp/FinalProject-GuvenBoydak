﻿using AutoMapper;
using FinalProject.Api;
using FinalProject.Api.Middleware;
using FinalProject.Business;
using FinalProject.DataAccess;
using FluentMigrator.Runner;
using FluentValidation.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Fluent Migrations
builder.Services.AddFluentMigratorCore()
            // Configure the runner
            .ConfigureRunner(
            buil => buil
            // Use PosgreSql
            .AddPostgres11_0()
            // connection string
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("PosgreSql"))
            // Specify the assembly with the migrations
            .WithMigrationsIn(typeof(InitialMigration).Assembly));


builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(GenericService<>));
builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IProductRepository, DpProductRespository>();
builder.Services.AddScoped<ICategoryRepository, DpCategoryRespository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();



//Default FluentValidation Filterini devre dışı bırakıp kendi yazdıgımız ValidatorFilterAttribute u ekliyoruz.
builder.Services.AddControllers(option => option.Filters.Add<ValidatorFilterAttribute>()).AddFluentValidation(x => 
x.RegisterValidatorsFromAssemblyContaining(typeof(ProductAddDtoValidator)));

//AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapProfile());
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

//fluent migration 
try
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        //Instantiate the runner
        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        //Execute the migrations
        runner.MigrateUp();

        Console.WriteLine("Migration has successfully executed.");
    }
}
catch (Exception e)
{
    throw new Exception(e.Message);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Exceptionları handler etigimiz middleware
app.UseCustomExeption();

app.UseAuthorization();

app.MapControllers();

app.Run();
