using Azure.Identity;
using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateAccountModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }



        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            Username = "SampleUser";
            Email = "Sample@email.com";
            PhoneNumber = "80080080000";
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            return Page();
        }

        public void CreateAccount()
        {
            if (Username != null && Password != null && Email != null && PhoneNumber != null)
            {
                PersonalInfo PI = new PersonalInfo(Email, PhoneNumber, Username, Password);
                DatabaseControls.CreateAccount(PI);
            }
        }

        public IActionResult OnGet()
        {
           
            return Page();
        }

        public IActionResult OnPost()
        {
            CreateAccount();
            DatabaseControls.CreateHashedUser(Username, Password);
            HttpContext.Session.SetString("UserType", "");
            return RedirectToPage("/Index");
        }
    }
}
