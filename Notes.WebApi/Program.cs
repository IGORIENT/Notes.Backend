using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.Persistence;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.WebApi.Middleware;
using System.Reflection;

//все что здесь происходит - инициализация приложения
var builder = WebApplication.CreateBuilder(args);


#region регистрация сервисов

// регистрация автомаппер:
// 1) создается конфигураия AutoMapper
// 2) регистрируется сервис IMapper в контейнере DI
builder.Services.AddAutoMapper(cfg =>  // под капотом создается конфигурация MapperConfiguration
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())); // возвращает только ту сборку, из которой запущен код (в данном случае Notes.WebApi)
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly)); // у любого Type есть свойство Assembly
});
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();

// для теста сделам так, чтобы на наш сайт могли заходить и стучаться кто угодно
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(config=>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options => 
    {
        options.Authority = "https://localhost:44319/";
        options.Audience = "NotesWebAPI";
        options.RequireHttpsMetadata = false;
    });
#endregion

#region построение приложения
var app = builder.Build();
#endregion


#region Middleware  (эквивалент Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();   //порядок важен, так как прежде чем быть авторизованым делать что-то, нужно пройти аутентификацию
app.UseAuthorization();

app.MapControllers();
#endregion

#region Инициализация БД (Scope + DbInitializer)

using (var scope = app.Services.CreateScope()) //создается scope - временный контейнер зависимостей
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>(); //Вытаскиваю из DI реализацию интерфейса INotesDbContext
        DbInitializer.Initialize(context);  //
    }
    catch (Exception exeption)
    {

    }
}
#endregion

// запуск веб приложение - оно уходит в цикл обработки запросов.
app.Run();
