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


        public Users[] GetEmployees()
        {
            AssignedProject[] ap = ObjectConverter.ToAssignedProject(DatabaseControls.SelectFilter(1, 15, TaskID));
            List<Users> holder = new List<Users>();

            foreach (AssignedProject p in ap)
            {
                holder.Add(p.GetAssignedUser());
            }

            return holder.ToArray();
        }

        public void CompleteTask()
        {
            Completed = true;
            DatabaseControls.CompleteTask(this);
        }

        public void AssignTask(string UserID)
        {
            AssignedTask at = new AssignedTask(UserID,TaskID);
            if (DatabaseControls.CheckDuplicate(at) == false)
            {
                DatabaseControls.Insert(at);
            }
        }

        public void AddChildTask(string Name, string Description,DateOnly Start,DateOnly Due)
        {
            ChildTask ct = new ChildTask(Name, Description, Start,Due ,false, TaskID);
            DatabaseControls.Insert(ct);
        }

        public Project GetProject()
        { 
            return ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ProjectID))[0];
        }

        public ChildTask[] GetChildTasks()
        {
           return ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 15, TaskID));
        }

        public void SetVars()
        {

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
