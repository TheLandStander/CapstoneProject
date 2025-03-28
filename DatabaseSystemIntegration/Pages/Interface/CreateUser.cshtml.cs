using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Identity.Client;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class CreateUser : PageModel
    {
        [BindProperty]
        public string AccountID { get; set; }

        [BindProperty]
        public string PartnerID { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string UserTypeID { get; set; }

        [BindProperty]
        public string RoleID { get; set; }

        public Partner[] Partners { get; set; }

        public PersonalInfo[] Accounts {get;set;}

        public UserType[] Types { get; set; }


        public Role[] Roles { get; set; }


        public void RefreshSelection()
        {
            Partners = ObjectConverter.ToPartner(DatabaseControls.SelectNoFilter(2));
            Accounts = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(11));
            Types = ObjectConverter.ToUserType(DatabaseControls.SelectNoFilter(18));
            Roles = ObjectConverter.ToRole(DatabaseControls.SelectNoFilter(10));

        }

        public void AddUser()
        {
            if (Name != null && UserTypeID != null && RoleID != null && AccountID != null && Name.Length != 0)
            {
                Users U = new Users(Name, UserTypeID, DatabaseControls.GetUserStatus("Active").UserStatusID, AccountID);
                UserRole ur = new UserRole(U.UserID, RoleID);
                DatabaseControls.Insert(U);
                DatabaseControls.Insert(ur);

            }
        
        }

        public void OnPost()
        {
            AddUser();
            RefreshSelection();
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            AccountID = "123456";   
            PartnerID = "123456"; 
            Name = "Default User Name";  
            UserTypeID = "123456";
            return Page();
        }

        public IActionResult OnPostClearHandler()
        {
            ModelState.Clear();
            RefreshSelection();

            return Page();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    RefreshSelection();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }
    }
}
