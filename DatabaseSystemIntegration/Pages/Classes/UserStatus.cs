using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class UserStatus
    {
        public string UserStatusID { get; set; }
        public string StatusFlag { get; set; }

        public UserStatus(string statusFlag)
        {
            UserStatusID = DatabaseControls.MakeID();  // Set primary key
            StatusFlag = statusFlag;
        }
    }

}
