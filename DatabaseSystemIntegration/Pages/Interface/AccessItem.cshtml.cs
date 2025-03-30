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

        [BindProperty]
        public string PartnerStatusID { get; set; }

        [BindProperty]
        public string Note { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public DateOnly Date { get; set; }

        [BindProperty]
        public DateOnly Date2 { get; set; }

        [BindProperty]
        public DateOnly UpdatedDate { get; set; }

        [BindProperty]
        public string User_ID { get; set; }

        [BindProperty]
        public string User_ID2 { get; set; }

        public string ItemType { get; set; }
        public string ItemID { get; set; }
        public Grant grant { get; set; }
        public Project project { get; set; }
        public Partner partner { get; set; }
        public GrantStatus[] GrantStatus { get; set; }
        public ProjectStatus[] ProjectStatus { get; set; }

        public PartnerStatus[] PartnerStatus { get; set; }
        public Users[] Users { get; set; }

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
                Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            }
        }

        public void LoadPartner()
        {
            if (ItemType == "Partner")
            {
                partner = ObjectConverter.ToPartner(DatabaseControls.SelectFilter(2, 2, ItemID))[0];
                PartnerStatus = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectNoFilter(3));
            }
        }

        public void LoadTask()
        {
            if (ItemType == "Task")
            {
                ParentTask = ObjectConverter.ToTask(DatabaseControls.SelectFilter(15, 15, ItemID))[0];
            }
        }

        public void CompleteChildTask(string ID)
        {
                ChildTask = ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 4, ID))[0];
                ChildTask.CompleteTask();
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

        public IActionResult OnPostSelectPartner(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Partner");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAssignTask()
        {
            LoadObjects();
            ParentTask.AssignTask(HttpContext.Session.GetString("UserID"));
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAssignProject()
        {
            LoadObjects();
            project.AssignProject(User_ID);
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateProjectLead()
        {
            LoadObjects();
            project.SetProjectLeader(User_ID2);
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddNote()
        {
            LoadObjects();
            project.AddNote(Note);
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddMeeting()
        {
            LoadObjects();
            partner.AddMeeting(Note, Date);
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddTask()
        {
            LoadObjects();
            if (Date < Date2)
            {
                project.AddTask(Name, Description, Date, Date2);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddChildTask()
        {
            LoadObjects();
            if (Date < Date2)
            {
                ParentTask.AddChildTask(Name, Description, Date, Date2);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateTask()
        {
            LoadObjects();
            ParentTask.CompleteTask();
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateChildTask(string ID)
        {
            LoadObjects();
            CompleteChildTask(ID);
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdatePartner()
        {
            LoadObjects();
            if (PartnerStatusID != null)
            {
                partner.UpdateStatus(PartnerStatusID);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateProject()
        {
            LoadObjects();
            if (ProjectStatusID != null)
            {
                project.UpdateStatus(ProjectStatusID);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateGrant()
        {
            LoadObjects();
            if (GrantStatusID != null)
            {
                grant.UpdateStatus(GrantStatusID);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateProjectDate()
        {
            LoadObjects();
            if (UpdatedDate != DateOnly.MinValue)
            {
                project.UpdateDueDate(UpdatedDate);
            }
            SetVars();
            return RedirectToPage("AccessItem");
        }

        public void SetVars()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);
            Date2 = DateOnly.FromDateTime(DateTime.Now);
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public void LoadObjects()
        {
            ItemType = HttpContext.Session.GetString("ItemType");
            ItemID = HttpContext.Session.GetString("ItemID");
            LoadGrant();
            LoadProject();
            LoadTask();
            LoadPartner();
        }

        public void OnGet()
        {
            LoadObjects();
            SetVars();
        }

        public void OnPost()
        {
            LoadObjects();
            SetVars();

        }
    }
}
