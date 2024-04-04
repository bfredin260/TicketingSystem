public abstract class Ticket {
    public int TicketID = 1;
    public string Summary { get; set; }
    public string Status { get; set; }
    public Level Priority { get; set; }
    public string Submitter { get; set; }
    public string Assigned { get; set; }
    public List<string> Watching { get; set; }

    // constructor
    public Ticket() {
        StreamReader sr = new("TempTickets.csv");

        // default value of 0 so that the first ticketID is 1
        string line = "0";

        // loop until the last line of the temp file
        while (!sr.EndOfStream) {

            // set line to that line
            // might be a little weird to set this over and over, but once the loop ends it will be set to the final line
            line = sr.ReadLine();
        }

        // set ticket ID to 1 over the highest ticket ID (incrementing)
        TicketID = int.Parse(line.Split("|")[0]) + 1;

        // close stream
        sr.Close();
    }

    public abstract string displayTicket();
}


public class BugTicket : Ticket {
    public Level Severity { get; set; }

    public override string ToString() {
        return $"{TicketID}|bug|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{Severity}";
    }

    public override string displayTicket() {
        return $"\nTicket #{this.TicketID}:\n    TicketID: {this.TicketID}\n    Summary: \"{this.Summary}\"\n    Status: \"{this.Status}\"\n    Priority level: \"{this.Priority}\"\n    Submitter: \"{this.Submitter}\"\n    Assigned to: \"{this.Assigned}\"\n    Watching: \"{string.Join("\", \"", this.Watching)}\"\n    Severity level: \"{this.Severity}\"";
    }
}

public class EnhancementTicket : Ticket {
    public List<string> Software { get; set; }
    public double Cost { get; set; }
    public string Reason { get; set; }

    // estimated amount of time in DAYS for the ticket to be completed
    public TimeSpan Estimate { get; set; }

    public override string ToString() {
        return $"{TicketID}|enhancement|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{string.Join("~", Software)}|{Cost}|{Reason}|{Estimate.Days}";
    }

    public override string displayTicket() {
        return $"\nTicket #{this.TicketID}:\n    TicketID: {this.TicketID}\n    Summary: \"{this.Summary}\"\n    Status: \"{this.Status}\"\n    Priority Level: \"{this.Priority}\"\n    Submitter: \"{this.Submitter}\"\n    Assigned to: \"{this.Assigned}\"\n    Watching: \"{string.Join("\", \"", this.Watching)}\"\n    Software requirements: \"{string.Join("\", \"", this.Software)}\"\n    Estimated cost: \"{this.Cost}\"\n    Reason: \"{this.Reason}\"\n    Estimated time to completion: \"{this.Estimate.Days} Days\"";
    }
}

public class TaskTicket : Ticket {
    public string ProjectName { get; set; }
    public DateTime DueDate { get; set; }

    public override string ToString() {
        return $"{TicketID}|task|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{ProjectName}|{DueDate:yyyy/MM/dd}";
    }

    public override string displayTicket() {
        return $"\nTicket #{this.TicketID}:\n    TicketID: {this.TicketID}\n    Summary: \"{this.Summary}\"\n    Status: \"{this.Status}\"\n    Priority Level: \"{this.Priority}\"\n    Submitter: \"{this.Submitter}\"\n    Assigned to: \"{this.Assigned}\"\n    Watching: \"{string.Join("\", \"", this.Watching)}\"\n    Project name: \"{this.ProjectName}\"\n    Due date: \"{this.DueDate:MMMM dd, yyyy}\"";
    }
}