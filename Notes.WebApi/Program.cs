using Notes.Persistence;
using Notes.Application;
using System.Reflection;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;

//все что здесь происходит - инициализаци€ приложени€
var builder = WebApplication.CreateBuilder(args);


#region регистраци€ сервисов
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();

// дл€ теста сделам так, чтобы на наш сайт могли заходить и стучатьс€ кто угодно
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
#endregion

#region построение приложени€
var app = builder.Build();
#endregion


#region Middleware  (эквивалент Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
#endregion

#region »нициализаци€ Ѕƒ (Scope + DbInitializer)
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>(); //¬ытаскиваю из DI реализацию интерфейса INotesDbContext
        DbInitializer.Initialize(context);
    }
    catch (Exception exeption)
    {

    }
}
#endregion

// запуск веб приложение - оно уходит в цикл обработки запросов.
app.Run();
