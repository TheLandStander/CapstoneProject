using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AccessItemModel : PageModel
    {

        [BindProperty]
        public string GrantStatusID { get; set; } 

        [BindProperty]
        public string ProjectStatusID { get; set; }

        public string ItemType { get; set; }
        public string ItemID { get; set; }
        public Grant grant { get; set; }
        public Project project { get; set; }
        public GrantStatus[] GrantStatus { get; set; }
        public ProjectStatus[] ProjectStatus { get; set; }
        public Tasks ParentTask { get; set; }
        public ChildTask ChildTask { get; set; }

        public void LoadGrant()
        {
            if (ItemType == "Grant")
            {
                grant = ObjectConverter.ToGrants(DatabaseControls.SelectFilter(7, 7, ItemID))[0];
                GrantStatus = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(6));
            }
        }

        public void LoadProject()
        {
            if (ItemType == "Project")
            {
                project = ObjectConverter.ToProject(DatabaseControls.SelectFilter(12, 12, ItemID))[0];
                ProjectStatus = ObjectConverter.ToProjectStatus(DatabaseControls.SelectNoFilter(14));
            }
        }

        public void LoadTask()
        {
            if (ItemType == "Task")
            {
                ParentTask = ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 15, ItemID))[0];
            }
        }

        public void LoadChildTask()
        {
            if (ItemType == "ChildTask")
            {
                ChildTask = ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 4, ItemID))[0];
            }
        }


        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Project");
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
            HttpContext.Session.SetString("ItemType", "ChildTask");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateProject()
        {
            SetVars();
            if (ProjectStatusID != null)
            {
                project.UpdateStatus(ProjectStatusID);
            }
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateGrant()
        {
            SetVars();
            if (GrantStatusID != null)
            {
                grant.UpdateStatus(GrantStatusID);
            }
            return RedirectToPage("AccessItem");
        }

        public void SetVars()
        {
            ItemType = HttpContext.Session.GetString("ItemType");
            ItemID = HttpContext.Session.GetString("ItemID");
            LoadGrant();
            LoadProject();
        }

        public void OnGet()
        {
            SetVars();
        }

        public void OnPost()
        {
            SetVars();

        }
    }
}
