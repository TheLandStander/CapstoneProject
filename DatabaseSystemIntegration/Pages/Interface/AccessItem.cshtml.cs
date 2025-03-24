using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AccessItemModel : PageModel
    {

        [BindProperty]
        public string GrantStatusID { get; set; } 

        [BindProperty]
        public string ProjectStatusID { get; set; }

        [BindProperty]
        public DateOnly GrantAwardDate { get; set; }

        [BindProperty]
        public DateOnly ProjectEndDate { get; set; }

        [BindProperty]
        public GrantStatus[] Statuses { get; set; } = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(11));

        public string ItemType { get; set; }

        
        public string ItemID { get; set; }

     
        public BusProject BProject { get; set; }

     
        public Classes.Task[] Tasks { get; set; }


   
        public BusRep[] BusReps { get; set; }

      
        public BusPartner[] BusPartners { get; set; }

        
        public GrantProject GProject { get; set; }

       
        public FacultyProject[] FacultyProjects { get; set; }


        public EmployeeProject[] EmployeeProjects { get; set; }

        public Grant ThisGrant { get; set; }

        public Grant[] AssociatedGrants { get; set; }

   
        public ProjectNotes[] ProjectNotes { get; set; }

    
        public MeetingMinutes[] MeetingNotes { get; set; }

    
        public Faculty[] FacultyMembers { get; set; }
      
        public Employee[] Employees { get; set; }


        public void LoadGrant()
        {
            if (ItemType == "Grant")
            {
                ThisGrant = ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 10, ItemID))[0];
                GProject = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, ThisGrant.Grant_Project_ID))[0];
            }
        }



        public void LoadBusProject()
        {
            if (ItemType == "BusProject")
            {
                BProject = ObjectConverter.ToBusProject(DatabaseControls.SelectFilter(2, 2, ItemID))[0];
                ProjectNotes = ObjectConverter.ToProjectNotes(DatabaseControls.SelectFilter(15, 2, ItemID));
                Tasks = ObjectConverter.ToTask(DatabaseControls.SelectFilter(16, 2, BProject.Bus_Project_ID));
                EmployeeProjects = ObjectConverter.ToEmployeeProject(DatabaseControls.SelectFilter(5, 2, ItemID));
                List<Employee> EH = new List<Employee>();

                foreach (EmployeeProject ep in EmployeeProjects)
                {
                    EH.Add(ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 4, ep.Employee_ID))[0]);
                }

                Employees = EH.ToArray();
            }
        }

        public void LoadGrantProject()
        {
            if (ItemType == "GrantProject")
            {
                GProject = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, ItemID))[0];
                AssociatedGrants = ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 9, ItemID));
                FacultyProjects = ObjectConverter.ToFacultyProject(DatabaseControls.SelectFilter(7, 9, ItemID));
                List<BusRep> RepHolder = new List<BusRep>();
                List<BusPartner> PartnerHolder = new List<BusPartner>();
                List<MeetingMinutes> MMHolder = new List<MeetingMinutes>();
                List<Faculty> FacultyHolder = new List<Faculty>();

                foreach (FacultyProject fp in FacultyProjects)
                {
                    FacultyHolder.Add(ObjectConverter.ToFaculty(DatabaseControls.SelectFilter(6, 6, fp.Faculty_ID))[0]);

                }

                foreach (Grant G in AssociatedGrants)
                {
                   PartnerHolder.Add(ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0,0,G.Bus_Partner_ID))[0]);

                }

                foreach (BusPartner BP in PartnerHolder)
                {
                    if (DatabaseControls.SelectFilter(3, 0, BP.Bus_Partner_ID).HasRows)
                    {
                        RepHolder.Add(ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 0, BP.Bus_Partner_ID))[0]);
                    }
                }

                foreach (BusRep BR in RepHolder)
                {
                    MeetingMinutes[] Holder = ObjectConverter.ToMeetingMinutes(DatabaseControls.SelectFilter(12, 3, BR.Bus_Rep_ID));
                    if (Holder.Length > 0)
                    {
                        MMHolder.Add(Holder[0]);
                    }
                }

                BusPartners = PartnerHolder.ToArray();
                BusReps = RepHolder.ToArray();
                MeetingNotes = MMHolder.ToArray();
                FacultyMembers = FacultyHolder.ToArray();
            }
        }
        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateProject()
        {
            SetVars();
            if (ProjectEndDate > BProject.Start_Date)
            {
                DatabaseControls.UpdateBusProject(ItemID, ProjectEndDate);
            }
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostUpdateGrant()
        {
            SetVars();
            if (ThisGrant.Award_Date > ThisGrant.Submission_Date)
            {
                DatabaseControls.UpdateGrantAwardDate(ItemID, GrantAwardDate);
            }
            return RedirectToPage("AccessItem");
        }
        public IActionResult OnPostUpdateGrantStatus()
        {
            SetVars();
            DatabaseControls.UpdateGrantStatus(ItemID, GrantStatusID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrantProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "GrantProject");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public void SetVars()
        {
            ItemType = HttpContext.Session.GetString("ItemType");
            ItemID = HttpContext.Session.GetString("ItemID");
            Statuses = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(11));
            LoadGrant();
            LoadGrantProject();
            LoadBusProject();
        }

        public void OnGet()
        {
            SetVars();
            GrantAwardDate = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            ProjectEndDate = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        public void OnPost()
        {
            SetVars();

        }
    }
}
