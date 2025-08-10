using System.Globalization;
using Chipsoft.Assignments.EPD.BLL;
using Chipsoft.Assignments.EPD.BLL.Dto;
using Chipsoft.Assignments.EPD.BLL.Managers;
using Chipsoft.Assignments.EPD.DAL.EF;

namespace Chipsoft.Assignments.EPDConsole;

public class ConsoleApp(IPatientManager patientManager, IPhysicianManager physicianManager, IAppointmentManager appointmentManager)
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

        var patientIndex = ConsoleUtilities.ChooseFromList(patients, "patient");
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
            var firstName = ConsoleUtilities.GetNonNullInput("First name: ");
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

        var physicianIndex = ConsoleUtilities.ChooseFromList(physicians, "physician");
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
        var physicians = physicianManager.GetAllPhysicians().ToList();
        var patients = patientManager.GetAllPatients().ToList();

        var physicianIndex = ConsoleUtilities.ChooseFromList(physicians, "physician");
        if (physicianIndex == null) return;
        
        var patientIndex = ConsoleUtilities.ChooseFromList(patients, "patient");
        if (patientIndex == null) return;
        
        var physician = physicians[physicianIndex.Value];
        var patient = patients[patientIndex.Value];

        Console.Clear();
        
        bool dateParsed;
        DateTime dateTimeParsed;
        do
        {
            var dateTime = ConsoleUtilities.GetNonNullInput("Date and time (yyyy-MM-dd HH:mm): ");
            dateParsed = DateTime.TryParseExact(dateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeParsed);
        } while (!dateParsed);
        
        Console.Clear();
        
        bool durationParsed;
        
        int duration;
        do
        {
            var durationString = ConsoleUtilities.GetNonNullInput("Duration (minutes): ");
            durationParsed = int.TryParse(durationString, out duration);
        } while (!durationParsed);
        
        var result = appointmentManager.AddAppointment(new AddAppointmentDto(patient.Id, physician.Id, dateTimeParsed, duration));
        
        Console.WriteLine("\n" + (result.Success ?
            "Appointment added." :
            "Error: " + string.Join(", ", result.Errors)));
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    private void ShowAppointment()
    {
        Console.WriteLine("View all appointments (1), or view appointments of a physician (2) or patient (3)?: ");

        int choice;

        do
        {
            var input = Console.ReadLine();
            if (!int.TryParse(input, out choice))
            {
                Console.WriteLine("Please enter a valid number.");
            }
        } while (!new List<int> { 1, 2, 3 }.Contains(choice));

        List<ShowAppointmentDto>? appointments = null;
        
        switch (choice)
        {
            case 1:
                appointments = appointmentManager.GetAllAppointments().ToList();
                break;
            
            case 2:
                var physicians = physicianManager.GetAllPhysicians().ToList();
                var physicianIndex = ConsoleUtilities.ChooseFromList(physicians, "physician");
                if (physicianIndex == null) return;
                var physician = physicians[physicianIndex.Value];
                appointments = appointmentManager.GetAllAppointmentsOfPhysician(physician.Id).ToList();
                break;
                
           case 3:
               var patients = patientManager.GetAllPatients().ToList();
                var patientIndex = ConsoleUtilities.ChooseFromList(patients, "patient");
                if (patientIndex == null) return;
                var patient = patients[patientIndex.Value];
                appointments = appointmentManager.GetAllAppointmentsOfPatient(patient.Id).ToList();
                break;
        }

        if (appointments == null || appointments.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
        
        Console.Clear();
        foreach (var appointment in appointments)
        {
            Console.WriteLine(appointment.ToString());
        }
        
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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