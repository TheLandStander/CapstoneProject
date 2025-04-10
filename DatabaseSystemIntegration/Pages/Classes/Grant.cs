using DatabaseSystemIntegration.Pages.Tools;
using System.Threading.Tasks;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Grant
    {
        public string GrantID { get; set; }
        public string GrantName { get; set; }
        public string FundingAgency { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly SubmissionDate { get; set; }
        public DateOnly AwardDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string StatusID { get; set; }
        public string CategoryID { get; set; }

        public GrantCategory Category { get; set; }
        public GrantStatus Status { get; set; }

        public void SetVars()
        {
            Category = ObjectConverter.ToGrantCategory(DatabaseControls.SelectFilter(5, 5, CategoryID))[0];
            Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectFilter(6, 6, StatusID))[0];
        }

        public Project GetProject()
        {
            if (DatabaseControls.SelectFilter(12, 7, GrantID).HasRows)
            {
                return ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 7, GrantID))[0];
            }
            return null;
        }

        public void UpdateStatus(string StatusID)
        {
            DatabaseControls.UpdateGrantStatus(GrantID, StatusID);
            if (StatusID == DatabaseControls.GetGrantStatus("Accepted").StatusID)
            {
                DatabaseControls.UpdateSubmissionDate(GrantID, DateOnly.FromDateTime(DateTime.Now));
                DatabaseControls.UpdateGrantAwardDate(GrantID, DateOnly.FromDateTime(DateTime.Now));
            }
            if (StatusID == DatabaseControls.GetGrantStatus("Pending").StatusID)
            {
                DatabaseControls.UpdateSubmissionDate(GrantID, DateOnly.FromDateTime(DateTime.Now));
            }
        }

        public ProjectNotes[] GetNotes()
        {
            return DatabaseControls.GetNotes(GrantID);
        }

        public ChildTask[] GetSubTasks()
        {
            return DatabaseControls.GetSubTasks(GrantID);
        }

        public void AddNote(string Note, string Author, string Recipient)
        {
            ProjectNotes PN = new ProjectNotes(Note, Author, Recipient, DateOnly.FromDateTime(DateTime.Now), GrantID);
            DatabaseControls.Insert(PN);
        }

        public Grant(string grantName,string agency, decimal amount, string statusID, string categoryID)
        {
            GrantID = DatabaseControls.MakeID();  // Set primary key
            GrantName = grantName;
            FundingAgency = agency.ToUpper();
            Amount = amount;
            StatusID = statusID;
            CategoryID = categoryID;
        }
    }

}
