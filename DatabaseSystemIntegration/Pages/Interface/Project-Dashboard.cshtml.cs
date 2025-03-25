using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class Project_DashboardModel : PageModel
    {

        [BindProperty]
        public GrantProject[] GrantProjects { get; set; }

        [BindProperty]
        public BusProject[] BusProjects { get; set; }

        [BindProperty]
        public Grant[] Grants { get; set; }

        [BindProperty]
        public GrantStatus[] GrantStatus { get; set; }

        [BindProperty]
        public GrantCategory[] GrantCategories { get; set; }

        [BindProperty]
        public BusPartner[] Partners { get; set; }

        [BindProperty]
        public Faculty[] Faculty { get; set; }


        [BindProperty]
        public FacultyProject[] FacultyProject { get; set; }

        [BindProperty]
        public Employee[] Employees { get; set; }

        [BindProperty]
        public BusRep[] Reps { get; set; }


        public void LoadEmployee()
        {
            string ID = HttpContext.Session.GetString("AccountID");
            List<BusProject> ProjectHolder = new List<BusProject>();
            List<Employee> EmployeeHolder = new List<Employee>();
            Employee ThisEmployee = ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 14, ID))[0];
            EmployeeProject[] EP = ObjectConverter.ToEmployeeProject(DatabaseControls.SelectFilter(5, 4, ThisEmployee.Employee_ID));

            foreach (EmployeeProject P in EP)
            {
                //find all the employees associated with the project and the projects associated with the current user
                //there should be only one project/employee associated with each ID
                EmployeeHolder.Add(ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 4, P.Employee_ID))[0]);
                ProjectHolder.Add(ObjectConverter.ToBusProject(DatabaseControls.SelectFilter(2, 2, P.Bus_Project_ID))[0]);
             }

            BusProjects = ProjectHolder.ToArray();
            Employees = EmployeeHolder.ToArray();
        }

        public void LoadBusRep()
        {
            string ID = HttpContext.Session.GetString("AccountID");
            List<Grant> GrantHolder = new List<Grant>();
            BusRep ThisRep = ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 14, ID))[0];
            BusPartner ThisPartner = ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0, 14, ThisRep.Bus_Partner_ID))[0];
            Grants = ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 0, ThisRep.Bus_Partner_ID));
            List<GrantProject> GPHolder = new List<GrantProject>();

            foreach (Grant g in Grants)
            {
                GPHolder.Add(ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(10, 10, g.Grant_Project_ID))[0]);
            }

            GrantProjects = GPHolder.ToArray();
        }

        public void LoadPartner()
        {
            
            string ID = HttpContext.Session.GetString("AccountID");
            BusPartner ThisPartner = ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0, 14,ID))[0];
            Grants = ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 0,ThisPartner.Bus_Partner_ID));
            List<GrantProject> GPHolder = new List<GrantProject>();

            foreach (Grant g in Grants)
            {
                GPHolder.Add(ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, g.Grant_Project_ID))[0]);
            }

            GrantProjects = GPHolder.ToArray();
        }

        public void LoadAdmin()
        {
            Grants = ObjectConverter.ToGrant(DatabaseControls.SelectNoFilter(10));
            GrantProjects = ObjectConverter.ToGrantProject(DatabaseControls.SelectNoFilter(9));
            BusProjects = ObjectConverter.ToBusProject(DatabaseControls.SelectNoFilter(2));
            Partners = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            FacultyProject = ObjectConverter.ToFacultyProject(DatabaseControls.SelectNoFilter(7));

        }

        public void LoadFaculty()
        {
            string ID = HttpContext.Session.GetString("AccountID");
            Faculty ThisFaculty = ObjectConverter.ToFaculty(DatabaseControls.SelectFilter(6, 14, ID))[0];
            List<Grant> GrantHolder = new List<Grant>();
            FacultyProject = ObjectConverter.ToFacultyProject(DatabaseControls.SelectFilter(7, 6, ThisFaculty.Faculty_ID));
            List<GrantProject> GPHolder = new List<GrantProject>();
            foreach (FacultyProject FP in FacultyProject)
            {
                GrantHolder.Add(ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 9, FP.Grant_Project_ID))[0]);
                GPHolder.Add(ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, FP.Grant_Project_ID))[0]);

            }

            Grants = GrantHolder.ToArray();
            GrantProjects = GPHolder.ToArray();
        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                string UserType = HttpContext.Session.GetString("UserType");
                if (UserType == "Admin")
                {
                    LoadAdmin();
                }
                else if (UserType == "Employee")
                {

                    LoadEmployee();
                }
                else if (UserType == "BusPartner")
                {
                    LoadPartner();
                }
                else if (UserType == "BusRep")
                {
                    LoadBusRep();
                }
                else if (UserType == "Faculty")
                {
                    LoadFaculty();
                }

                    return Page();
            }
            return RedirectToPage("Index");
        }

        public IActionResult OnPostSelectBusProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "BusProject");
            HttpContext.Session.SetString("ItemID", ID);

            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrantProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "GrantProject");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectProjectNote(string ID)
        {
            HttpContext.Session.SetString("ItemType", "ProjectNote");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectMeetingNote(string ID)
        {
            HttpContext.Session.SetString("ItemType", "MeetingNote");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }
    }
}
