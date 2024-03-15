using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

string? choice;

List<Ticket> tickets = new List<Ticket>();

Console.WriteLine("\n   TICKETING SYSTEM\n----------------------");

do{
    choice = getChoice();

} while(choice != "");


string? getInput(string prompt) {
    Console.Write(prompt);
    
    return Console.ReadLine();
}

string? getChoice() {
    string? inp = getInput("\n\n SELECT:\n---------\n\n1) Add Ticket(s)\n2) View Tickets\n\nEnter to exit\n\n> ");
    string? ch = "";
    
    do {
        if(inp != "") {
            ch = inp.ToLower()
                        .ToCharArray()[0]
                        .ToString()
            ;

            Console.WriteLine();

            if(ch == "1" || ch == "2") {
                logger.Info($"Input: \"{ch}\"\n");

                string sel;
                switch(ch) {
                    case "1":
                        sel = "Add Ticket(s)";
                        break;
                    default:
                        sel = "View Tickets";
                        break;
                }

                logger.Info($"Selected: \"{sel}\"");
            } else {
                logger.Error("Please enter a valid option.");
            }
        }
    } while (ch != "1" && ch != "2" && ch != "");

    return ch;
}
