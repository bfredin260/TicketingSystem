using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

string choice;

List<Ticket> tickets = new List<Ticket>();

// ==================== MAIN PROGRAM FUNCTIONALITY ==================== //

Console.WriteLine("\n   TICKETING SYSTEM\n----------------------");

// loop determines user's choice ("1", "2", "3", "4", or "" to exit the program)
do{
    choice = getChoice();

    // progresses the program by which option the user has chosen
    switch(choice) {

        // add ticket for "1"
        case "1":
            addTicket();
            break;

        // view tickets for "2"
        case "2":

            break;
        
        // load tickets for "3"
        case "3":

            break;

        // save tickets for "4"
        case "4":

            break;

        // exit loop when user chooses "" (press enter)
        case "":
            Console.WriteLine();
            logger.Info("Closing program...");
            break;
    }

} while(choice != "");



// ==================== FUNCTIONS ==================== //


// ----- Basic User Input ----- //

// prompts user and then returns their response (unverified)
string getInput(string prompt) {
    Console.Write(prompt);
    
    return Console.ReadLine();
}

// shortens the input to a single character string
string shorten(string input) {
    // convert input to lowercase, then first character only, then back to string
    // ("Tree" becomes "t")
    return input.ToLower()
                .ToCharArray()[0]
                .ToString()
    ;
}

// gets user's choice (used only for selecting 1-4 to add ticket, view tickets, load and save tickets)
string getChoice() {
    string inp;
    do {
        // gets input from user
        inp = getInput("\n\n SELECT:\n---------\n\n1) Add Ticket\n2) View Ticket(s)\n3) Load Ticket(s) from File\n4) Save Tickets to File\n\nEnter to exit\n\n> ");
    
        // if the input is NOT empty:
        if(inp != "") {

            // shorten string to one character
            inp = shorten(inp);

            Console.WriteLine();

            // if user chose one of the four options, output it using logger
            if(inp == "1" || inp == "2" || inp == "3" || inp == "4") {
                string sel;
                switch(inp) {
                    case "1":
                        sel = "Add Ticket";
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

                // if not, warn user and repeat the loop
                logger.Warn("Please enter a valid option!");
            }
        }
    // loop repeats until the user inputs a valid choice
    } while (inp != "1" 
                && inp != "2" 
                && inp != "3" 
                && inp != "4" 
                && inp != ""
            );

    return inp;
}


// ----- "Add Ticket" Choice -----

// get user's input for status field while adding a ticket
string getStatus() {
    string inp;
    do{
        // get input from user
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n-Is this ticket currently open?\n\ny) Yes\nn) No\n\nEnter to cancel\n\n> ");

        // if input is NOT empty (user did NOT press enter):
        if(inp != "") {

            // shorten string to one character
            inp = shorten(inp);

            // sets input to "Open" or "Closed" based on "y" or "n",
            // or warns user if they did not enter "y" or "n"
            switch(inp) {
                case "y":
                    inp = "Open";
                    break;
                case "n":
                    inp = "Closed";
                    break;
                default:
                    Console.WriteLine();
                    logger.Warn("Please enter \"y\" or \"n\" only!");
                    break;
            }
        }

    // loop repeats until user enters "y" or "n", or user presses enter to cancel
    } while (inp != "Closed" 
                && inp != "Open" 
                && inp != ""
            )
    ;

    return inp;
}

// get user's input for priority field while adding a ticket
Level getPriority() {
    string inp;
    Level lev = Level.None;

    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n-What priority level is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

        // if input is NOT empty:
        if (inp != "") {

            // shorten string to one character
            inp = shorten(inp);

            // sets input to "High", "Medium" or "Low" based on "h", "m" or "l",
            // or warns user if they did not enter "h", "m" or "l"
            switch(inp) {
                case "h":
                    lev = Level.High;
                    break;
                case "m":
                    lev = Level.Medium;
                    break;
                case "l":
                    lev = Level.Low;
                    break;
                default:
                    Console.WriteLine();
                    logger.Warn("Please enter \"h\", \"m\" or \"l\" only!");
                    break; 
            }
        }

    // loop repeats until user enters "h", "m", "l", or "" (User pressed enter)
    } while (inp != "h" 
                && inp != "m" 
                && inp != "l" 
                && inp != ""
            )
    ;

    return lev;
}

// checks if user hit enter to cancel
bool isCanceled(string input) {
    if(input == "" || input == "None"){
        Console.WriteLine();
        logger.Warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// adds a new ticket to the list and temporary file
// asks user for inputs for ticket type and fields
bool addTicket() {
    string inp;

    // loop determines which type of ticket to add, using user input with validation
    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n1) Bug/Defect\n2) Enhancement\n3) Task\n\nEnter to cancel\n\n> ");

        // if the input is NOT empty (the user did NOT hit enter)
        if(inp != "") {

            // convert input to lowercase, then first character only, then back to string
            // ("Tree" becomes "t")
            inp = inp.ToLower()
                        .ToCharArray()[0]
                        .ToString()
            ;

            Console.WriteLine();

            // if input is one of the three options, output it using logger
            if(inp == "1" || inp == "2" || inp == "3") {

                string sel;
                switch(inp) {
                    case "1":
                        sel = "Bug/Defect";
                        break;
                    case "2":
                        sel = "Enhancement";
                        break;
                    default:
                        sel = "Task";
                        break;
                }
                logger.Info($"Selected: \"{sel}\"");
            } else {

                // if not, warn user and restart loop
                logger.Warn("Please enter a valid option!");
            }
        }
    
    // loop runs until user inputs "1", "2", "3", or "" (they pressed enter)
    } while (inp != "1" 
                && inp != "2" 
                && inp != "3" 
                && inp != ""
            )
    ;

    // checks if user hit enter to cancel
    if(isCanceled(inp)) {
        return false;
    }
    
    // gets summary for ticket
    string smr = getInput("\n\n ADD TICKET:\n-------------\n\n-Please Enter Ticket Summary:\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isCanceled(smr)) {
        return false;
    } else {
        // if user DID NOT hit enter, log added summary
        Console.WriteLine();
        logger.Info($"Added summary: \"{smr}\"");
    }

    // gets status for ticket
    string sts = getStatus();

    // checks if user hit enter to cancel
    if(isCanceled(sts)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added status
        Console.WriteLine();
        logger.Info($"Added status: \"{sts}\"");
    }

    // gets priority level for ticket
    Level pri = getPriority();

    // checks if user hit enter to cancel
    if(isCanceled(pri.ToString())) {
        return false;
    } else {

        // if user DID NOT hit enter, log added priority
        Console.WriteLine();
        logger.Info($"Added priority: \"{pri}\"");
    }

    // gets submitter for ticket
    string sbm = getInput("\n\n ADD TICKET:\n-------------\n\n-Who submitted this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isCanceled(sbm)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added submitter
        Console.WriteLine();
        logger.Info($"Added submitter: \"{sbm}\"");
    }

    // gets assigned for ticket
    string assi = getInput("\n\n ADD TICKET:\n-------------\n\n-Who is assigned to this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isCanceled(assi)) {
        return false;
    } else {

        // if user DID NOT hit enter, log assigned
        Console.WriteLine();
        logger.Info($"Added assigned: \"{assi}\"");
    }
    switch(inp) {
        case "1":
            // addBugTicket();
            break;
        case "2":
            break;
        default:
            break;
    }

    return true;
}