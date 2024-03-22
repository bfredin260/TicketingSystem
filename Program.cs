using NLog;

// ==================== VARIABLES ==================== //

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

// TicketFile objects
TicketFile bugTicketFile = new TicketFile(), 
           enhancementTicketFile = new TicketFile(), 
           taskTicketFile = new TicketFile()
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
            info("Closing program...");
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


// -- just to shorten code a little bit --

// just a long line to divide each prompt
void line() {
    Console.WriteLine("=========================================================================\n\n");
}

// line + logger.Info()
void info(string msg) {
    line();
    logger.Info(msg);
}

// line +logger.Warn()
void warn(string msg) {
    line();
    logger.Warn(msg);
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
                info($"Selected: \"{sel}\"");
            } else {

                // if not, warn user and repeat the loop
                warn("Please enter a valid option!");
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
                    warn("Please enter \"y\" or \"n\" only!");
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
                    warn("Please enter \"h\", \"m\" or \"l\" only!");
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
    
    // for (int i = 1; (inp != "done" || i == 2) && inp != "" && (!isNoneValid || (i == 1 && isNoneValid && inp.ToLower() !="none")); i++) {
    for (int i = 1; !(inp.ToLower() == "done" && i > 2 || (inp.ToLower() == "none" && isNoneValid && i == 2) || inp == ""); i++) {
        Console.Write($"{prompt} (#{i})\n\nEnter to cancel");

        if(i > 1) {
            Console.Write("\n\nType \"done\" to finish");
        }

        // if the isNoneValid is true, it means the user can type "none" to quit out of this input without canceling
        // should only let the user do this on the first input
        if(isNoneValid && i == 1)
            Console.Write($"\n\nType \"none\" if there are no values for {field}");


        inp = getInput("\n\n> ");

        // if input is NOT "done" OR it is the first input
        if(i == 1 || inp.ToLower() != "done") {

            // add input to the list of watchers
            wat.Add(inp);
        }

        // if the input is valid to be prompted
        if(!((i == 1 && inp == "none" && isNoneValid) || (inp == "done" && i > 1) || inp == "")) {

            // log added watcher
            info($"Added \"{inp}\" to {field}");
        }
    }

    return wat;
}

// get user's input for a cost field while adding a ticket
double getCost() {
    string inp;
    double cost = double.NaN;

    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n-Enter a cost estimate for this enhancement (in dollars and cents)\n\nEnter to cancel\n\n> $");

        // if the input is NOT empty
        if(inp != "") {

            // then check if the input can NOT be parsed into a double
            if(!Double.TryParse(inp, out cost)) {
                warn("Please enter a valid input!");

                // if parse fails, set cost to NaN so that the loop runs again
                cost = double.NaN;
            
            // if parse DID NOT fail,
            } else {

                // then check if the value is negative to warn the user
                if(cost < 0) {
                    warn("Please enter a positive value!");

                }
            }
        }
    
    // loop will repeat until the input is "" (user hit enter), OR the user inputted a valid cost
    } while (inp != "" 
                && !(cost >= 0)
            )
    ;

    return cost;
}

