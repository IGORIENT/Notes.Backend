using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.Persistence;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.WebApi.Middleware;
using System.Reflection;

//��� ��� ����� ���������� - ������������� ����������
var builder = WebApplication.CreateBuilder(args);


#region ����������� ��������

// ����������� ����������:
// 1) ��������� ����������� AutoMapper
// 2) �������������� ������ IMapper � ���������� DI
builder.Services.AddAutoMapper(cfg =>  // ��� ������� ��������� ������������ MapperConfiguration
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())); // ���������� ������ �� ������, �� ������� ������� ��� (� ������ ������ Notes.WebApi)
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly)); // � ������ Type ���� �������� Assembly
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

#region ���������� ����������
var app = builder.Build();
#endregion


#region Middleware  (���������� Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();   //������� �����, ��� ��� ������ ��� ���� ������������� ������ ���-��, ����� ������ ��������������
app.UseAuthorization();

app.MapControllers();
#endregion

#region ������������� �� (Scope + DbInitializer)

using (var scope = app.Services.CreateScope()) //��������� scope - ��������� ��������� ������������
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>(); //���������� �� DI ���������� ���������� INotesDbContext
        DbInitializer.Initialize(context);  //
    }
    catch (Exception exeption)
    {

    }
}
#endregion

// ������ ��� ���������� - ��� ������ � ���� ��������� ��������.
app.Run();
