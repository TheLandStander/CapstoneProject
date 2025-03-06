using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class IndexModel : PageModel
    {
        //creates the objects that the user will be selecting in the drop down
        [BindProperty]
        public PersonalInfo[] UserInfo { get; set; } = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));

        [BindProperty]
        public BusPartnerStatus[] Status { get; set; } = ObjectConverter.ToBusPartnerStatus(DatabaseControls.SelectNoFilter(1));

        [BindProperty]
        public OrgType[] Orgs { get; set; } = ObjectConverter.ToOrgType(DatabaseControls.SelectNoFilter(13));

        [BindProperty]
        public BusPartner[] Partners { get; set; } = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));

        [BindProperty]
        public string Rep_Name { get; set; } = "";

        [BindProperty]
        public string Partner_ID { get; set; } = "";

        [BindProperty]
        public string Rep_Info_ID { get; set; } = "";

        //details for making an employee
        [BindProperty]
        public string Employee_Name { get; set; } = "";

        [BindProperty]
        public bool is_Admin { get; set; }

        [BindProperty]
        public string Employee_Info_ID { get; set; } = "";


        //details for making a faculty
        [BindProperty]
        public string Faculty_Name { get; set; } = "";

        [BindProperty]
        public string Faculty_Info_ID { get; set; } = "";

        //details for making a business partner
        [BindProperty]
        public string Bus_Name { get; set; } = "";

        [BindProperty]
        public string Org_Type_ID { get; set; } = "";

        [BindProperty]
        public string Partner_Status_ID { get; set; } = "";

        [BindProperty]
        public string Partner_Info_ID { get; set; } = "";

        //checks if the Form was complete and then adds it to the list & into the database
        public void CheckAddEmployee()
        {
            Console.Write(is_Admin);
            if (Employee_Name != "" && Employee_Info_ID != "")
            {
                Employee E = new Employee(Employee_Name,is_Admin, Employee_Info_ID);
                DatabaseControls.InsertEmployee(E);
            }
        
        }

        public void CheckAddFaculty()
        {
            if (Faculty_Name != "" && Faculty_Info_ID != "")
            {
                Faculty F = new Faculty(Faculty_Name, Faculty_Info_ID);
                DatabaseControls.InsertFaculty(F);
            }
        
        }

        public void CheckAddBusPartner()
        {
            if (Bus_Name != "" && Org_Type_ID != "" && Partner_Status_ID != "" && Partner_Info_ID != "")
            {

                BusPartner BS = new BusPartner(Bus_Name, Partner_Status_ID, Org_Type_ID, Partner_Info_ID);
                DatabaseControls.InsertBusPartner(BS);
            }
        }

        public void CheckAddBusRep()
        {
            if (Rep_Name != "" && Partner_ID != "")
            {

                BusRep BR = new BusRep(Rep_Name,Partner_ID, Rep_Info_ID);
                DatabaseControls.InsertBusRep(BR);
            }
        }

        public void RefreshSelection()
        {
            //refreshes selection options after the user adds a record when they load the page
        UserInfo = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectNoFilter(14));
        Status = ObjectConverter.ToBusPartnerStatus(DatabaseControls.SelectNoFilter(1));
        Orgs = ObjectConverter.ToOrgType(DatabaseControls.SelectNoFilter(13));
        Partners = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
        
        }
        public void OnPost()
        {
            CheckAddBusPartner();
            CheckAddEmployee();
            CheckAddFaculty();
            CheckAddBusRep();
            RefreshSelection();
        }

        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();
            RefreshSelection();
            Rep_Name = "Sample representative";
            Employee_Name = "Sample Employee";
            Faculty_Name = "Sample Faculty";
            Bus_Name = "Sample Partner";
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
