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

        public ChildTask[] DisplayedSubTasks { get; set; }

        public void SetObjects()
        {
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            DisplayedSubTasks = DatabaseControls.GetUserSubTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
        }

        public ChildTask[] SearchTasks(string Name)
        {
            ChildTask[] AllTasks = DatabaseControls.GetUserSubTasks(User.UserID);
            List<ChildTask> SearchResults = new List<ChildTask>();
            List<ChildTask> ActiveTasks = new List<ChildTask>();
            foreach (ChildTask t in AllTasks)
            {
                if (t.TaskName == Name)
                {
                    Console.Write(Name);
                    SearchResults.Add(t);
                }
            }

            foreach (ChildTask t in SearchResults)
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
                DisplayedSubTasks = SearchTasks(Search);
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
            }
            else
            {
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedSubTasks = DatabaseControls.GetUserSubTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
            }
            User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            return Page();
        }

        public void OnPost()
        {
            SetObjects();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                SetObjects();
                return Page();
            }

            return RedirectToPage("/index");
        }

        public IActionResult OnPostSelectProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Project");
            HttpContext.Session.SetString("ItemID", ID);
            SetObjects();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectTask(string ID)
        {
            SetObjects();
            HttpContext.Session.SetString("ItemType", "Task");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public void CompleteChildTask(string ID)
        {
           ChildTask ChildTask = ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 4, ID))[0];
           ChildTask.CompleteTask();
        }
        public IActionResult OnPostUpdateChildTask(string ID)
        {
            SetObjects();
            CompleteChildTask(ID);
            return Page();
        }


    }
}
