using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.ObjectPool;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateGrantModel : PageModel
    {
        [BindProperty] 
        public string GrantName { get; set; }

        [BindProperty]
        public double Amount { get; set; }

        [BindProperty]
        public DateOnly SubDate { get; set; }

        [BindProperty]
        public DateOnly DueDate { get; set; }

        [BindProperty]
        public string StatusID { get; set; }

        [BindProperty]
        public string CategoryID { get; set; }

        [BindProperty]
        public string ProjectID { get; set; }

        [BindProperty]
        public Project[] Projects { get; set; }

        [BindProperty]
        public GrantStatus[] Status { get; set; }

        [BindProperty]
        public GrantCategory[] Category { get; set; }

        public void RefreshSelection()
        {
            Projects = ObjectConverter.ToProject(DatabaseControls.SelectNoFilter(12));
            Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(6));
            Category = ObjectConverter.ToGrantCategory(DatabaseControls.SelectNoFilter(5));
        }

        public void CheckAddGrant()
        {
            if (GrantName != null && Amount > 0 && SubDate != DateOnly.MinValue &&
               DueDate > SubDate && StatusID != null && CategoryID != null && ProjectID != null)
            {
                Grant g = new Grant(GrantName, (decimal)Amount, SubDate, DueDate, StatusID, CategoryID, ProjectID);
                DatabaseControls.Insert(g);
            }
        }

       
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                RefreshSelection();
                    return Page();
            }
            return new RedirectToPageResult("/Index");
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            GrantName = "Sample Grant";
            Amount = 1000;
            SubDate = new DateOnly(1999, 1, 1);
            DueDate = new DateOnly(1999, 3, 1);
            StatusID = "123456";
            ProjectID = "123456";
            CategoryID = "123456";
            RefreshSelection();

            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            RefreshSelection();
            return Page();
        }

        public void OnPost()
        {
            CheckAddGrant();
            RefreshSelection();
        }
    }
}
