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
        //For the selection lists, wont be modified 
        public Faculty[] LeadFaculty = ObjectConverter.ToFaculty(DatabaseControls.SelectNoFilter(6));
        public GrantCategory[] GrantCategories = ObjectConverter.ToGrantCategory(DatabaseControls.SelectNoFilter(8));
        public GrantStatus[] Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(11));
        public BusPartner[] Partners = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
        public GrantProject[] GrantProjectsSelection = ObjectConverter.ToGrantProject(DatabaseControls.SelectNoFilter(9));



        //Fields needed to create a Grant_Project (projects hold grants, this must be created first)
        [BindProperty]
        public string Project_Name { get; set; } = "";

        [BindProperty]
        public string Description { get; set; } = "";


        //Fields needed to create a grant
        [BindProperty]
        public string Grant_Name { get; set; } = "";

        [BindProperty]
        public double Amount { get; set; }

        [BindProperty]
        public DateOnly Submission_Date { get; set; }

        [BindProperty]
        public DateOnly Award_Date { get; set; }

        [BindProperty]
        public string Grant_Project_ID { get; set; } = "";

        [BindProperty]
        public string Lead_Faculty_ID { get; set; } = "";

        [BindProperty]
        public string Status_ID { get; set; } = "";

        [BindProperty]
        public string Category_ID { get; set; } = "";

        [BindProperty]
        public string Bus_Partner_ID { get; set; } = "";

        public void CheckAddGrantProject()
        {
            if (Project_Name != "" && Description != "")
            {
                GrantProject GP = new GrantProject(Project_Name,Description);
                DatabaseControls.InsertGrantProject(GP);
            }
        }

        public void CheckAddGrant()
        {
           
            if (Grant_Name != "" && Amount != 0 && DateOnly.TryParse(Submission_Date.ToString(), out DateOnly result) != false
                && DateOnly.TryParse(Award_Date.ToString(), out DateOnly result1) != false && Lead_Faculty_ID != ""
                && Status_ID != "" && Category_ID != "" && Bus_Partner_ID != "" && Grant_Project_ID != "")
            {
                Grant G = new Grant(Grant_Name, Amount, 
                DateOnly.ParseExact(Submission_Date.ToString(),"M/d/yyyy"), DateOnly.ParseExact(Award_Date.ToString(),"M/d/yyyy"),
                Lead_Faculty_ID, Status_ID, Category_ID, Bus_Partner_ID,Grant_Project_ID);
                DatabaseControls.InsertGrant(G);

            }
        }

        public void RefreshSelection()
        {
            //refereshes values after user finishes creating a record or loading the page
            LeadFaculty = ObjectConverter.ToFaculty(DatabaseControls.SelectNoFilter(6));
            GrantCategories = ObjectConverter.ToGrantCategory(DatabaseControls.SelectNoFilter(8));
            Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(11));
            Partners = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            GrantProjectsSelection = ObjectConverter.ToGrantProject(DatabaseControls.SelectNoFilter(9));
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin" || HttpContext.Session.GetString("UserType") == "Employee" || HttpContext.Session.GetString("UserType") == "Faculty")
                {

                    return Page();
                }
            }
            return new RedirectToPageResult("/Index");
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Grant_Name = "Sample Grant";
            Amount = 1000;
            Submission_Date = new DateOnly(1999, 1, 1);
            Award_Date = new DateOnly(2025, 12, 1);
            Project_Name = "Sample Project";
            Description = "Sample Project Description";
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
            CheckAddGrantProject();
            RefreshSelection();
        }
    }
}
