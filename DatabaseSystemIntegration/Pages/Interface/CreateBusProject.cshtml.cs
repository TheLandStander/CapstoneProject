using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateBusProjectModel : PageModel
    {
        [BindProperty]
        public string Project_Name { get; set; }

        [BindProperty]
        public string Project_Description { get; set; }

        [BindProperty]
        public DateOnly StartDate { get; set; }

        [BindProperty]
        public DateOnly DueDate { get; set; }

        [BindProperty]
        public DateOnly EndDate { get; set; }

        [BindProperty]
        public BusProject[] ActiveProjects { get; set; }

        [BindProperty]
        public  string Active_Project_ID { get; set; }

        public void UpdateProject()
        {
            if (EndDate > StartDate && Active_Project_ID != "")
            {
                DatabaseControls.UpdateBusProject(Active_Project_ID, EndDate);
            
            }
        
        }

        private void CheckAndSubmitProject()
        {
            if (Project_Name != "" && StartDate < DueDate)
            {
                BusProject BP = new BusProject(Project_Name, Project_Description, StartDate, DueDate);
                DatabaseControls.InsertBusProject(BP);
                ActiveProjects = DatabaseControls.GetActiveBusProjects();
            }
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    ActiveProjects = DatabaseControls.GetActiveBusProjects();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            ActiveProjects = DatabaseControls.GetActiveBusProjects();
            Project_Name = "Sample Project";
            Project_Description = "Sample Description";
            StartDate = new DateOnly(2025, 12, 1);
            DueDate = new DateOnly(2026, 12, 2);
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            ActiveProjects = DatabaseControls.GetActiveBusProjects();

            return Page();
        }


        public void OnPost()
        {
            CheckAndSubmitProject();
            UpdateProject();
        
        }
    }
}
