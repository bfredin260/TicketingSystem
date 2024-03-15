using NLog;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string file = "Tickets.csv";
string userChoice;

List<Ticket> tickets = new List<Ticket>();

Console.WriteLine("\n");

do{
    Console.WriteLine("Select an Option:\n\n1) Add Ticket(s)\n2) View Tickets");

} while(true);
