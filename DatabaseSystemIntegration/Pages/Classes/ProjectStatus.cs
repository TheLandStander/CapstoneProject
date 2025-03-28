using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ProjectStatus
    {
        public string ProjectStatusID { get; set; }
        public string ProjectStatusName { get; set; }

        public ProjectStatus(string projectStatusName)
        {
            ProjectStatusID = DatabaseControls.MakeID();  // Set primary key
            ProjectStatusName = projectStatusName;
        }
    }

}
