public abstract class Ticket {
    public string Summary { get; set; }
    public string Status { get; set; }
    public Level Priority { get; set; }
    public string Submitter { get; set; }
    public string Assigned { get; set; }
    public List<string> Watching { get; set; }

    public Ticket() {
        Watching = new List<string>();
    }

    public abstract string FileFormat();
}

public class BugTicket : Ticket {
    public Level Severity { get; set; }

    public override string FileFormat() {
        return $"";
    }
}

public class EnhancementTicket : Ticket {
    public List<string> Software { get; set; }
    public double Cost { get; set; }
    public string Reason { get; set; }
    public string Estimate { get; set; }

    public override string FileFormat()
    {
        return $"";
    }
}

public class TaskTicket : Ticket {
    public string ProjectName { get; set; }
    public DateOnly DueDate { get; set; }

    public override string FileFormat()
    {
        return $"";
    }
}