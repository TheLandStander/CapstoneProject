using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AddTasksModel : PageModel
    {
        [BindProperty]
        public string TaskName { get; set; }

        [BindProperty]
        public string Task_Description { get; set; }

        [BindProperty]
        public string Bus_Project_ID { get; set; }

        [BindProperty]
        public BusProject[] Projects { get; set; } = DatabaseControls.GetActiveBusProjects();

        [BindProperty]
        public BusPartner[] Partner { get; set; } = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));

        public void CheckAddTask()
        {
            if (TaskName != null && Bus_Project_ID != null)
            {
                Classes.Task t = new Classes.Task(TaskName, Task_Description, Bus_Project_ID);
                DatabaseControls.InsertTask(t);
            }
        }


        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Task_Description = "Sample Task Description";
            TaskName = "Sample Task Name";
            Partner = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            Projects = DatabaseControls.GetActiveBusProjects();
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            Partner = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            Projects = DatabaseControls.GetActiveBusProjects();
            return Page();
        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    Partner = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
                    Projects = DatabaseControls.GetActiveBusProjects();

                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public void OnPost()
        {
            CheckAddTask();
            Partner = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            Projects = DatabaseControls.GetActiveBusProjects();
        }

    }

}