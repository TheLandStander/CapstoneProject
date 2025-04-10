using DatabaseSystemIntegration.Pages.Tools;
using System.Threading.Tasks;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Project
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string ProjectLeadID { get; set; }
        public string ProjectStatusID { get; set; }
        public string GrantID { get; set; }
        public Users ProjectLeader { get; set; }

        public ProjectStatus Status { get; set; }

        public Users[] GetEmployees()
        {
            AssignedProject[] ap = ObjectConverter.ToAssignedProject(DatabaseControls.SelectFilter(0, 12, ProjectID));
            List<Users> holder = new List<Users>();

            foreach (AssignedProject p in ap)
            {
                holder.Add(p.GetAssignedUser());
            }

            return holder.ToArray();
        }

        public ProjectNotes[] GetNotes()
        {
            return DatabaseControls.GetNotes(ProjectID);
        }

        public Tasks[] GetTasks()
        {
            return ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 12, ProjectID));
        }

        public Grant[] GetGrants()
        {
            return ObjectConverter.ToGrants(DatabaseControls.SelectFilter(7, 7, GrantID));
        }

        public Users GetProjectLead()
        { 
            return ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, ProjectLeadID))[0];
        }

        public void SetProjectLeader(string LeaderID)
        {
            ProjectLeadID = LeaderID;
            ProjectLeader = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, LeaderID))[0];
            DatabaseControls.SetProjectLead(this,ProjectLeader);
        }

        public void UpdateStatus(string StatusID)
        {
            DatabaseControls.UpdateProjectStatus(ProjectID, StatusID);
            
        }

        public void UpdateDueDate(DateOnly Date)
        {
            DatabaseControls.UpdateProjectDueDate(ProjectID, Date);
        }

        public void AssignProject(string UserID)
        {
            AssignedProject[] All = ObjectConverter.ToAssignedProject(DatabaseControls.SelectNoFilter(0));
            AssignedProject ap = new AssignedProject(UserID, ProjectID);
            if (DatabaseControls.CheckDuplicate(ap) == false)
            {
                DatabaseControls.Insert(ap);
            }
        }

        public void AddNote(string Note, string Author,string Recipient)
        {
            ProjectNotes PN = new ProjectNotes(Note,Author,Recipient, DateOnly.FromDateTime(DateTime.Now),ProjectID);
            DatabaseControls.Insert(PN);
        }

        public void AddTask(string Name, string Description, DateOnly Due)
        {
            Tasks t = new Tasks(Name, Description, DateOnly.FromDateTime(DateTime.Now), Due, false, ProjectID);
            DatabaseControls.Insert(t);
        }

        public void SetVars()
        {
            Status = ObjectConverter.ToProjectStatus(DatabaseControls.SelectFilter(14,14, ProjectStatusID))[0];
        }

        public Project(string projectName, string description, DateOnly startDate,DateOnly dueDate, string projectStatusID, string grantID)
        {
            ProjectID = DatabaseControls.MakeID();  // Set primary key
            ProjectName = projectName;
            Description = description;
            StartDate = startDate;
            DueDate = dueDate;
            ProjectStatusID = projectStatusID;
            GrantID = grantID;
        }
    }

}
