using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class ProjectNotesModel : PageModel
    {
        [BindProperty]
       public ProjectNotes[] Notes { get; set; }
        public void getNotes()

        {
            Employee ThisEmployee = ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 14, HttpContext.Session.GetString("AccountID")))[0];
            EmployeeProject[] EP = ObjectConverter.ToEmployeeProject(DatabaseControls.SelectFilter(5, 4, ThisEmployee.Employee_ID));
            List<ProjectNotes> NotesHolder = new List<ProjectNotes>();

            foreach (EmployeeProject ep in EP)
            {

                NotesHolder.Add(ObjectConverter.ToProjectNotes(DatabaseControls.SelectFilter(15,2,ep.Bus_Project_ID))[0]);
            }

            Notes = NotesHolder.ToArray();
        }

        public void OnGet()
        {
            getNotes();
            if (HttpContext.Session.GetString("UserType") == "Admin")
            {
                Notes = ObjectConverter.ToProjectNotes(DatabaseControls.SelectNoFilter(15));
            }
        }
    }
}
