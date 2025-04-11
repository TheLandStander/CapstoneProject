using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class HomeModel : PageModel
    {
        public Users User { get; set; }

        [BindProperty]
        public string Search { get; set; }

        public Tasks[] DisplayedTasks { get; set; }

        public ChildTask[] DisplayedSubTasks { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public void SetObjects()
        {
            if (User == null)
            {
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
            }
            if (DisplayedTasks == null)
            {
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            }
            if (DisplayedSubTasks == null)
            {
                DisplayedSubTasks = DatabaseControls.GetUserSubTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
            }
        }

        public ChildTask[] SearchTasks(string Name)
        {
            ChildTask[] AllTasks = DatabaseControls.GetUserSubTasks(User.UserID);
            List<ChildTask> SearchResults = new List<ChildTask>();
            foreach (ChildTask t in AllTasks)
            {
                if (t.TaskName == Name && t.Completed == false)
                {
                    SearchResults.Add(t);
                }
            }            

            return SearchResults.OrderBy(t => t.DueDate).ToArray();
        }

        public IActionResult OnPostSearch()
        {
            if (Search != null)
            {
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedSubTasks = SearchTasks(Search);
            }
            else
            {
                DisplayedTasks = DatabaseControls.GetUserTasks(User.UserID).OrderBy(p => p.DueDate).ToArray();
                User = DatabaseControls.GetUser(HttpContext.Session.GetString("UserID"));
                DisplayedSubTasks = DatabaseControls.GetUserSubTasks(User.UserID).OrderBy(t => t.DueDate).ToArray();
            }

            return Page();
        }

        public void OnPost()
        {
            SetObjects();
        }

        public async Task<IActionResult> OnPostDownloadPdf(string id, string name)
        {
            SetObjects();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();

            return File(fileStream, "application/pdf", $"{name}.pdf");
        }

        public async Task<IActionResult> OnPostDownloadDoc(string id, string name)
        {
            SetObjects();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();
            return File(fileStream, "application/msword", $"{name}.docx");
        }

        public async Task<IActionResult> OnPostDownloadExel(string id, string name)
        {
            SetObjects();
            var fileStream = await FileManager.RetrieveFileAsync(id);
            if (fileStream == null) return NotFound();

            return File(fileStream, "application/vnd.ms-excel", $"{name}.xls");
        }

        public async Task<IActionResult> OnPostUpload(string id)
        {
            SetObjects();
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                return Page();
            }
            using var stream = UploadedFile.OpenReadStream();
            await FileManager.UploadFileAsync(id, stream);
            return Page();
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
        public IActionResult OnPostUpdateChildTask(string id)
        {
            CompleteChildTask(id);
            SetObjects();
            return Page();
        }


    }
}
