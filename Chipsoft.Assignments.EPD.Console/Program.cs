using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL;
using Chipsoft.Assignments.EPD.DAL.EF;
using Chipsoft.Assignments.EPDConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<EpdDbContext>();

builder.Services.AddSingleton<IPatientManager, PatientManager>();

builder.Services.AddSingleton<IPatientRepository, PatientRepository>();

builder.Services.AddSingleton<ConsoleApp>();

using var host = builder.Build();

var app = host.Services.GetRequiredService<ConsoleApp>();

app.Run();