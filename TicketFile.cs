using NLog;

public class BugTicketFile {
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<BugTicket> Tickets = new List<BugTicket>();

    public void AddToList(BugTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Bug Ticket added! ");
    }
}

public class EnhancementTicketFile {
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<EnhancementTicket> Tickets = new List<EnhancementTicket>();

    public void AddToList(EnhancementTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Enhancement Ticket added! ");
    }
}

public class TaskTicketFile {
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();

    public List<TaskTicket> Tickets = new List<TaskTicket>();

    public void AddToList(TaskTicket ticket) {
        Tickets.Add(ticket);

        Console.WriteLine();
        logger.Info($"Task Ticket added! ");
    }
}