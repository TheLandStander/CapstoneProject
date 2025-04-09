using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace DatabaseSystemIntegration.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string password { get; set; }

        [BindProperty]
        public string username { get; set; }

        public Users User { get; set; }

        public void OnGet()
        {
            HttpContext.Session.SetInt32("LoggedIn", 0);
            HttpContext.Session.SetString("AccountID", "");
        }

        public IActionResult OnPost()
        {
            /*If there is an account with the provided credentials, go into the personal info table and set the account ID
            Passwords were removed from the personalinfo table, and are now stored in the HashedCredentials table
            when an account is created, a personal inffo recorded is created first, then a credential record that references the personal infoID
            */
            if (DatabaseControls.HashedParameterLogin(username, password) == true)
            {
                HttpContext.Session.SetString("AccountID", DatabaseControls.GetHashedAccount(username, password));
                string ID = HttpContext.Session.GetString("AccountID");
                if (DatabaseControls.SelectFilter(19, 11, ID).HasRows)
                {
                    HttpContext.Session.SetInt32("LoggedIn", 1);
                    User = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 11, ID))[0];
                    HttpContext.Session.SetString("UserID", User.UserID);
                    HttpContext.Session.SetString("UserName", User.Name);
                    HttpContext.Session.SetString("UserType", User.type.UserTypeName);
                }
                return RedirectToPage("/Interface/Home");
            }

            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }


        }
        
    }
}
