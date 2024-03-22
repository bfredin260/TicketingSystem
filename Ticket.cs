public abstract class Ticket {
    public string Summary { get; set; }
    public string Status { get; set; }
    public Level Priority { get; set; }
    public string Submitter { get; set; }
    public string Assigned { get; set; }
    public List<string> Watching { get; set; }
}

public class BugTicket : Ticket {
    public Level Severity { get; set; }

    public override string ToString() {
        return $"bug|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{Severity}";
    }
}

public class EnhancementTicket : Ticket {
    public List<string> Software { get; set; }
    public double Cost { get; set; }
    public string Reason { get; set; }

    // estimated amount of time in DAYS for the ticket to be completed
    public TimeSpan Estimate { get; set; }

    public override string ToString()
    {
        return $"enhancement|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{string.Join("~", Software)}|{Cost}|{Reason}|{Estimate.Days}";
    }
}

public class TaskTicket : Ticket {
    public string ProjectName { get; set; }
    public DateTime DueDate { get; set; }

    public override string ToString()
    {
        return $"bug|{Summary}|{Status}|{Priority}|{Submitter}|{Assigned}|{string.Join("~", Watching)}|{ProjectName}|{DueDate}";
    }
}