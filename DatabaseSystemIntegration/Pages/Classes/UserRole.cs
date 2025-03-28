using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class UserRole
    {
        public string UserRoleID { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }

        public Role role { get; set; }

        public void SetVars()
        {
             role = ObjectConverter.ToRole(DatabaseControls.SelectFilter(10,10, RoleID))[0];
        }

        public Users getUser()
        {
           return ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, UserID))[0];
        }

        public UserRole(string userID, string roleID)
        {
            UserRoleID = DatabaseControls.MakeID();  // Set primary key
            UserID = userID;
            RoleID = roleID;
        }
    }

}
