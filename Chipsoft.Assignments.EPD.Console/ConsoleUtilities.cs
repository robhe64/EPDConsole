namespace Chipsoft.Assignments.EPDConsole;

internal static class ConsoleUtilities
{
    internal static string GetNonNullInput(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        } while (string.IsNullOrEmpty(input));

        return input;
    }

    internal static int? ChooseFromList<T>(IList<T> items) where T : notnull
    {
        if (items.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No items in list. Press any key to continue.");
            Console.ReadKey();
            return null;
        }

        bool parsed, indexNotInRange;
        int index;
        
        do
        {
            Console.Clear();
            for (var i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i].ToString()}");
            }

            Console.Write("\nSelect item or type 'c' to cancel: ");
            var input = Console.ReadLine();
            if (input?.ToLower() == "c") return null;
            
            parsed = int.TryParse(input, out index);
        
            indexNotInRange = index - 1 < 0 || index - 1 >= items.Count;
        } while (!parsed || indexNotInRange);
        
        return index-1; // Decremented because the user sees a list from 1 to n instead of 0 to n
    }
}