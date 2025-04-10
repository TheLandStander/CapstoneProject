using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.ObjectPool;
//AI WAS USED TO IMPROVE THE UI STYLE 
namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateGrantModel : PageModel
    {
        [BindProperty] 
        public string GrantName { get; set; }

        [BindProperty]
        public string FundingAgency { get; set; }

        [BindProperty]
        public double Amount { get; set; }

        [BindProperty]
        public DateOnly DueDate { get; set; }

        [BindProperty]
        public string CategoryID { get; set; }

        [BindProperty]
        public GrantCategory[] Category { get; set; }

        public void RefreshSelection()
        {
            Category = ObjectConverter.ToGrantCategory(DatabaseControls.SelectNoFilter(5));
            DueDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public void CheckAddGrant()
        {
            if (GrantName != null && Amount > 0 &&
               DueDate >= DateOnly.FromDateTime(DateTime.Now) && CategoryID != null && FundingAgency != null)
            {
                Grant g = new Grant(GrantName,FundingAgency, (decimal)Amount, DatabaseControls.GetGrantStatus("InProgress").StatusID, CategoryID);
                g.DueDate = DueDate;
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
            DueDate = new DateOnly(1999, 3, 1);
            CategoryID = "1234567890";
            RefreshSelection();

            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            RefreshSelection();
            return Page();
        }

        public IActionResult OnPost()
        {
            CheckAddGrant();
            RefreshSelection();
            return RedirectToPage("Project-Dashboard");
        }
    }
}
