using Notes.Persistence;
using Notes.Application;
using System.Reflection;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;

//��� ��� ����� ���������� - ������������� ����������
var builder = WebApplication.CreateBuilder(args);


#region ����������� ��������
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();

// ��� ����� ������ ���, ����� �� ��� ���� ����� �������� � ��������� ��� ������
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

#region ���������� ����������
var app = builder.Build();
#endregion


#region Middleware  (���������� Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
#endregion

#region ������������� �� (Scope + DbInitializer)
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>(); //���������� �� DI ���������� ���������� INotesDbContext
        DbInitializer.Initialize(context);
    }
    catch (Exception exeption)
    {

    }
}
#endregion

// ������ ��� ���������� - ��� ������ � ���� ��������� ��������.
app.Run();
