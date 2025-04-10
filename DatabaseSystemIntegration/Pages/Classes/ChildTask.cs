using DatabaseSystemIntegration.Pages.Tools;
using System.Threading.Tasks;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ChildTask
    {
        public string ChildTaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool Completed { get; set; }

        public string UserID { get; set; }

        public string ItemID { get; set; }


        public void CompleteTask()
        {
            Completed = true;
            DatabaseControls.CompleteChildTask(this);
        }

        public bool isAssigned()
        {
            if (UserID.Length == 10)
            {
                return true;
            }
            return false;
        }

        public void AssignTask(string userID)
        {
                UserID = userID;
                DatabaseControls.AssignChildTask(this);
        }

        public Users GetAssignedUser()
        {
            if (DatabaseControls.SelectFilter(19, 19, UserID).HasRows)
            {
                return ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, UserID))[0];
            }
            return null; 
        }


        public ChildTask(string taskName, string description, DateOnly Start, DateOnly Due, bool completed, string itemID)
        {
            ChildTaskID = DatabaseControls.MakeID();  // Set primary key
            TaskName = taskName;
            Description = description;
            StartDate = Start;
            DueDate = Due;
            Completed = completed;
            ItemID = itemID;
        }
    }

}

