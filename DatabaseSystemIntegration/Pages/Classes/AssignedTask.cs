using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class AssignedTask
    {
        public string AssignedTaskID { get; set; }
        public string UserID { get; set; }
        public string TaskID { get; set; }

        public void SetVars()
        {
        }

        public Tasks GetTask()
        {
            return ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 15, TaskID))[0];
        }

        public Users GetAssignedUser()
        {
            return ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, UserID))[0];
        }


        public AssignedTask(string userID, string taskID)
        {
            AssignedTaskID = DatabaseControls.MakeID();  // Set primary key
            UserID = userID;
            TaskID = taskID;
        }
    }

}
