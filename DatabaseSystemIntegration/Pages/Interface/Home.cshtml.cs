using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class Home : PageModel
    {
        public Users User { get; set; }

        [BindProperty]
        public string Search { get; set; }

        public Tasks[] DisplayedTasks { get; set; }

        public Project[] DisplayedProjects { get; set; }

        public Tasks[] SearchTasks(string Name)
        {
            Tasks[] AllTasks = DatabaseControls.GetUserTasks(User.UserID);
            List<Tasks> SearchResults = new List<Tasks>();
            List<Tasks> ActiveTasks = new List<Tasks>();
            foreach (Tasks t in AllTasks)
            {
                if (t.TaskName == Name)
                {
                    Console.Write(Name);
                    SearchResults.Add(t);
                }
            }

            foreach (Tasks t in SearchResults)
            {
                if (t.Completed == false)
                {
                    ActiveTasks.Add(t);
                
                }
            }

            return ActiveTasks.OrderBy(t => t.DueDate).ToArray();
        }

        public IActionResult OnPostSearch()
        {
            if (Search != null)
            {
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedTasks = SearchTasks(Search);
                DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
            }
            else
            {
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            }
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
            return Page();
        }

        public void OnPost()
        {
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
                DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
                return Page();
            }

            return RedirectToPage("/index");
        }

        public IActionResult OnPostSelectProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Project");
            HttpContext.Session.SetString("ItemID", ID);
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectTask(string ID)
        {
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            DisplayedProjects = DatabaseControls.GetUserProjects(User.UserID).OrderBy(p => p.DueDate).ToArray();
            HttpContext.Session.SetString("ItemType", "Task");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }
    }
}
