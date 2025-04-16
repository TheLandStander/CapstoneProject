using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//AI WAS USED TO IMPROVE THE UI STYLE
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
        public string GrantName { get; set; }

        [BindProperty]
        public string ProjectName { get; set; }

        [BindProperty]
        public string ProjectDesc { get; set; }

        [BindProperty]
        public string TaskDesc { get; set; }

        [BindProperty]
        public string GrantDesc { get; set; }

        [BindProperty]
        public string SubTaskDesc { get; set; }

        [BindProperty]
        public string TaskName { get; set; }

        [BindProperty]
        public string SubTaskName { get; set; }

        [BindProperty]
        public string NoteTo { get; set; }

        [BindProperty]
        public DateOnly Date { get; set; }

        [BindProperty]
        public DateOnly UpdatedDate { get; set; }

        [BindProperty]
        public string User_ID { get; set; }

        [BindProperty]
        public string User_ID2 { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public string ItemType { get; set; }
        public string ItemID { get; set; }
        public Grant grant { get; set; }
        public Project project { get; set; }
        public Partner partner { get; set; }
        public GrantStatus[] GrantStatus { get; set; }
        public ProjectStatus[] ProjectStatus { get; set; }

        public Users CurrentUser { get; set; }

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

        public void AssignChildTask(string ID,string ManAssign = "")
        {
            ChildTask = ObjectConverter.ToChildTask(DatabaseControls.SelectFilter(4, 4, ID))[0];
            if (ManAssign != "")
            {
                ChildTask.AssignTask(ManAssign);
            }
            else
            {
                ChildTask.AssignTask(HttpContext.Session.GetString("UserID"));
            }
                
        }

        public async Task<IActionResult> OnPostDownloadPdf(string id, string name)
        {
            LoadObjects();
            SetVars();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();

            return File(fileStream, "application/pdf", $"{name}.pdf");
        }

        public async Task<IActionResult> OnPostDownloadDoc(string id, string name)
        {
            LoadObjects();
            SetVars();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();
            return File(fileStream, "application/msword", $"{name}.docx");
        }

        public async Task<IActionResult> OnPostDownloadExel(string id, string name)
        {
            LoadObjects();
            SetVars();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();

            return File(fileStream, "application/vnd.ms-excel", $"{name}.xls");
        }

        public async Task<IActionResult> OnPostUpload(string id)
        {
            LoadObjects();
            SetVars();
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                 return RedirectToPage("AccessItem");
            }
            using var stream = UploadedFile.OpenReadStream();
            await FileManager.UploadFileAsync(id, stream);
             return RedirectToPage("AccessItem");
        }
        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            LoadObjects(); 
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Project");
            HttpContext.Session.SetString("ItemID", ID);
            LoadObjects();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectTask(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Task");
            HttpContext.Session.SetString("ItemID", ID);
            LoadObjects();
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
            LoadObjects();
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAssignTask()
        {
            LoadObjects();
            ParentTask.AssignTask(User_ID);
            SetVars();
             return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAssignChildTask(string ID)
        {
            LoadObjects();
            if (ItemType == "Grant")
            {
                AssignChildTask(ID, User_ID);
            }
            else
            {
                AssignChildTask(ID, HttpContext.Session.GetString("UserID"));
            }
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

        public IActionResult OnPostCreateProject(string ID)
        {
            LoadObjects();
            Project P = new Project(ProjectName, ProjectDesc, DateOnly.FromDateTime(DateTime.Now), Date, DatabaseControls.GetProjectStatus("Ongoing").ProjectStatusID, ID);
            DatabaseControls.Insert(P);
            return RedirectToPage("Project-Dashboard");
        }

        public IActionResult OnPostUpdateProjectLead()
        {
            LoadObjects();
            project.SetProjectLeader(User_ID2);
            SetVars();
             return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddNote(string ID)
        {
            LoadObjects();
            ProjectNotes N = new ProjectNotes(Note, CurrentUser.Name, NoteTo, DateOnly.FromDateTime(DateTime.Now), ID);
            DatabaseControls.Insert(N);
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
            if (DateOnly.FromDateTime(DateTime.Now) <= Date)
            {
                project.AddTask(TaskName, TaskDesc, Date);
            }
            SetVars();
             return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostAddChildTask(string ID)
        {
            LoadObjects();
            if (DateOnly.FromDateTime(DateTime.Now) <= Date)
            {
                ChildTask CT = new ChildTask(SubTaskName, SubTaskDesc, DateOnly.FromDateTime(DateTime.Now),Date, false, ID);
                DatabaseControls.Insert(CT);
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
            LoadObjects();
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
            UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public void LoadObjects()
        {
            ItemType = HttpContext.Session.GetString("ItemType");
            ItemID = HttpContext.Session.GetString("ItemID");
            Users = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            CurrentUser = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
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
