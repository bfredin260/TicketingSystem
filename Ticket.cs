public class Ticket {
    public string summary { get; set; }
    public string isOpen { get; set; }
    public string priority { get; set; }
    public string submitter { get; set; }
    public string assigned { get; set; }
    public string watching { get; set; }

    public Ticket(string summary, string isOpen, string priority, string submitter, string assigned, string watching) {
        this.summary = summary;
        this.isOpen = isOpen;
        this.priority = priority;
        this.submitter = submitter;
        this.assigned = assigned;
        this.watching = watching;
    }
}