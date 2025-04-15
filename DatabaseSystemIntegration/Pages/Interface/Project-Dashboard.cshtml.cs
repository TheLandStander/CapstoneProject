using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//AI WAS USED TO IMPROVE THE UI STYLE 
namespace DatabaseSystemIntegration.Pages.Interface
{
    //This is a test
    public class Project_DashboardModel : PageModel
    {
        public Users User { get; set; }
        public Role UserRoles { get; set; }

        public Grant[] AllGrants { get; set; }

        public Project[] AllProjects { get; set; }

        public Tasks[] AllTasks { get; set; }

        public Users[] AllUsers { get; set; }

        public Partner[] Partners { get; set; }

        public Project[] AssignedProjects { get; set; }

        public Tasks[] AssignedTasks { get; set; }

        public void LoadAdmin()
        {
            AllUsers = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            AllProjects = ObjectConverter.ToProject(DatabaseControls.SelectNoFilter(12));
            AllTasks = ObjectConverter.ToTask(DatabaseControls.SelectNoFilter(15));
            AllGrants = ObjectConverter.ToGrants(DatabaseControls.SelectNoFilter(7));
            Partners = ObjectConverter.ToPartner(DatabaseControls.SelectNoFilter(2));
        }

        public void LoadGenericUser()
        {
            AssignedProjects = User.GetAllProjects();
            AssignedTasks = User.GetAllTasks();
        }

        public void LoadProjectManager()
        {
            AllProjects = ObjectConverter.ToProject(DatabaseControls.SelectNoFilter(12));
            AllGrants = ObjectConverter.ToGrants(DatabaseControls.SelectNoFilter(7));
        }




        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("UserType") == "Admin" || HttpContext.Session.GetString("UserType") == "Project-Manager")
            {
                LoadAdmin();
            }
            else
            {
                LoadGenericUser();
            }
            return Page();
        }
        public IActionResult OnGet()
        {

            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                User = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 19, HttpContext.Session.GetString("UserID")))[0];
                if (HttpContext.Session.GetInt32("LoggedIn") == 1)
                {
                    if (HttpContext.Session.GetString("UserType") == "Admin" || HttpContext.Session.GetString("UserType") == "Project-Manager")
                    {
                        LoadAdmin();
                    }
                    else
                    {
                        LoadGenericUser();
                    }
                }
                else
                {
                    HttpContext.Session.SetString("UserType", "Guest");
                }
            }

            return Page();
        }

        public IActionResult OnPostCreatePartner()
        {

            return RedirectToPage("CreatePartner");
        }

        public IActionResult OnPostCreateProject()
        {

            return RedirectToPage("CreateProject");
        }

        public IActionResult OnPostCreateGrant()
        {

            return RedirectToPage("CreateGrant");
        }

        public IActionResult OnPostCreateUser()
        {

            return RedirectToPage("CreateUser");
        }

        public IActionResult OnPostGenerateReport()
        {

            return RedirectToPage("Search");
        }


        public IActionResult OnPostSelectPartner(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Partner");
            HttpContext.Session.SetString("ItemID", ID);

            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Project");
            HttpContext.Session.SetString("ItemID", ID);

            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectTask(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Task");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectChildTask(string ID)
        {
            HttpContext.Session.SetString("ItemType", "MeetingNote");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }
    }
}