// get user's input for an estimate field while adding a ticket
TimeSpan getEstimatedTimeSpan() {
    string inp;
    int val = -1;

    int years = 0;
    int months = 0;
    int days = 0;

    // this needs to loop 3 times (one for years, one for months, one for days)
    for(int i = 0; i < 3; i++) {

        // string that holds "years", "months", or "days" for user's prompt
        string ymd;
        switch (i) {
            case 0:
                ymd = "Years";
                break;
            
            case 1:
                ymd = "Months";
                break;
            
            default:
                ymd = "Days";
                break;
        }

        // loop that gets user's input for TimeSpan values
        do {
            inp = getInput($"\n\nADD TICKET:\n-------------\n\n-Enter estimated time span to complete this ticket ({ymd}):\n\nEnter to cancel\n\n> ");

            if(inp != "") {
            
                // if input parse is SUCCESSFUL
                // sets val to the parsed int value
                if(int.TryParse(inp, out val)) {

                    // check if input is NOT a valid value
                    if(!((val >= 0 && ymd != "Days") || (val > 0 && ymd == "Days" && years == 0 && months == 0) || (val >= 0 && ymd == "Days" && !(years == 0 && months == 0)))) {
                    
                        // warns the user of their mistake using the correct values
                        if(ymd == "Days" && val == 0 && years == 0 && months == 0) {
                            warn($"{ymd} input must be more than 0!");

                        } else {
                            warn($"{ymd} input must be positive!");

                        }
                    }
                
                // if the parse fails
                } else {
                    warn("Please enter a whole number!");

                    // set val to -1 so that the loop repeats
                    val = -1;
                }

            // if input is "" (user hit enter)
            } else {

                // return TimeSpan of 0 ticks
                return new TimeSpan(0);
            }

        // loop repeats until the user inputs a valid value
        } while (!((val >= 0 && ymd != "Days") || (val > 0 && ymd == "Days" && years == 0 && months == 0) || (val >= 0 && ymd == "Days" && !(years == 0 && months == 0))));

        // sets the correct variable to the user's input
        switch(i) {
            case 0:
                years = int.Parse(inp);
                break;
            case 1:
                months = int.Parse(inp);
                break;
            default:
                days = int.Parse(inp);
                break;
        }

        info($"{ymd} updated: {val}");
    }

    return new TimeSpan((years * 365) + (months * 28) + days, 0, 0, 0);
}

// gets user's input for a project name field while adding a ticket
string getProjectName() {
    string inp;
    do {
        inp = getInput("\n\n ADD TICKET:\n-------------\n\n-Enter a name for this task:\n\nEnter to cancel\n\n> ");

        if(string.IsNullOrWhiteSpace(inp)) {
            warn("Enter a valid name for this task!");
        }

    } while (!
        (inp == "" 
            || !string.IsNullOrWhiteSpace(inp)
        )
    );

    return inp;
}

// gets user's input for a due date field while adding a ticket
DateTime getDueDate() {
    string inp;
    int val = -1;

    int day = 0;
    int month = 0;
    int year = 0;

    string inf = "";

    // loops 3 times, once for month, then day, then year inputs
    for (int i = 1; i < 4; i++) {

        // holds "month", "year" or "day" for prompt
        string ymd;

        // holds lower bounds value, non-inclusive
        int low;

        // holds upper bounds value, non-inclusive
        int high;

        // gets year/month/date input and low and high values depending on iteration of loop
        switch(i) {
            case 1:
                ymd = "month";
                low = 0;
                high = 13;

                break;

            case 2:
                ymd = "year";
                
                if(DateTime.Now.Month <= month)
                    low = DateTime.Now.Year - 1;
                else {
                    low = DateTime.Now.Year;
                }

                // max year value of DateTime class is 9999
                high = 10000;

                break;

            default:
                ymd = "day";
                
                // first checks if due date is within the current month
                if(year == DateTime.Now.Year && month == DateTime.Now.Month) {

                    // if it is, that means the earlest that the task can be completed is tomorrow
                    low = DateTime.Now.Day;

                // otherwise, lowest day is the first of the month
                } else {
                    low = 0;
                }

                // checks if month is NOT february right away since that has leap years
                if(month != 2) {

                    // april, june, september and november all have 30 days always
                    if(month == 4 || month == 6 || month == 9 || month == 11) {
                        high = 31;

                    // if it is not those months, or february, then the month has 31 days
                    } else {
                        high = 32;
                    }
                
                // if it IS february:
                } else {

                    // then checks for leap year
                    if(DateTime.IsLeapYear(year)) {

                        //leap year february has 29 days
                        high = 30;

                    // if not a leap year, then it must have 28 days
                    } else {
                        high = 29;
                    }
                }

                break;
        }

        // loop to get input from user
        do {
            inp = getInput($"\n\n ADD TICKET:\n-------------\n\n-In which {ymd} is this ticket due? ({low + 1}-{high - 1})\n\nEnter to cancel\n\n> ");

            // make sure string is NOT "" (user hit enter)
            if (inp != "") {

                // check if the user entered a number
                if(int.TryParse(inp, out val)) {

                    // check if the inputted value is NOT within the bounds of the input requirement to warn user
                    if (!(val < high && val > low)) {
                        warn($"Enter a value between {low + 1} and {high - 1} for {ymd}!");
                    }

                // if they did not, then warn the user
                } else {
                    warn("Please enter a whole number!");

                    // set value to negative so that the loop is sure to repeat
                    val = -1;
                }
            
            // if input value IS "" (user hit enter), return DateTime(0);
            } else {
                return new DateTime(0);
            }

            // set value to the variable corresponding to the iteration of the loop
            inf = $"{val}";
            switch(i) {
                case 1:
                    month = val;
                    inf = $"{new DateTime(1, month, 1):MMMM}";

                    break;

                case 2:
                    year = val;

                    break;

                default:
                    day = val;

                    break;
            }
            
        } while (!
                    (val < high 
                        && val > low
                    )
            
        );

        info($"Added {ymd} to due date: \"{inf}\"");
    }

    return new DateTime(year, month, day);
}

