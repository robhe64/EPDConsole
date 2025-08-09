using Chipsoft.Assignments.EPD.BLL;
using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL.EF;

namespace Chipsoft.Assignments.EPDConsole;

public class ConsoleApp(IPatientManager patientManager, IPhysicianManager physicianManager)
{

    
    private void AddPatient()
    {
        OperationResult? result;

        do
        {
            var firstName = ConsoleUtilities.GetNonNullInput("First name: ");
            var lastName = ConsoleUtilities.GetNonNullInput("Last name: ");
            var address = ConsoleUtilities.GetNonNullInput("Address: ");
        
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

        var patientIndex = ConsoleUtilities.ChooseFromList(patients);
        if (patientIndex == null) return;
        
        var patient = patients[patientIndex.Value];
        var result = patientManager.DeletePatient(patient.Id);
        
        Console.WriteLine("\n" + (result.Success ?
            "Patient deleted." :
            "Error: " + string.Join(", ", result.Errors)));
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private void AddPhysician()
    {
        OperationResult? result;

        do
        { 
            var firstName =ConsoleUtilities.GetNonNullInput("First name: ");
            var lastName = ConsoleUtilities.GetNonNullInput("Last name: ");
            var department = ConsoleUtilities.GetNonNullInput("Department: ");
            
        
            var physicianDto = new AddPhysicianDto(firstName, lastName, department);
        
            result = physicianManager.AddPhysician(physicianDto);

            Console.WriteLine("\n" + (result.Success ?
                "Physician added." :
                "Error: " + string.Join(", ", result.Errors)));
        } while (!result.Success);
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private void DeletePhysician()
    {
        var physicians = physicianManager.GetAllPhysicians().ToList();

        var physicianIndex = ConsoleUtilities.ChooseFromList(physicians);
        if (physicianIndex == null) return;
        
        var patient = physicians[physicianIndex.Value];
        var result = physicianManager.DeletePhysician(patient.Id);
        
        Console.WriteLine("\n" + (result.Success ?
            "Physician deleted." :
            "Error: " + string.Join(", ", result.Errors)));
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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