using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AssignEmployeeModel : PageModel
    {
        public Employee[] Employees = ObjectConverter.ToEmployee(DatabaseControls.SelectNoFilter(4));
        public BusProject[] BusProjects = ObjectConverter.ToBusProject(DatabaseControls.SelectNoFilter(2));
        public Employee[] Admins = DatabaseControls.GetAdmins(ObjectConverter.ToEmployee(DatabaseControls.SelectNoFilter(4))); 


        [BindProperty]
        public string Admin_ID { get; set; }

        [BindProperty]
        public string Employee_ID { get; set; }

        [BindProperty]
        public string Project_ID { get; set; }

        public void Submit()
        {
            EmployeeProject EP = new EmployeeProject(Admin_ID, Employee_ID, Project_ID);
            DatabaseControls.AssignProject(EP);
        
        }


        public IActionResult  OnGet()
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
