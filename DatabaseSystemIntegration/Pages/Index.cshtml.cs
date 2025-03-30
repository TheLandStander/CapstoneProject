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
            if (DatabaseControls.SecureLogIn(username, password) == 1)
            {
                CheckLogin(DatabaseControls.LogIn(username, password));
                DatabaseControls.HashedParameterLogin(username, password);
            }

            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                //checks for a valid login
                string ID = HttpContext.Session.GetString("AccountID");
                User = ObjectConverter.ToUsers(DatabaseControls.SelectFilter(19, 11, ID))[0];
                HttpContext.Session.SetString("UserID",User.UserID);
                HttpContext.Session.SetString("UserName", User.Name);
                HttpContext.Session.SetString("UserType", User.type.UserTypeName);
                return RedirectToPage("Interface/Project-Dashboard");
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }


        }


        private bool CheckLogin(SqlDataReader results)
        {

            if (results.HasRows)
            {
                results.Read(); //move to the first (and only) row, get the info_id from the first column
                HttpContext.Session.SetInt32("LoggedIn", 1);
                HttpContext.Session.SetString("AccountID", results.GetValue(0).ToString());
                return true;
            }
            else
            {
                HttpContext.Session.SetInt32("LoggedIn", 0);
                return false;
            }
        }

    }
}
