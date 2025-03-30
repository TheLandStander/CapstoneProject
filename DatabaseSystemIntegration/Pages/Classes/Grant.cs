using DatabaseSystemIntegration.Pages.Tools;
using System.Threading.Tasks;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Grant
    {
        public string GrantID { get; set; }
        public string GrantName { get; set; }
        public decimal Amount { get; set; }
        public DateOnly SubmissionDate { get; set; }
        public DateOnly AwardDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string StatusID { get; set; }
        public string CategoryID { get; set; }
        public string ProjectID { get; set; }

        public GrantCategory Category { get; set; }
        public GrantStatus Status { get; set; }

        public void SetVars()
        {
            Category = ObjectConverter.ToGrantCategory(DatabaseControls.SelectFilter(5, 5, CategoryID))[0];
            Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectFilter(6, 6, StatusID))[0];
        }

        public Project GetProject()
        {
            return ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ProjectID))[0];
        }

        public void UpdateStatus(string StatusID)
        {
            DatabaseControls.UpdateGrantStatus(GrantID, StatusID);
            if (StatusID == DatabaseControls.GetGrantStatus("Accepted").StatusID)
            {
                DatabaseControls.UpdateGrantAwardDate(GrantID, DateOnly.FromDateTime(DateTime.Now));
            }
        }


        public Grant(string grantName, decimal amount, DateOnly submissionDate, DateOnly dueDate, string statusID, string categoryID, string projectID)
        {
            GrantID = DatabaseControls.MakeID();  // Set primary key
            GrantName = grantName;
            Amount = amount;
            SubmissionDate = submissionDate;
            DueDate = dueDate;
            StatusID = statusID;
            CategoryID = categoryID;
            ProjectID = projectID;
        }
    }

}
