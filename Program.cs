// See https://aka.ms/new-console-template for more information
using System.Net;

Console.WriteLine("Hello, World!");

string file = "Tickets.csv";
string userChoice;
do{
    Console.WriteLine("Select an Option:\n\n1) Add Ticket(s)\n2) View Tickets");

    userChoice = Console.ReadLine();
    if(userChoice == "1") {
        Console.Clear();

        Console.Write("\nEnter ticket summary: ");
        string summary = Console.ReadLine();

        Console.Write("\nIs this ticket open? (Y/N): ");
        string isOpen = Console.ReadLine();
        if(isOpen.ToUpper() == "Y") {
        
            isOpen = "Open";
        } else if(isOpen.ToUpper() == "N") {
            isOpen = "Closed";
        } else {
            isOpen = "Null";
        }

        Console.Write("\nEnter priority (High, Medium, Low): ");
        string priority = Console.ReadLine();

        Console.Write("\nWho submitted this ticket? ");
        string submitter = Console.ReadLine();

        Console.Write("\nWho is assigned to this ticket? ");
        string assigned = Console.ReadLine();

        Console.Write("\nWho is watching this ticket? (Separate names with '|'): ");
        string watching = Console.ReadLine();

        StreamWriter sw = new StreamWriter(file, append: true);
        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", File.ReadAllLines(file).Length + 1, summary, isOpen, priority, submitter, assigned, watching);
        sw.Close();

        Console.Clear();

        Console.WriteLine("Ticket added!\n");

    } else {
        StreamReader sr = new StreamReader(file);

        Console.Clear();

        while(!sr.EndOfStream) {
            string line = sr.ReadLine();
            string[] arr = line.Split(",");
            Console.WriteLine("\n\nTicketID: {0}\nSummary: {1}\nStatus: {2}\nPriority: {3}\nSubmitter: {4}\nAssigned: {5}\nWatching: {6}", arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
        }

        sr.Close();
    }
} while(userChoice == "1" || userChoice == "2");
