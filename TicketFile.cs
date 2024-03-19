using NLog;

public class TicketFile {
    public string FilePath { get; set; }
    public List<Ticket> Tickets = new List<Ticket>();
    Logger logger = LogManager.Setup().LoadConfigurationFromFile(Directory.GetCurrentDirectory() + "//nlog.config").GetCurrentClassLogger();
    
    // adds the ticket to the object's Ticket field (there will be one object for each Ticket type)
    public virtual void AddTicket(Ticket ticket) {
        Tickets.Add(ticket);

        string type;

        if(ticket is BugTicket) {
            type = "Bug";
        } else if(ticket is EnhancementTicket) {
            type = "Enhancement";
        } else {
            type = "Task";
        }

        logger.Info($"{type} Ticket added! ");
    }

}