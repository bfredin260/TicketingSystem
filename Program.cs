using NLog;

// ==================== VARIABLES ==================== //

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

// TicketFile objects
BugTicketFile bugTicketFile = new();
EnhancementTicketFile enhancementTicketFile = new();
TaskTicketFile taskTicketFile = new();

// TempTickets.csv to store temporary data
File.WriteAllText("TempTickets.csv", "");



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
            viewTickets();

            break;
        
        // load tickets for "3"
        case "3":
            loadTickets();

            break;

        // save tickets for "4"
        case "4":
            saveTickets();

            break;

        // exit loop when user chooses "" (press enter)
        case "":
            info("Closing program...");

            break;
    }

// loop repeats until user hits enter to exit the choice menu
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


// - just to shorten code a little bit -

// just a long line to divide each prompt
void line() {
    Console.Clear();
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
        inp = getInput("\n\n SELECT:\n---------\n\n1) Add Ticket\n2) View Ticket(s)\n3) Load Ticket(s) from File\n4) Save Ticket(s) to File\n\nEnter to exit\n\n> ");
    
        // if the input is NOT empty:
        if(inp != "") {

            // shorten string to one character
            inp = shorten(inp);

            // if user chose one of the four options, output it using logger
            if(inp == "1" || inp == "2" || inp == "3" || inp == "4") {
                string sel = inp switch
                {
                    "1" => "Add Ticket",
                    "2" => "View Ticket(s)",
                    "3" => "Load Ticket(s) from File",
                    _ => "Save Ticket(s) to File",
                };
                info($"Selected: \"{sel}\"");

            } else {

                // if not, warn user and repeat the loop
                warn("Please enter a valid option!");
            }
        }

    // loop repeats until the user inputs a valid choice
    } while (
        inp != "1" 
        && inp != "2" 
        && inp != "3" 
        && inp != "4" 
        && inp != ""
    );

    return inp;
}


// ----- "Add Ticket" Choice -----//


// - getters for user input - //

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
    } while (
        inp != "Closed" 
        && inp != "Open" 
        && inp != ""
    );

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
    } while (
        inp != "h" 
        && inp != "m" 
        && inp != "l" 
        && inp != ""
    );

    return lev;
}

