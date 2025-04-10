using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ProjectNotes
    {
        public string NotesID { get; set; }
        public string Notes { get; set; }
        public string Author { get; set; }
        public string Recipient { get; set; }
        public DateOnly Date { get; set; }
        public string ItemID { get; set; }

        
        public ProjectNotes(string note,string author, string recipient, DateOnly date, string item)
        {
            NotesID = DatabaseControls.MakeID();  // Set primary key
            Notes = note;
            Author = author;
            Recipient = recipient;
            Date = date;
            ItemID = item;
        }
    }

}
