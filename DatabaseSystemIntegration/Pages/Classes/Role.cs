using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Role
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }

        public Role(string roleName)
        {
            RoleID = DatabaseControls.MakeID();  // Set primary key
            RoleName = roleName;
        }
    }

}
