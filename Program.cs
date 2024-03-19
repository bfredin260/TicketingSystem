using NLog;

// ==================== VARIABLES ==================== //

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

// TicketFile objects
TicketFile bugTicketFile = new TicketFile() {FilePath = "bugTicketTemp.csv"}, 
           enhancementTicketFile = new TicketFile() {FilePath = "bugTicketTemp.csv"}, 
           taskTicketFile = new TicketFile() {FilePath = "bugTicketTemp.csv"}
;



// ==================== MAIN PROGRAM FUNCTIONALITY ==================== //

Console.WriteLine("\n   TICKETING SYSTEM\n----------------------");

string choice;
// loop determines user's choice ("1", "2", "3", "4", or "" to exit the program)
do{
    choice = getChoice();

    // progresses the program by which option the user has chosen
    switch(choice) {

        // add ticket for "1"
        case "1":
            newTicket();
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

} while (choice != "");



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

// get user's input for a level field while adding a ticket
Level getLevel(string prompt) {
    string inp;
    Level lev = Level.None;

    do {
        inp = getInput(prompt);

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

// get user's input(s) for a string list field while adding a ticket
List<string> getStringList(string prompt, string field, bool isNoneValid) {
    string inp = " ";

    List<string> wat = new List<string>();

    // same as the do-while loop, except it keeps track of the index (so I can number the watchers for the user)
    
    /* (inp != "done" || i == 2) && inp != "" && (!isNoneValid || (i == 1 && isNoneValid && inp.ToLower() !="none"))
            ^ that condition checks if:
                the input is not "done" 
                    OR it is the second input (the user cannot type "done" on the first input),

                AND the input is not "" (the user did not hit enter), 
                AND, either the bool is false (if the bool is false, they can't enter "none")
                    AND the input is not "none", 
                    AND it is the first input,
                    OR the bool is true (the bool just says if the user can enter "none" to create an empty List) */
    for (int i = 1; (inp != "done" || i == 2) && inp != "" && (!isNoneValid || (i == 1 && isNoneValid && inp.ToLower() !="none")); i++) {
        
        Console.Write(prompt);
        Console.Write($" (#{i})\n\nEnter to cancel");

        if(i > 1) {
            Console.Write("\n\nType \"done\" to finish");
        }

        // if the isNoneValid is true, it means the user can type "none" to quit out of this input without canceling
        // should only let the user do this on the first input
        if(isNoneValid && i == 1)
            Console.Write($"\n\nType \"none\" if there are no values for {field}");


        inp = getInput("\n\n> ");

        // if input is NOT "done" OR it is the first input
        if(inp.ToLower() != "done" || i == 1) {

            // add input to the list of watchers
            wat.Add(inp);

            // if the user did NOT cancel AND the input is NOT "none
            if(inp != "" && inp.ToLower() != "none") {

                // log added watcher
                Console.WriteLine();
                logger.Info($"Added \"{inp}\" to {field}");
            }
        }
    }

    return wat;
}

double getCost() {
    string inp;
    double cost = double.NaN;

    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n-Enter a cost estimate for this enhancement (in dollars)\n\nEnter to cancel\n\n> $");

        // if the input is NOT empty
        if(inp != "") {

            // then check if the input can NOT be parsed into a double
            if(!Double.TryParse(inp, out cost)) {
                Console.WriteLine();
                logger.Warn("Please enter a valid input!");

                // if parse fails, set cost to NaN so that the loop runs again
                cost = double.NaN;
            }
        }
    
    // loop will repeat until the input is "" (user hit enter), OR the user inputted a valid cost
    } while (inp != "" 
                && !double.IsNormal(cost)
            )
    ;

    return cost;
}

// checks if user hit enter to cancel
bool isStringCanceled(string input) {
    if(input == ""){
        Console.WriteLine();
        logger.Warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

bool isStringListCanceled(List<string> input) {
    if(input[0] == "") {
        Console.WriteLine();
        logger.Warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

bool isDoubleCanceled(double input) {
    if (double.IsNaN(input)) {
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
bool newTicket() {
    string inp;

    // loop determines which type of ticket to add, using user input with validation
    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n1) Bug/Defect\n2) Enhancement\n3) Task\n\nEnter to cancel\n\n> ");

        // if the input is NOT empty (the user did NOT hit enter)
        if(inp != "") {

            // convert input to lowercase, then first character only, then back to string
            // ("Tree" becomes "t")
            shorten(inp);

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
    if(isStringCanceled(inp)) {
        return false;
    }
    
    // gets summary for ticket
    string smr = getInput("\n\n ADD TICKET:\n-------------\n\n-Please Enter Ticket Summary:\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(smr)) {
        return false;
    } else {
        // if user DID NOT hit enter, log added summary
        Console.WriteLine();
        logger.Info($"Added summary to ticket: \"{smr}\"");
    }

    // gets status for ticket
    string sts = getStatus();

    // checks if user hit enter to cancel
    if(isStringCanceled(sts)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added status
        Console.WriteLine();
        logger.Info($"Added status to ticket: \"{sts}\"");
    }

    // gets priority level for ticket
    Level pri = getLevel("\n\n ADD TICKET:\n-------------\n\n-What priority level is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(pri == Level.None) {
        return false;
    } else {

        // if user DID NOT hit enter, log added priority
        Console.WriteLine();
        logger.Info($"Added priority level to ticket: \"{pri}\"");
    }

    // gets submitter for ticket
    string sbm = getInput("\n\n ADD TICKET:\n-------------\n\n-Who submitted this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(sbm)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added submitter
        Console.WriteLine();
        logger.Info($"Added submitter to ticket: \"{sbm}\"");
    }

    // gets assigned for ticket
    string assi = getInput("\n\n ADD TICKET:\n-------------\n\n-Who is assigned to this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(assi)) {
        return false;
    } else {

        // if user DID NOT hit enter, log assigned
        Console.WriteLine();
        logger.Info($"Assigned \"{assi}\" to ticket");
    }

    // gets watching for ticket
    List<string> watc = getStringList($"\n\n ADD TICKET:\n-------------\n\n-Who is watching this ticket?", "Watching", false);

    // checks if user hit enter to cancel
    if(isStringListCanceled(watc)) {
        return false;
    } else {

        // if user DID NOT hit enter, log watchers
        Console.WriteLine();
        logger.Info($"Added {watc.Count} watcher(s) to ticket");
    }

    // use original input ("1", "2" or "3") to decide if the user is creating a Bug ticket, an Enhancement ticket or a Task ticket
    switch(inp) {

        // bug ticket
        case "1":

            // gets severity for bug ticket
            Level sev = getLevel("\n\n ADD TICKET:\n-------------\n\n-How severe is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

            // checks if user hit enter to cancel
            if(sev == Level.None) {
                return false;

            // if the ticket was NOT cancelled:
            } else {

                //log added severity
                Console.WriteLine();
                logger.Info($"Added severity level to ticket: \"{sev}\"");
            }

            // create new bug ticket with the given inputs
            BugTicket tkt = new BugTicket() {
                Summary = smr,
                Status = sts,
                Priority = pri,
                Submitter = sbm,
                Assigned = assi,
                Watching = watc,
                Severity = sev
            };

            bugTicketFile.AddToList(tkt);

            break;

        // enhancement ticket
        case "2":

            // gets software requirements for ticket
            List<string> sftw = getStringList("\n\n ADD TICKET:\n-------------\n\n-Enter a software requirement for this ticket", "Software", true);

            Console.WriteLine();

            // checks if user hit enter to cancel
            if(isStringListCanceled(sftw)) {
                return false;
            } else if(sftw[0] != "none"){
                // if user DID NOT hit enter AND they didn't input "none", log software
                logger.Info($"Added {sftw.Count} software requirements to ticket");
            } else {
                // if user DID input "none"
                logger.Info("No software requirements were added to ticket");
            }

            double cst = getCost();

            if(isDoubleCanceled(cst)) {
                return false;
            } else {
                
                // if user DID NOT hit enter, log cost
                Console.WriteLine();
                logger.Info($"Added cost to ticket: \"{cst:#0.00}\"");
            }

            break;

        // only other option is a task ticket (inp =/= "" here)
        default:
            // TODO: ADD FUNCTIONALITY

            break;
    }

    return true;
}