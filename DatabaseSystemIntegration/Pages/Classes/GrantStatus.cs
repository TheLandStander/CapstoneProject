using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class GrantStatus
    {
        public string StatusID { get; set; }
        public string GrantStatusName { get; set; }

        public GrantStatus(string grantStatusName)
        {
            StatusID = DatabaseControls.MakeID();  // Set primary key
            GrantStatusName = grantStatusName;
        }
    }

}
