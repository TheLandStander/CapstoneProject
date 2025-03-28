using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreatePartner : PageModel
    {
        [BindProperty]
        public string BusinessName { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string StatusID { get; set; }

        public PartnerStatus[] Status { get; set; }

        public void AddPartner()
        {
            if (BusinessName != null && Address != null && Phone != null
                && Email != null && Description != null && StatusID != null)
            {
                Partner p = new Partner(BusinessName, Address, Phone, Email, Description, StatusID);
                DatabaseControls.Insert(p);
            }
        
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            BusinessName = "Default Business Name";
            Address = "1234 Default Address, City, Country";
            Phone = "123-456-7890";
            Email = "example@example.com";
            Description = "Default Description of the business.";
            StatusID = "1";
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectNoFilter(3));
            return Page();
        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectNoFilter(3));

                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectNoFilter(3));
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public void onPost()
        {
            Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectNoFilter(3));
        }
    }
}
