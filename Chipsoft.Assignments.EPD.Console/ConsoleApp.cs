using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole;

public class ConsoleApp(IPatientManager patientManager)
{
    private static string GetNonNullInput(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        } while (string.IsNullOrEmpty(input));

        return input;
    }
    
    private void AddPatient()
    {
        var firstName = GetNonNullInput("Patient first name: ");
        var lastName = GetNonNullInput("Patient last name: ");
        var address = GetNonNullInput("Patient address: ");
        
        var patientDto = new AddPatientDto(firstName, lastName, address);
        
        var result = patientManager.AddPatient(patientDto);

        Console.WriteLine(result.Success ?
            "Patient added." :
            "Error: " + string.Join(", ", result.Errors));

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private void ShowAppointment()
    {
    }

    private void AddAppointment()
    {
    }

    private void DeletePhysician()
    {
    }

    private void AddPhysician()
    {
    }

    private void DeletePatient()
    {
    }
    
    private bool ShowMenu()
    {
        Console.Clear();
        foreach (var line in File.ReadAllLines("logo.txt"))
        {
            Console.WriteLine(line);
        }
        Console.WriteLine("");
        Console.WriteLine("1 - Patient toevoegen");
        Console.WriteLine("2 - Patienten verwijderen");
        Console.WriteLine("3 - Arts toevoegen");
        Console.WriteLine("4 - Arts verwijderen");
        Console.WriteLine("5 - Afspraak toevoegen");
        Console.WriteLine("6 - Afspraken inzien");
        Console.WriteLine("7 - Sluiten");
        Console.WriteLine("8 - Reset db");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    AddPatient();
                    return true;
                case 2:
                    DeletePatient();
                    return true;
                case 3:
                    AddPhysician();
                    return true;
                case 4:
                    DeletePhysician();
                    return true;
                case 5:
                    AddAppointment();
                    return true;
                case 6:
                    ShowAppointment();
                    return true;
                case 7:
                    return false;
                case 8:
                    var dbContext = new EpdDbContext();
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();
                    return true;
                default:
                    return true;
            }
        }
        return true;
    }
    
    public void Run()
    {
        while (ShowMenu())
        {
            //Continue
        }
    }
}