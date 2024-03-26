using NLog;

public abstract class TicketFile {
    public void AddToTemp(string ticket) {
        StreamWriter sw = new StreamWriter("TempTickets.csv", append: true);

        sw.WriteLine(ticket);

        sw.Close();
    }
}

public class BugTicketFile : TicketFile{
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<BugTicket> Tickets = new List<BugTicket>();

    public void AddToList(BugTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Bug Ticket added! ");
    }
}

public class EnhancementTicketFile : TicketFile {
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<EnhancementTicket> Tickets = new List<EnhancementTicket>();

    public void AddToList(EnhancementTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Enhancement Ticket added! ");
    }
}

public class TaskTicketFile : TicketFile{
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<TaskTicket> Tickets = new List<TaskTicket>();

    public void AddToList(TaskTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Task Ticket added! ");
    }
}