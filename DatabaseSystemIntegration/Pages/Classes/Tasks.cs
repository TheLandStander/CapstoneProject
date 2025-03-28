using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Tasks
    {
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool Completed { get; set; }
        public string ProjectID { get; set; }
        public Project AssociatedProject { get; set; }

        public ChildTask[] ChildTasks { get; set; }

        public Users[] GetEmployees()
        {
            AssignedProject[] ap = ObjectConverter.ToAssignedProject(DatabaseControls.SelectFilter(1, 1, TaskID));
            List<Users> holder = new List<Users>();

            foreach (AssignedProject p in ap)
            {
                holder.Add(p.AssignedUser);
            }

            return holder.ToArray();
        }

        public void CompleteTask()
        {
            DatabaseControls.CompleteTask(this);
        }

        public void AssignTask(string UserID)
        {
            AssignedTask at = new AssignedTask(UserID,TaskID);
            DatabaseControls.Insert(at);
        }

        public void AddChildTask(string Name, string Description,DateOnly Start,DateOnly Due)
        {
            ChildTask ct = new ChildTask(Name, Description, Start,Due ,false, TaskID);
            DatabaseControls.Insert(ct);
        }

        public void SetVars()
        {
            AssociatedProject = ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ProjectID))[0];
            ChildTasks = ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 15, TaskID));
        }
        public Tasks(string taskName, string description,DateOnly Start,DateOnly Due,bool completed, string projectID)
        {
            TaskID = DatabaseControls.MakeID();  // Set primary key
            TaskName = taskName;
            Description = description;
            StartDate = Start;
            DueDate = Due;
            Completed = completed;
            ProjectID = projectID;
        }
    }

}