// get user's input(s) for a string list field while adding a ticket
List<string> getStringList(string prompt, string field, bool isNoneValid) {
    string inp = " ";

    List<string> wat = new List<string>();

    // same as the do-while loop, except it keeps track of the index (so I can number the watchers for the user)
    // conditional tests for all values that are valid so that it can continue the loop
    for (int i = 1; !(inp.ToLower() == "done" && i > 2 || (inp.ToLower() == "none" && isNoneValid && i == 2) || inp == ""); i++) {
        Console.Write($"{prompt} (#{i})\n\nEnter to cancel");

        // lets user know that they can type "done" to end the loop, as long as it is not the first input
        if(i > 1) {
            Console.Write("\n\nType \"done\" to finish");
        }

        // if the isNoneValid is true, it means the user can type "none" to quit out of this input without canceling
        // should only let the user do this on the first input
        if(isNoneValid && i == 1)
            Console.Write($"\n\nType \"none\" if there are no values for {field}");


        // gets actual user input
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
    } while (
        inp != "" 

        // !(cost >= 0) instead of cost < 0, because the first condition makes sure that it loops when cost = double.NaN
        && !(cost >= 0)
    );

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
        string ymd = i switch
        {
            // first loop gets years
            0 => "Years",
            // second loop gets months
            1 => "Months",
            // final loop gets days
            _ => "Days",
        };

        // loop that gets user's input for TimeSpan values
        do {
            inp = getInput($"\n\nADD TICKET:\n-------------\n\n-Enter estimated time span to complete this ticket ({ymd}):\n\nEnter to cancel\n\n> ");

            // checks if user did NOT hit enter
            if(inp != "") {
            
                // if input parse is SUCCESSFUL
                // sets val to the parsed int value
                if(int.TryParse(inp, out val)) {

                    // check if input is NOT a valid value
                    // I know this is a very big condition, but basically:
                    // separates all VALID inputs with || (since any of them will be valid)
                    // surrounds every valid input with () and flips it with !
                    // this just says if the input is NOT valid, the condition is true (to move on and output the user's mistake)
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
        } while (
            !(
                (
                    val >= 0 
                    && ymd != "Days"
                ) 
                || (
                    val > 0 
                    && ymd == "Days" 
                    && years == 0 
                    && months == 0
                ) 
                || (
                    val >= 0 
                    && ymd == "Days" 
                    && !(
                        years == 0 
                        && months == 0
                    )
                )
            )
        );

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

        // warns user if their input is null or whitespace (basically if they hit enter or only have spaces)
        if(string.IsNullOrWhiteSpace(inp)) {
            warn("Enter a valid name for this task!");
        }

    // loop repeats until the input is valid
    } while (
        !(
            inp == ""
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

            // first loop is month
            case 1:
                ymd = "month";
                low = 0;
                high = 13;

                break;

            // second loop is year
            case 2:
                ymd = "year";
                
                // checks if current month is NOT after the inputted month
                if(DateTime.Now.Month <= month)

                    // if it is not that month yet, the lowest value the user can input is the current year
                    low = DateTime.Now.Year - 1;

                // if the current month IS after the inputted month, lowest value is NEXT year (due date cannot be in the past)
                else {
                    low = DateTime.Now.Year;
                }

                // max year value of DateTime class is 9999
                high = 10000;

                break;

            // final loop is day
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

                    // if the value IS within the bounds,
                    } else {

                        // set value to the variable corresponding to the iteration of the loop
                        inf = $"{val}";

                        switch(i) {

                            // updates month on first loop
                            case 1:
                                month = val;

                                // gets month name from the inputed month using formatted string
                                inf = $"{new DateTime(1, month, 1):MMMM}";

                                break;

                            // updates year on second loop
                            case 2:
                                year = val;

                                break;

                            // updates day on first loop
                            default:
                                day = val;

                                break;
                        }
    
                    }

                // if they did not, then warn the user
                } else {
                    warn("Please enter a whole number!");

                    // set value to negative so that the loop is sure to repeat
                    val = -1;
                }
            
            // if input value IS "" (user hit enter), return DateTime(0) (to detect that the user canceled)
            } else {
                return new DateTime(0);
            }
            
        // loop repeats until user enters a value within the high and low bounds
        } while (
            !(
                val < high 
                && val > low
            )
        );

        info($"Added {ymd} to due date: \"{inf}\"");
    }

    return new DateTime(year, month, day);
}


// - test for canceled methods - //
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


// - main function - //

