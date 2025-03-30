using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ObjectPool;
//AI WAS USED TO IMPROVE THE UI STYLE 
namespace DatabaseSystemIntegration.Pages.Interface
{
    public class SearchModel : PageModel
    {

        public string[] Status { get; set; } = { "Complete", "Incomplete", "Past-Due" };

        [BindProperty]
        public DateOnly Date1 { get; set; }

        [BindProperty]
        public DateOnly Date2 { get; set; }

        [BindProperty]
        public string StatusSelected { get; set; }

        public Grant[] Grants { get; set; }

        public Project[] Projects { get; set; }

        public Tasks[] ProjectTasks { get; set; }


        public void SetVars()
        {
           Status = new string[]{ "Complete", "Incomplete", "Past-Due" };
           Date1 = DateOnly.FromDateTime(DateTime.Now);
           Date2 = DateOnly.FromDateTime(DateTime.Now);
        }

        public void LoadObjects()
        {
            Projects = ObjectConverter.ToProject(DatabaseControls.SelectNoFilter(12));
            Grants = ObjectConverter.ToGrants(DatabaseControls.SelectNoFilter(7));
            ProjectTasks = ObjectConverter.ToTask(DatabaseControls.SelectNoFilter(15));
        }

        public void FilterByDue()
        {
            List<Grant> GHolder = new List<Grant>();
            List<Project> PHolder = new List<Project>();
            List<Tasks> THolder = new List<Tasks>();

            foreach (Grant g in Grants)
            {
                if (g.DueDate <= DateOnly.FromDateTime(DateTime.Now) && g.AwardDate == DateOnly.MinValue)
                {
                    GHolder.Add(g);
                }

            }

            foreach (Project p in Projects)
            {
                if (p.DueDate <= DateOnly.FromDateTime(DateTime.Now) && p.EndDate == DateOnly.MinValue)
                {
                    PHolder.Add(p);
                }

            }

            foreach (Tasks t in ProjectTasks)
            {
                if (t.DueDate <= DateOnly.FromDateTime(DateTime.Now) && t.EndDate == DateOnly.MinValue)
                {
                    THolder.Add(t);
                }

            }

            Grants = GHolder.ToArray();
            Projects = PHolder.ToArray();
            ProjectTasks = THolder.ToArray();
        }

        public void FilterByCompleted()
        {
            List<Grant> GHolder = new List<Grant>();
            List<Project> PHolder = new List<Project>();
            List<Tasks> THolder = new List<Tasks>();

            foreach (Grant g in Grants)
            {
                if (g.AwardDate != DateOnly.MinValue)
                {
                    GHolder.Add(g);
                }

            }

            foreach (Project p in Projects)
            {
                if (p.EndDate != DateOnly.MinValue)
                {
                    PHolder.Add(p);
                }

            }

            foreach (Tasks t in ProjectTasks)
            {
                if (t.EndDate != DateOnly.MinValue)
                {
                    THolder.Add(t);
                }

            }

            Grants = GHolder.ToArray();
            Projects = PHolder.ToArray();
            ProjectTasks = THolder.ToArray();
        }

        public void FilterByIncomplete()
        {
            List<Grant> GHolder = new List<Grant>();
            List<Project> PHolder = new List<Project>();
            List<Tasks> THolder = new List<Tasks>();

            foreach (Grant g in Grants)
            {
                if (g.AwardDate == DateOnly.MinValue)
                {
                    GHolder.Add(g);
                }

            }

            foreach (Project p in Projects)
            {
                if (p.EndDate == DateOnly.MinValue)
                {
                    PHolder.Add(p);
                }

            }

            foreach (Tasks t in ProjectTasks)
            {
                if (t.EndDate == DateOnly.MinValue)
                {
                    THolder.Add(t);
                }

            }

            Grants = GHolder.ToArray();
            Projects = PHolder.ToArray();
            ProjectTasks = THolder.ToArray();
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


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    SetVars();
                    LoadObjects();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostFilterStatus()
        {
            LoadObjects();
            SetVars();
            if (StatusSelected == "Complete")
            {
                FilterByCompleted();
            }
            if (StatusSelected == "Incomplete")
            {
                FilterByIncomplete();
            }
            if (StatusSelected == "Past-Due")
            {
                FilterByDue();
            }
            return Page();
        }

        public IActionResult OnPostFilterDueDate()
        {
            LoadObjects();
            FilterGrantDueDate(Date1, Date2);
            FilterProjectDueDate(Date1, Date2);
            FilterTaskDueDate(Date1, Date2);
            SetVars();
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
