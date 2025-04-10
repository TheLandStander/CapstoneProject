using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
//AI WAS USED TO IMPROVE THE UI STYLE 
namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateProject : PageModel
    {
        [BindProperty]
        public string Project_Name { get; set; }

        [BindProperty]
        public string Project_Description { get; set; }

        [BindProperty]
        public DateOnly DueDate { get; set; }

        [BindProperty]
        public Users[] Users { get; set; }

        public void RefreshSelection()
        {
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            DueDate = DateOnly.FromDateTime(DateTime.Now);
        }


        public void CheckAndSubmitProject(string ID)
        {
            if (Project_Name != null && Project_Description != null && DateOnly.FromDateTime(DateTime.Now) < DueDate)
            {
                Project P = new Project(Project_Name, Project_Description, DateOnly.FromDateTime(DateTime.Now), DueDate,DatabaseControls.GetProjectStatus("Ongoing").ProjectStatusID,ID);
                DatabaseControls.Insert(P);
            }
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin" || HttpContext.Session.GetString("UserType") == "Project-Manager")
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
            DueDate = new DateOnly(2026, 12, 2);
            RefreshSelection();
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            RefreshSelection();
            return Page();
        }


        public IActionResult OnPost(string ID)
        {
            CheckAndSubmitProject(ID);
            RefreshSelection();
            return RedirectToPage("Project-Dashboard");
        }
    }
}