// adds a new ticket to the list and temporary file
// asks user for inputs for ticket type and fields
void newTicket() {
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
                string sel = inp switch
                {
                    // first choice is Bug/Defect 
                    "1" => "Bug/Defect",
                    // second choice is Enhancement 
                    "2" => "Enhancement",
                    // third choice is Task 
                    _ => "Task",
                };
                info($"Selected: \"{sel}\"");
            
            } else {

                // if not, warn user and restart loop
                warn("Please enter a valid option!");
            }
        }
    
    // loop runs until user inputs "1", "2", "3", or "" (they pressed enter)
    } while (
        inp != "1" 
        && inp != "2" 
        && inp != "3" 
        && inp != ""
    );

    // checks if user hit enter to cancel
    if(isStringCanceled(inp)) {
        return;
    }
    
    // gets summary for ticket
    string smr = getInput("\n\n ADD TICKET:\n-------------\n\n-Please Enter Ticket Summary:\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(smr)) {
        return;
    } else {

        // if user DID NOT hit enter, log added summary
        info($"Added summary to ticket: \"{smr}\"");
    }

    // gets status for ticket
    string sts = getStatus();

    // checks if user hit enter to cancel
    if(isStringCanceled(sts)) {
        return;
    } else {

        // if user DID NOT hit enter, log added status
        info($"Added status to ticket: \"{sts}\"");
    }

    // gets priority level for ticket
    Level pri = getLevel("\n\n ADD TICKET:\n-------------\n\n-What priority level is this ticket?\n\nh) High\nm) Medium\nl) Low (lowercase L)\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isLevelCanceled(pri)) {
        return;
    } else {

        // if user DID NOT hit enter, log added priority
        info($"Added priority level to ticket: \"{pri}\"");
    }

    // gets submitter for ticket
    string sbm = getInput("\n\n ADD TICKET:\n-------------\n\n-Who submitted this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(sbm)) {
        return;
    } else {

        // if user DID NOT hit enter, log added submitter
        info($"Added submitter to ticket: \"{sbm}\"");
    }

    // gets assigned for ticket
    string assi = getInput("\n\n ADD TICKET:\n-------------\n\n-Who is assigned to this ticket?\n\nEnter to cancel\n\n> ");

    // checks if user hit enter to cancel
    if(isStringCanceled(assi)) {
        return;
    } else {

        // if user DID NOT hit enter, log assigned
        info($"Assigned \"{assi}\" to ticket");
    }

    // gets watching for ticket
    List<string> watc = getStringList($"\n\n ADD TICKET:\n-------------\n\n-Who is watching this ticket?", "Watching", false);

    // checks if user hit enter to cancel
    if(isStringListCanceled(watc)) {
        return;
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
                return;

            // if the ticket was NOT cancelled:
            } else {

                //log added severity
                info($"Added severity level to ticket: \"{sev}\"");
            }

            // create new bug ticket with the given inputs
            BugTicket bgTkt = new() {
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

            // add bugTicket to the temp file
            TicketFile.AddToTemp(bgTkt.ToString());

            break;

        // enhancement ticket
        case "2":

            // gets software requirements field for enhancement ticket
            List<string> sftw = getStringList("\n\n ADD TICKET:\n-------------\n\n-Enter a software requirement for this ticket", "Software", true);

            // checks if user hit enter to cancel
            if(isStringListCanceled(sftw)) {
                return;

            } else {
                // if user DID NOT hit enter AND they didn't input "none"
                if(sftw[0] != "none"){

                    // log software
                    info($"Added {sftw.Count} software requirements to ticket");

                // if user DID input "none"
                } else {

                    // log that none were added
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
                return;
            } else {
                
                // if user DID NOT hit enter, log cost
                info($"Added cost to ticket: \"{cst:#0.00}\"");
            }

            // gets reason field for enhancement ticket
            string rsn = getInput("\n\nADD TICKET:\n-------------\n\n-What is the reason for this ticket?\n\nEnter to cancel\n\n> ");
            
            // checks if user hit enter to cancel
            if(isStringCanceled(rsn)) {
                return;
            } else {
                
                // if user DID NOT hit enter, log reason
                info($"Added reason to ticket: \"{rsn}\"");
            }

            // gets estimate field for enhancement ticket
            TimeSpan est = getEstimatedTimeSpan();

            // checks if user hit enter to cancel
            if(isTimeSpanCanceled(est)) {
                return;

            // if user did NOT hit enter, log timespan estimate
            } else {

                // # of days in estimate TimeSpan
                int estDys = int.Parse(est.ToString().Split(".").First());

                // # of years is ( days - (days % 365) ) / 365
                int estYrs = (estDys - estDys % 365) / 365;

                // # of remaining days is days - (years * 365) [subtracts the amount of days in that many years]
                estDys -= estYrs * 365;

                // # of months is ( remaining days - (remaining days % 28) ) / 28
                int estMts = (estDys - estDys % 28) / 28;

                // # of remaining days is days - (months * 28) [subtracts the amount of days in that many months]
                estDys -= estMts * 28;

                Console.WriteLine();
                logger.Info($"Added estimated time span to ticket: \"{estYrs} years, {estMts} months, {estDys} days.\"");
            }

            // create new enhancement ticket with the given inputs
            EnhancementTicket enTkt = new() {
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

            // add enhancement ticket to the list in the associated object
            enhancementTicketFile.AddToList(enTkt);

            // add enhancementTicket to the temp file
            TicketFile.AddToTemp(enTkt.ToString());

            break;

        // only other option is a task ticket (inp =/= "" here)
        default:
            
            // gets project name field for task ticket
            string pjn = getProjectName();

            // checks if user hit enter to cancel
            if(isStringCanceled(pjn)) {
                return;
            } else {

                // if user DID NOT hit enter, log project name
                info($"Added project name to ticket: \"{pjn}\"");
            }

            // gets due date field for task ticket
            DateTime duda = getDueDate();

            // checks if user hit enter to cancel
            if(isDateTimeCanceled(duda)) {
                return;
            } else {

                // if user DID NOT hit enter, log cost
                Console.WriteLine();
                logger.Info($"Added due date to ticket: \"{duda:MMMM dd, yyyy}\"");
            }

            // create new task ticket with the given inputs
            TaskTicket tskTkt = new() {
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

            // add taskTicket to the temp file
            TicketFile.AddToTemp(tskTkt.ToString());

            break;
    }
}


// ----- "View Ticket(s)" option ----- //


// - main function - //

// prompts user for which tickets they would like to view, displays each ticket with information.
void viewTickets() {
    string inp;

    // bools so that the program knows which ticket(s) to view using user's inputs
    bool bug = false;
    bool enh = false;
    bool tsk = false;

    // loop determines which type of ticket to view, using user input with validation
    do { 
        inp = getInput("\n\n VIEW TICKET(S):\n-----------------\n\n-Which tickets would you like to view?\n\n1) Bug/Defect\n2) Enhancement\n3) Task\n4) View All\n\nEnter to cancel\n\n> ");

        // if the input is NOT empty (the user did NOT hit enter)
        if(inp != "") {

            // convert input to lowercase, then first character only, then back to string
            // ("Tree" becomes "t")
            inp = shorten(inp);

            // if input is one of the four options, output it using logger AND set corresponding bool(s) to true
            if(inp == "1" || inp == "2" || inp == "3" || inp == "4") {

                string sel;

                switch(inp) {

                    // first choice is Bug/Defect
                    case "1":
                        sel = "Bug/Defect";

                        bug = true;
                    
                        break;
                    
                    // second choice is Enhancement
                    case "2":
                        sel = "Enhancement";

                        enh = true;
                    
                        break;
                    
                    // third choice is Task
                    case "3":
                        sel = "Task";

                        tsk = true;
                    
                        break;
                    
                    // fourth choice is View All
                    default:
                        sel = "View All";

                        bug = true;
                        enh = true;
                        tsk = true;

                        break;
                }
                
                info($"Selected: \"{sel}\"");
            
            } else {

                // if not, warn user and restart loop
                warn("Please enter a valid option!");
            }

            // header for option selection
            Console.Write("\n\n VIEW TICKET(S):\n-----------------");

            // if user wants to view Bug/Defect Tickets
            if(bug) {
                Console.Write("\n\n Bug/Defect:\n-------------");

                // if there are ANY bug tickets in the local list:
                if(bugTicketFile.Tickets.Count > 0) {

                    // loop over every ticket and output it for the user
                    for(int i = 0; i < bugTicketFile.Tickets.Count; i++) {
                        Console.WriteLine($"\nTicket #{i + 1}:\n    TicketID: {bugTicketFile.Tickets[i].TicketID}\n    Summary: \"{bugTicketFile.Tickets[i].Summary}\"\n    Status: \"{bugTicketFile.Tickets[i].Status}\"\n    Priority level: \"{bugTicketFile.Tickets[i].Priority}\"\n    Submitter: \"{bugTicketFile.Tickets[i].Submitter}\"\n    Assigned to: \"{bugTicketFile.Tickets[i].Assigned}\"\n    Watching: \"{string.Join("\", \"", bugTicketFile.Tickets[i].Watching)}\"\n    Severity level: \"{bugTicketFile.Tickets[i].Severity}\"");
                    }

                // if there are NO bug tickets in the local list, display that
                } else {
                    Console.WriteLine("\nNo Bug/Defect tickets found");
                }
            } 

            // if user wants to view Enhancement Tickets
            if(enh) {
                Console.Write("\n\n Enhancement:\n--------------");

                // if there are ANY enhancement tickets in the local list:
                if(enhancementTicketFile.Tickets.Count > 0) {

                    // loop over every ticket and output it for the user
                    for(int i = 0; i < enhancementTicketFile.Tickets.Count; i++) {
                        Console.WriteLine($"\nTicket #{i + 1}:    TicketID: {enhancementTicketFile.Tickets[i].TicketID}\n    Summary: \"{enhancementTicketFile.Tickets[i].Summary}\"\n    Status: \"{enhancementTicketFile.Tickets[i].Status}\"\n    Priority Level: \"{enhancementTicketFile.Tickets[i].Priority}\"\n    Submitter: \"{enhancementTicketFile.Tickets[i].Submitter}\"\n    Assigned to: \"{enhancementTicketFile.Tickets[i].Assigned}\"\n    Watching: \"{string.Join("\", \"", enhancementTicketFile.Tickets[i].Watching)}\"\n    Software requirements: \"{string.Join("\", \"", enhancementTicketFile.Tickets[i].Software)}\"\n    Estimated cost: \"{enhancementTicketFile.Tickets[i].Cost}\"\n    Reason: \"{enhancementTicketFile.Tickets[i].Reason}\"\n    Estimated time to completion: \"{enhancementTicketFile.Tickets[i].Estimate}\"");
                    }

                // if there are NO enhancemnet tickets in the local list, display that
                } else {
                    Console.WriteLine("\nNo Enhancement tickets found");
                }
            }

            // if user wants to view Task Tickets
            if(tsk) {
                Console.Write("\n\n Task:\n-------");

                // if there are ANY task tickets in the local list:
                if(taskTicketFile.Tickets.Count > 0) {

                    // loop over every ticket and output it for the user
                    for(int i = 0; i < taskTicketFile.Tickets.Count; i++) {
                        Console.WriteLine($"\nTicket #{i + 1}:\n    TicketID: {taskTicketFile.Tickets[i].TicketID}\n    Summary: \"{taskTicketFile.Tickets[i].Summary}\"\n    Status: \"{taskTicketFile.Tickets[i].Status}\"\n    Priority Level: \"{taskTicketFile.Tickets[i].Priority}\"\n    Submitter: \"{taskTicketFile.Tickets[i].Submitter}\"\n    Assigned to: \"{taskTicketFile.Tickets[i].Assigned}\"\n    Watching: \"{string.Join("\", \"", taskTicketFile.Tickets[i].Watching)}\"\n    Project name: \"{taskTicketFile.Tickets[i].ProjectName}\"\n    Due date: \"{taskTicketFile.Tickets[i].DueDate:MMMM dd, yyyy}\"");
                    }

                // if there are NO task tickets in the local list, display that
                } else {
                    Console.WriteLine("\nNo Task tickets found");
                }
            }
        }
    
    // loop runs until user inputs "1", "2", "3", "4 or "" (they pressed enter)
    } while (
        inp != "1" 
        && inp != "2" 
        && inp != "3"
        && inp != "4"
        && inp != ""
    );

}


