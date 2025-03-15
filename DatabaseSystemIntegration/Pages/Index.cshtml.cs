using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Http;
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
            }

            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                //checks for a valid login
                string ID = HttpContext.Session.GetString("AccountID");
                HttpContext.Session.SetString("UserName",username);
                //check if the ID returns anything in the other tables (table 0 = buspartner, field zero is the primary key)
                //the second parameter is the field name i am searching for, for example, field 14 is the "Info_ID" colunn,
                //IT DOES NOT REPREENT THE COLUMN NUMBER, ONLY A STRING COLUMN TITLE
                if (DatabaseControls.SelectFilter(0, 14, ID).HasRows)
                {
                    HttpContext.Session.SetString("UserType", "BusPartner");
                    return RedirectToPage("Interface/Project-Dashboard");
                }

                else if (DatabaseControls.SelectFilter(4, 14, ID).HasRows)
                {
                    //need to check if this is an Employee or Admin...
                    SqlDataReader EmployeeRecord = DatabaseControls.SelectFilter(4, 14, ID);
                    EmployeeRecord.Read();
                    //gets the field that hold the admin true/false, true =1, false = 0
                    if (EmployeeRecord.GetValue(2).ToString() == "True")
                    {
                        HttpContext.Session.SetString("UserType", "Admin");
                        return RedirectToPage("Interface/Project-Dashboard");
                    }
                    else
                    {
                        HttpContext.Session.SetString("UserType", "Employee");
                        return RedirectToPage("Interface/Project-Dashboard");
                    }
                }

                else if (DatabaseControls.SelectFilter(6, 14, ID).HasRows)
                {
                    HttpContext.Session.SetString("UserType", "Faculty");
                    return RedirectToPage("Interface/Project-Dashboard");
                }

                else if (DatabaseControls.SelectFilter(3,14, ID).HasRows)
                {
                    HttpContext.Session.SetString("UserType", "BusRep");
                    return RedirectToPage("Interface/Project-Dashboard");
                }
                else
                {
                    return Page();
                }
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
