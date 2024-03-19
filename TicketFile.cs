using NLog;

public class TicketFile {
    public string FilePath { get; set; }
    public List<Ticket> Tickets { get; set; }
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();
    
    public TicketFile() {
        Tickets = new List<Ticket>();
    }

    // adds the ticket to the object's Ticket field (there will be one object for each Ticket type)
    public virtual void AddToList(Ticket ticket) {
        Tickets.Add(ticket);

        string type;

        if(ticket is BugTicket) {
            type = "Bug";
        } else if(ticket is EnhancementTicket) {
            type = "Enhancement";
        } else {
            type = "Task";
        }

        Console.WriteLine();
        logger.Info($"{type} Ticket added! ");
    }

}