// ----- "Load Ticket(s) from File" option ----- //


// - converters from string to class - //

// gets priority level from input string since I don't know how to Parse an enum
Level getLevelFromString(string input) {
    return input switch
    {
        "High" => Level.High,
        "Medium" => Level.Medium,
        _ => Level.Low,
    };
}

// gets Date from yyyy/MM/dd string format
DateTime getDateFromStringFormat(string input) {

    // splits date into array where date[0] = yyyy, date[1] = MM, date[2] = dd
    string[] date = input
        .Split("/")
    ;

    // parse all string values into ints and return a new DateTime object using those values
    return new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]));
}


// - main function - //

// fills each Ticket type's list with the corresponding objects in the csv file
void loadTickets() {

    string choice = getInput("\n\n LOAD TICKET(S):\n-----------------\n\n-!!This will overwrite any other unsaved Tickets!!\n\n-Press \"y\" to confirm\n\n> ");

    // if user does NOT hit "y", cancel "load"
    if(choice.Length == 0 || shorten(choice) != "y") {
        info("\"Load\" canceled");
        return;

    // if they DID hit "y", continue "load"
    } else {
        info("Loading from file...");
    }

    // writes to Temp file
    StreamWriter sw = new("TempTickets.csv");

    // reads from each ticket type's save file
    StreamReader bug = new("Tickets.csv");
    StreamReader enh = new("Enhancements.csv");
    StreamReader tsk = new("Tasks.csv");

    // check if all ticket save files are empty
    if(File.ReadLines("Tickets.csv").Count() < 2 && File.ReadLines("Enhancements.csv").Count() < 2 && File.ReadLines("Tasks.csv").Count() < 2) {

            // if they all are empty, warn user
            Console.WriteLine();
            logger.Warn("There are no tickets to load! Please save tickets to file before loading!");

    // if AT LEAST ONE save file has a ticket in it, begin creating tickets using the data
    } else {

        // - bug file - //

        // eats the first line of the file, since it is just headers
        bug.ReadLine();

        // loops over every line in the bug file (each line should be one ticket)
        while (!bug.EndOfStream) {

            // saves line to an array, split by the column seperator
            string[] bugLine = bug
                .ReadLine()
                .Split("|")
            ;
            
            // create new ticket from line
            BugTicket bugTicket = new() {
                TicketID = int.Parse(bugLine[0]),
                Summary = bugLine[1],
                Status = bugLine[2],
                Priority = getLevelFromString(bugLine[3]),
                Submitter = bugLine[4],
                Assigned = bugLine[5],

                // watching field changed back into list, since it had to be saved as a string
                Watching = bugLine[6]
                    .Split("~")
                    .ToList()
                ,

                Severity = getLevelFromString(bugLine[7])
            };

            // add ticket to corresponding list
            bugTicketFile.AddToList(bugTicket);

            // add ticket to temp file
            TicketFile.AddToTemp(bugTicket.ToString());
        }

        // - enhancement file - //

        // eats the first line of the file, since it is just headers
        enh.ReadLine();

        // loops over every line in the enhancement file (each line should be one ticket)
        while (!enh.EndOfStream) {

            // saves line to an array, split by the column seperator
            string[] enhLine = enh
                .ReadLine()
                .Split("|")
            ;
            
            // create new ticket from line
            EnhancementTicket enhTicket = new() {
                TicketID = int.Parse(enhLine[0]),
                Summary = enhLine[1],
                Status = enhLine[2],
                Priority = getLevelFromString(enhLine[3]),
                Submitter = enhLine[4],
                Assigned = enhLine[5],

                // watching field changed back into list, since it had to be saved as a string
                Watching = enhLine[6]
                    .Split("~")
                    .ToList()
                ,

                // software field changed back into list, since it had to be saved as a string
                Software = enhLine[7]
                    .Split("~")
                    .ToList()
                ,

                Cost = double.Parse(enhLine[8]),
                Reason = enhLine[9],

                Estimate = new TimeSpan(
                    int.Parse(enhLine[10]), 
                    0, 
                    0, 
                    0
                )
            };

            // add ticket to corresponding list
            enhancementTicketFile.AddToList(enhTicket);

            // add ticket to temp file
            TicketFile.AddToTemp(enhTicket.ToString());
        }

        // - task file - //

        // eats the first line of the file, since it is just headers
        tsk.ReadLine();
        
        // loops over every line in the bug file (each line should be one ticket)
        while (!tsk.EndOfStream) {

            // saves line to an array, split by the column seperator
            string[] tskLine = tsk
                .ReadLine()
                .Split("|")
            ;
            
            // create new ticket from line
            TaskTicket tskTicket = new() {
                TicketID = int.Parse(tskLine[0]),
                Summary = tskLine[1],
                Status = tskLine[2],
                Priority = getLevelFromString(tskLine[3]),
                Submitter = tskLine[4],
                Assigned = tskLine[5],

                // watching field changed back into list, since it had to be saved as a string
                Watching = tskLine[6]
                    .Split("~")
                    .ToList()
                ,

                ProjectName = tskLine[7],
                DueDate = getDateFromStringFormat(tskLine[8])
            };

            // adds ticket to corresponding list
            taskTicketFile.AddToList(tskTicket);

            // add ticket to temp file
            TicketFile.AddToTemp(tskTicket.ToString());
        }

        // -- this section just sorts the TempTickets.csv file by TicketID so that they are in order
        // this is important because new Tickets' IDs are determined through adding 1 to the final Ticket's ID in this file.
        string[] tempFile = File.ReadAllLines("TempTickets.csv");
        Array.Sort(tempFile);

        File.WriteAllLines("TempTickets.csv", tempFile);
        // --

        // log success to user
        Console.WriteLine();
        logger.Info($"Successfully loaded {File.ReadLines("TempTickets.csv").Count()} tickets from file!");
    }

    // flush/close all streams
    sw.Flush();
    sw.Close();

    bug.Close();

    enh.Close();

    tsk.Close();
}


