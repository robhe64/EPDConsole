using Chipsoft.Assignments.EPD.BLL;
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
        OperationResult? result;

        do
        {
            var firstName = GetNonNullInput("First name: ");
            var lastName = GetNonNullInput("Last name: ");
            var address = GetNonNullInput("Address: ");
        
            var patientDto = new AddPatientDto(firstName, lastName, address);
        
            result = patientManager.AddPatient(patientDto);

            Console.WriteLine("\n" + (result.Success ?
                "Patient added." :
                "Error: " + string.Join(", ", result.Errors)));
        } while (!result.Success);
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private void DeletePatient()
    {
        var patients = patientManager.GetAllPatients().ToList();
        
        if (patients.Count == 0)
        {
            Console.WriteLine("No patients to delete.");
            return;
        }

        bool parsed, indexNotInRange;
        int patientIndex;
        
        do
        {
            Console.Clear();
            for (var i = 0; i < patients.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {patients[i].FirstName} {patients[i].LastName}");
            }

            Console.Write("\nSelect patient to delete or type 'c' to cancel: ");
            var input = Console.ReadLine();
            if (input?.ToLower() == "c") return;
            
            parsed = int.TryParse(input, out patientIndex);
        
            indexNotInRange = patientIndex - 1 < 0 || patientIndex - 1 >= patients.Count;
        } while (!parsed || indexNotInRange);
        
        patientIndex--;
        var patient = patients[patientIndex];
        patientManager.DeletePatient(patient.Id);
        
        Console.WriteLine("Patient deleted. Press any key to continue...");
        Console.ReadKey();
    }
    
    
    private void DeletePhysician()
    {
        
    }

    private void AddPhysician()
    {
    }

    private void AddAppointment()
    {
    }
    
    private void ShowAppointment()
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
        Console.WriteLine("1 - Add patient");
        Console.WriteLine("2 - Delete patient");
        Console.WriteLine("3 - Add physician");
        Console.WriteLine("4 - Delete physician");
        Console.WriteLine("5 - Add appointment");
        Console.WriteLine("6 - View appointments");
        Console.WriteLine("7 - Close");
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