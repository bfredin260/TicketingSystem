using NLog;

// super class to inherit method from
public abstract class TicketFile {

    // adds ticket ToString(), containing field info, to the temp file
    public static void AddToTemp(string ticket) {
        StreamWriter sw = new("TempTickets.csv", append: true);

        sw.WriteLine(ticket);

        // close stream
        sw.Close();
    }
}

public class BugTicketFile : TicketFile{
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<BugTicket> Tickets = new();

    // adds ticket to the object's ticket file
    public void AddToList(BugTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Bug Ticket added! ");
    }
}

public class EnhancementTicketFile : TicketFile {
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<EnhancementTicket> Tickets = new();

    // adds ticket to the object's ticket file
    public void AddToList(EnhancementTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Enhancement Ticket added! ");
    }
}

public class TaskTicketFile : TicketFile{
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<TaskTicket> Tickets = new();
    
    // adds ticket to the object's ticket file
    public void AddToList(TaskTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Task Ticket added! ");
    }
}