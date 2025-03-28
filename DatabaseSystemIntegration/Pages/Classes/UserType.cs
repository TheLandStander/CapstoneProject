using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class UserType
    {
        public string UserTypeID { get; set; }
        public string UserTypeName { get; set; }

        public UserType(string userTypeName)
        {
            UserTypeID = DatabaseControls.MakeID();  // Set primary key
            UserTypeName = userTypeName;
        }
    }

}
