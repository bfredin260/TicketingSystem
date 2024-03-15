using System.ComponentModel;
using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

string? choice;

List<Ticket> tickets = new List<Ticket>();

Console.WriteLine("\n   TICKETING SYSTEM\n----------------------");

do{
    choice = getChoice();

    switch(choice) {
        case "1":
            // addTicket();
            break;
        case "2":

            break;
        case "3":

            break;
        case "4":

            break;
        case "":
            logger.Info("Closing program...");
            break;
    }

} while(choice != "");


string? getInput(string prompt) {
    Console.Write(prompt);
    
    return Console.ReadLine();
}

string? getChoice() {
    string? inp = "";
    do {
        inp = getInput("\n\n SELECT:\n---------\n\n1) Add Ticket(s)\n2) View Ticket(s)\n3) Load Ticket(s) from File\n4) Save Tickets to File\n\nEnter to exit\n\n> ");
    
        if(inp != "") {
            inp = inp.ToLower()
                        .ToCharArray()[0]
                        .ToString()
            ;

            Console.WriteLine();

            if(inp == "1" || inp == "2" || inp == "3" || inp == "4") {
                logger.Info($"Input: \"{inp}\"\n");

                string sel;
                switch(inp) {
                    case "1":
                        sel = "Add Ticket(s)";
                        break;
                    case "2":
                        sel = "View Ticket(s)";
                        break;
                    case "3":
                        sel = "Load Ticket(s) from File";
                        break;
                    default:
                        sel = "Save Ticket(s) to File";
                        break;
                }
                logger.Info($"Selected: \"{sel}\"");
            } else {
                logger.Warn("Please enter a valid option!");
            }
        }
    } while (inp != "1" 
                && inp != "2" 
                && inp != "3" 
                && inp != "4" 
                && inp != ""
            );

    return inp;
}
