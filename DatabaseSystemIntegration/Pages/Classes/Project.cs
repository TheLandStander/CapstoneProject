using DatabaseSystemIntegration.Pages.Tools;

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

        public ProjectNotes[] ProjectNotes { get; set; }
        public Grant[] ProjectGrants {get;set;}
        public Tasks[] ProjectTasks { get; set; }
        public Users ProjectLeader { get; set; }

        public ProjectStatus Status { get; set; }

        public Users[] GetEmployees()
        {
            AssignedProject[] ap = ObjectConverter.ToAssignedProject(DatabaseControls.SelectFilter(0, 0, ProjectID));
            List<Users> holder = new List<Users>();

            foreach (AssignedProject p in ap)
            {
                holder.Add(p.AssignedUser);
            }

            return holder.ToArray();
        }

        public void SetProjectLeader(Users Leader)
        {
            ProjectLeader = Leader;
            DatabaseControls.SetProjectLead(this, Leader);
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
            AssignedProject ap = new AssignedProject(UserID, ProjectID);
            DatabaseControls.Insert(ap);
        }

        public void AddNote(string Note)
        {
            ProjectNotes PN = new ProjectNotes(Note, DateOnly.FromDateTime(DateTime.Now),ProjectID);
            DatabaseControls.Insert(PN);
        }


        public void SetVars()
        {
            ProjectLeader = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19,19, ProjectLeadID))[0];
            Status = ObjectConverter.ToProjectStatus(DatabaseControls.SelectFilter(14,14, ProjectStatusID))[0];
            ProjectNotes = ObjectConverter.ToProjectNotes(DatabaseControls.SelectFilter(13, 12, ProjectID));
            ProjectGrants = ObjectConverter.ToGrants(DatabaseControls.SelectFilter(7, 12, ProjectID));
            ProjectTasks = ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 12, ProjectID));
        }

        public Project(string projectName, string description, DateOnly startDate,DateOnly dueDate, string projectLead, string projectStatusID)
        {
            ProjectID = DatabaseControls.MakeID();  // Set primary key
            ProjectName = projectName;
            Description = description;
            StartDate = startDate;
            DueDate = dueDate;
            ProjectLeadID = projectLead;
            ProjectStatusID = projectStatusID;
        }
    }

}
