using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AssignFacultyModel : PageModel
    {
        public Faculty[] Faculty = ObjectConverter.ToFaculty(DatabaseControls.SelectNoFilter(6));
        public GrantProject[] GrantProjects = ObjectConverter.ToGrantProject(DatabaseControls.SelectNoFilter(9));
      
        [BindProperty]
        public string Faculty_ID { get; set; }

        [BindProperty]
        public string Project_ID { get; set; }

        public void Submit()
        {
            FacultyProject FP = new FacultyProject(Faculty_ID, Project_ID);
            DatabaseControls.AssignFacultyProject(FP);

        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public void OnPost()
        {
            Submit();
        }
    }
}