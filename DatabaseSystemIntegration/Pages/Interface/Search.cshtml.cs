using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ObjectPool;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class SearchModel : PageModel
    {

        public string[] Filters { get; set; } = { "Grant", "Task", "Project"};

       [BindProperty]
       public string SelectedFilter { get; set; }

        [BindProperty]
        public DateOnly Date1 { get; set; }

        [BindProperty]
        public DateOnly Date2 { get; set; }

        [BindProperty]
        public string StatusID { get; set; }

        public GrantStatus[] GrantStatus { get; set; }

        public ProjectStatus[] ProjectStatus { get; set; }

        public Grant[] Grants { get; set; }

        public Project[] Projects { get; set; }

        public Tasks[] ProjectTasks { get; set; }


        public void SetVars()
        {
           Projects = ObjectConverter.ToProject(DatabaseControls.SelectNoFilter(12));
           Grants = ObjectConverter.ToGrants(DatabaseControls.SelectNoFilter(7));
           ProjectTasks = ObjectConverter.ToTask(DatabaseControls.SelectNoFilter(15));
           GrantStatus = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(6));
           ProjectStatus = ObjectConverter.ToProjectStatus(DatabaseControls.SelectNoFilter(14));

        }

      
        public void FilterGrantDueDate(DateOnly D1, DateOnly D2)
        {
           
          List<Grant> GHolder = new List<Grant>();

            foreach (Grant g in Grants)
            {
                if (g.DueDate >= D1 && g.DueDate <= D2)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();
        }


        public void FilterProjectDueDate(DateOnly D1, DateOnly D2)
        {
            List<Project> Holder = new List<Project>();
            foreach (Project p in Projects)
            {
                if (p.DueDate >= D1 && p.DueDate <= D2)
                {
                    Holder.Add(p);
                }

            }
            Projects = Holder.ToArray();
        }

        public void FilterTaskDueDate(DateOnly D1, DateOnly D2)
        {
            List<Tasks> Holder = new List<Tasks>();
            foreach (Tasks t in ProjectTasks)
            {
                if (t.DueDate >= D1 && t.DueDate <= D2)
                {
                    Holder.Add(t);
                }

            }
            ProjectTasks = Holder.ToArray();
        }

        public void FilterGrantStatus(string Status)
        {

            List<Grant> GHolder = new List<Grant>();
            foreach (Grant g in Grants)
            {
                if (g.StatusID == Status)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();

        }

        public void FilterProjectStatus(string Status)
        {

            List<Project> Holder = new List<Project>();
            foreach (Project p in Projects)
            {
                if (p.ProjectStatusID == Status)
                {
                    Holder.Add(p);
                }

            }
            Projects = Holder.ToArray();

        }

        public void FilterTaskStatus(string status)
        {
            List<Tasks> Holder = new List<Tasks>();
            foreach (Tasks t in ProjectTasks)
            {
                if (t.Completed == true && status == "Complete" || t.Completed == false && status == "Incomplete")
                {
                    Holder.Add(t);
                }

            }
           ProjectTasks = Holder.ToArray();

        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    SetVars();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public void Load()
        {
            SetVars();
            if (SelectedFilter == "Grant")
            {
                if (Date1 < Date2)
                {
                    FilterGrantDueDate(Date1, Date2);
                }
                if (StatusID != null)
                {
                    FilterGrantStatus(StatusID);
                }
            }

            if (SelectedFilter == "Task")
            {
                if (Date1 < Date2)
                {
                    FilterTaskDueDate(Date1, Date2);
                }
                if (StatusID != null)
                {
                    FilterTaskStatus(StatusID);
                }
            }

            if (SelectedFilter == "Project")
            {
                if (Date1 < Date2)
                {
                    FilterProjectDueDate(Date1, Date2);
                }
                if (StatusID != null)
                {
                    FilterProjectStatus(StatusID);
                }
            }
        }


        public IActionResult OnPost()
        {
            Load();
            return Page();
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

    }
}