// ----- Test for Canceled methods ----- //

// checks if user hit enter to cancel

// for strings
bool isStringCanceled(string input) {
    if(input == ""){
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// for List<string>s
bool isStringListCanceled(List<string> input) {
    if(input[0] == "") {
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// for doubles
bool isDoubleCanceled(double input) {
    if (double.IsNaN(input)) {
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// for Levels
bool isLevelCanceled(Level input) {
    if(input == Level.None) {
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// for TimeSpans
bool isTimeSpanCanceled(TimeSpan input) {
    if(input == new TimeSpan(0)) {
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// for DateTimes
bool isDateTimeCanceled(DateTime input) {
    if(input == new DateTime(0)) {
        warn("Cancelling...");

        // returns out of the function if user hit enter.
        return true;
    } else {
        return false;
    }
}

// ----- MAIN FUNCTION ----- //

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
            inp = shorten(inp);

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
                info($"Selected: \"{sel}\"");
            } else {

                // if not, warn user and restart loop
                warn("Please enter a valid option!");
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
        info($"Added summary to ticket: \"{smr}\"");
    }

    // gets status for ticket
    string sts = getStatus();

    // checks if user hit enter to cancel
    if(isStringCanceled(sts)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added status
        info($"Added status to ticket: \"{sts}\"");
    }

    // gets priority level for ticket
    Level pri = getLevel("\n\n ADD TICKET:\n-------------\n\n-What priority level is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isLevelCanceled(pri)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added priority
        info($"Added priority level to ticket: \"{pri}\"");
    }

    // gets submitter for ticket
    string sbm = getInput("\n\n ADD TICKET:\n-------------\n\n-Who submitted this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(sbm)) {
        return false;
    } else {

        // if user DID NOT hit enter, log added submitter
        info($"Added submitter to ticket: \"{sbm}\"");
    }

    // gets assigned for ticket
    string assi = getInput("\n\n ADD TICKET:\n-------------\n\n-Who is assigned to this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(assi)) {
        return false;
    } else {

        // if user DID NOT hit enter, log assigned
        info($"Assigned \"{assi}\" to ticket");
    }

    // gets watching for ticket
    List<string> watc = getStringList($"\n\n ADD TICKET:\n-------------\n\n-Who is watching this ticket?", "Watching", false);

    // checks if user hit enter to cancel
    if(isStringListCanceled(watc)) {
        return false;
    } else {

        // if user DID NOT hit enter, log watchers
        info($"Added {watc.Count} watcher(s) to ticket");
    }

    // use original input ("1", "2" or "3") to decide if the user is creating a Bug ticket, an Enhancement ticket or a Task ticket
    switch(inp) {

        // bug ticket
        case "1":

            // gets severity for bug ticket
            Level sev = getLevel("\n\n ADD TICKET:\n-------------\n\n-How severe is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

            // checks if user hit enter to cancel
            if(isLevelCanceled(sev)) {
                return false;

            // if the ticket was NOT cancelled:
            } else {

                //log added severity
                info($"Added severity level to ticket: \"{sev}\"");
            }

            // create new bug ticket with the given inputs
            BugTicket bgTkt = new BugTicket() {
                Summary = smr,
                Status = sts,
                Priority = pri,
                Submitter = sbm,
                Assigned = assi,
                Watching = watc,
                Severity = sev
            };

            // add bug ticket to the list in the associated object
            bugTicketFile.AddToList(bgTkt);

            break;

        // enhancement ticket
        case "2":

            // gets software requirements field for enhancement ticket
            List<string> sftw = getStringList("\n\n ADD TICKET:\n-------------\n\n-Enter a software requirement for this ticket", "Software", true);

            // checks if user hit enter to cancel
            if(isStringListCanceled(sftw)) {
                return false;
            } else {
                if(sftw[0] != "none"){

                    // if user DID NOT hit enter AND they didn't input "none", log software
                    info($"Added {sftw.Count} software requirements to ticket");
                } else {

                    // if user DID input "none"
                    info("No software requirements were added to ticket");
                }
            }

            // gets cost field for enhancement ticket
            // uses string interpolation formatting to always get a number with two values after the decimal point

            /* this does not stop the variable from trimming extra zeroes at the end of the number, 
            but it DOES stop the variable from storing a double with more than two decimal places, which makes sense for cost */
            double cst = Double.Parse($"{getCost():#0.00}"); 

            // checks if user hit enter to cancel
            if(isDoubleCanceled(cst)) {
                return false;
            } else {
                
                // if user DID NOT hit enter, log cost
                info($"Added cost to ticket: \"{cst:#0.00}\"");
            }

            // gets reason field for enhancement ticket
            string rsn = getInput("\n\nADD TICKET:\n-------------\n\n-What is the reason for this ticket?\n\nEnter to cancel\n\n> ");
            
            // checks if user hit enter to cancel
            if(isStringCanceled(rsn)) {
                return false;
            } else {
                
                // if user DID NOT hit enter, log reason
                info($"Added reason to ticket: \"{rsn}\"");
            }

            TimeSpan est = getEstimatedTimeSpan();

            if(isTimeSpanCanceled(est)) {
                return false;
            } else {

                // if user DID NOT hit enter, log TimeSpan estimate
                Console.WriteLine(est);

                // # of days in estimate TimeSpan
                int estDys = int.Parse(est.ToString().Split(".").First());

                // # of years is ( days - (days % 365) ) / 365
                int estYrs = (estDys - estDys % 365) / 365;

                // # of remaining days is days - (years * 365) [subtracts the amount of days in that many years]
                estDys = estDys - (estYrs * 365);

                // # of months is ( remaining days - (remaining days % 28) ) / 28
                int estMts = (estDys - estDys % 28) / 28;

                // # of remaining days is days - (months * 28) [subtracts the amount of days in that many months]
                estDys = estDys - (estMts * 28);

                logger.Info($"Added estimated time span to ticket: \"{estYrs} years, {estMts} months, {estDys} days.\"");
            }

            // create new enhancement ticket with the given inputs
            EnhancementTicket enTkt = new EnhancementTicket() {
                Summary = smr,
                Status = sts,
                Priority = pri,
                Submitter = sbm,
                Assigned = assi,
                Watching = watc,
                Software = sftw,
                Cost = cst,
                Reason = rsn,
                Estimate = est
            };

            // add bug ticket to the list in the associated object
            enhancementTicketFile.AddToList(enTkt);

            break;

        // only other option is a task ticket (inp =/= "" here)
        default:
            
            // gets project name field for task ticket
            string pjn = getProjectName();

            // checks if user hit enter to cancel
            if(isStringCanceled(pjn)) {
                return false;
            } else {

                // if user DID NOT hit enter, log project name
                info($"Added project name to ticket: \"{pjn}\"");
            }

            // gets due date field for task ticket
            DateTime duda = getDueDate();

            // checks if user hit enter to cancel
            if(isDateTimeCanceled(duda)) {
                return false;
            } else {

                // if user DID NOT hit enter, log cost
                Console.WriteLine();
                logger.Info($"Added due date to ticket: \"{duda:MMMM dd, yyyy}\"");
            }

            // create new task ticket with the given inputs
            TaskTicket tskTkt = new TaskTicket() {
                Summary = smr,
                Status = sts,
                Priority = pri,
                Submitter = sbm,
                Assigned = assi,
                Watching = watc,
                ProjectName = pjn,
                DueDate = duda
            };

            // add task ticket to the list in the associated object
            taskTicketFile.AddToList(tskTkt);

            break;
    }

    return true;
}