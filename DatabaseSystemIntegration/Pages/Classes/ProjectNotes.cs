using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ProjectNotes
    {
        public string NotesID { get; set; }
        public string Notes { get; set; }
        public DateOnly Date { get; set; }
        public string ProjectID { get; set; }
        public Project AssociatedProject { get; set; }

        public void SetVars()
        {
            AssociatedProject = ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ProjectID))[0];
        }
        public ProjectNotes(string projectNotes, DateOnly date, string projectID)
        {
            NotesID = DatabaseControls.MakeID();  // Set primary key
            Notes = projectNotes;
            Date = date;
            ProjectID = projectID;
        }
    }

}
