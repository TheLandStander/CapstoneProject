using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateProject : PageModel
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
        public string LeadID { get; set; }

        [BindProperty]
        public string StatusID { get; set; }

        [BindProperty]
        public Users[] Users { get; set; }

        [BindProperty]
        public ProjectStatus[] ProjectStatuses { get; set; }

        public void RefreshSelection()
        {
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            ProjectStatuses = ObjectConverter.ToProjectStatus(DatabaseControls.SelectNoFilter(14));
        }


        private void CheckAndSubmitProject()
        {
            if (Project_Name != null && Project_Description != null && StatusID != null && StartDate < DueDate && LeadID != null )
            {
                Project P = new Project(Project_Name, Project_Description, StartDate, DueDate, LeadID,StatusID);
                DatabaseControls.Insert(P);
            }
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    RefreshSelection();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Project_Name = "Sample Project";
            Project_Description = "Sample Description";
            StartDate = new DateOnly(2025, 12, 1);
            DueDate = new DateOnly(2026, 12, 2);
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            RefreshSelection();
            return Page();
        }


        public void OnPost()
        {
            CheckAndSubmitProject();
            RefreshSelection();

        }
    }
}