// ----- "Save Ticket(s) to File" option ----- //


// - write line to file - //

// writes the given line to a file using the given stream writer
void writeLineToFile(StreamWriter sw, string[] line) {

    // loops over each column in the line (each column is a different field)
    for(int i = 0; i < line.Length; i++) {

        // determines if there needs to be a separator after the field
        // when i == 1, that is only there to show what type of ticket that is earlier
        // since that has already been determined, that is garbage at this point in the process
        if(i != 1 && i != line.Length - 1) {
            sw.Write($"{line[i]}|");

        } else if(i == line.Length - 1) {
            sw.WriteLine(line[i]);
        }
    }
}


// - main function - //

// writes each object to a line in the csv file corresponding to the FileType
void saveTickets() {

    string choice = getInput("\n\n SAVE TICKET(S):\n-----------------\n\n-!!This will overwrite any other saved Tickets!!\n\n-Press \"y\" to confirm\n\n> ");

    // if user does NOT hit "y", cancel "save"
    if(choice.Length == 0 || shorten(choice) != "y") {
        info("\"Save\" canceled");

        return;

    // if user DID hit "y", continue "save"
    } else {
        info("Saving to file...");
    }

    // reads temp file
    StreamReader sr = new("TempTickets.csv");

    // writes to each ticket's save file
    StreamWriter bug = new("Tickets.csv");
    StreamWriter enh = new("Enhancements.csv");
    StreamWriter tsk = new("Tasks.csv");

    // clear all files in order to save new tickets over them
    File.WriteAllText("Tickets.csv", "");
    File.WriteAllText("Enhancements.csv", "");
    File.WriteAllText("Tasks.csv", "");
    
    // set first line to headers in csv files.
    bug.WriteLine("TicketID|Summary|Status|Priority level|Submitter|Assigned to|Watching|Severity level");
    enh.WriteLine("TicketID|Summary|Status|Priority level|Submitter|Assigned to|Watching|Software|Cost|Reason|Estimated time to completion");
    tsk.WriteLine("TicketID|Summary|Status|Priority level|Submitter|Assigned to|Watching|Project name|Due date");

    // loop repeats until the last line of the temp file is reached
    while(!sr.EndOfStream) {

        // saves line to an array, split by the column seperator
        string[] line = sr
            .ReadLine()
            .Split("|")
        ;

        // line[1] holds what type the ticket is, for this step only
        switch(line[1]) {

            case "bug":
                writeLineToFile(bug, line);

                break;

            case "enhancement":
                writeLineToFile(enh, line);

                break;

            default:
                writeLineToFile(tsk, line);

                break;
        }
    }

    // flush/close all streams
    bug.Flush();
    bug.Close();

    enh.Flush();
    enh.Close();

    tsk.Flush();
    tsk.Close();

    sr.Close();

    // log success
    Console.WriteLine();
    logger.Info($"Successfully saved {File.ReadLines("TempTickets.csv").Count()} tickets to file!");
}