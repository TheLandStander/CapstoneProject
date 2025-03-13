using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.ObjectPool;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class SearchModel : PageModel
    {

        public string[] Filters { get; set; } = {"Grant Award Date", "Grant Submission Date","Grant Status",
                                                 "Business Partner Status", "Organization Type", "Projects Due",
                                                  "Project Start Date","Project Due Date", "Project End Date", "Lead Faculty"};
        [BindProperty]
        public List<string> SelectedFilter { get; set; } = new List<string>();

        [BindProperty]
        public DateOnly Start { get; set; }

        [BindProperty]
        public DateOnly Due { get; set; }
        [BindProperty]
        public DateOnly End { get; set; }
        [BindProperty]
        public DateOnly Submission { get; set; }
        [BindProperty]
        public DateOnly Award { get; set; }

        [BindProperty]
        public string GStatus { get; set; }

        [BindProperty]
        public string PStatus { get; set; }

        [BindProperty]
        public string OrgType { get; set; }

        [BindProperty]
        public string GrantCategory { get; set; }

        [BindProperty]
        public string LeadFacultyID { get; set; }


        public GrantStatus[] GrantStatus { get; set; }

        public GrantCategory[] GrantCategories { get; set; }

        public BusPartnerStatus[] PartnerStatus { get; set; }

        public OrgType[] OrgTypes { get; set; }

        public GrantProject[] GrantProjects { get; set; }

        public BusProject[] BusProjects { get; set; }
        public Grant[] Grants { get; set; }

        public BusPartner[] Partners { get; set; }

        public Faculty[] LeadFaculty { get; set; }

        public BusRep[] Reps { get; set; }


        public void SetVars()
        {
            GrantCategories = ObjectConverter.ToGrantCategory(DatabaseControls.SelectNoFilter(8));
            GrantStatus = ObjectConverter.ToGrantStatus(DatabaseControls.SelectNoFilter(11));
            PartnerStatus = ObjectConverter.ToBusPartnerStatus(DatabaseControls.SelectNoFilter(1));
            OrgTypes = ObjectConverter.ToOrgType(DatabaseControls.SelectNoFilter(13));
            BusProjects = ObjectConverter.ToBusProject(DatabaseControls.SelectNoFilter(2));
            Partners = ObjectConverter.ToBusPartner(DatabaseControls.SelectNoFilter(0));
            Grants = ObjectConverter.ToGrant(DatabaseControls.SelectNoFilter(10));
            GrantProjects = ObjectConverter.ToGrantProject(DatabaseControls.SelectNoFilter(9));
            Reps = ObjectConverter.ToBusRep(DatabaseControls.SelectNoFilter(3));
            LeadFaculty = DatabaseControls.GetLeadFaculty();
            Award = DateOnly.FromDateTime(DateTime.Now);
            Submission = DateOnly.FromDateTime(DateTime.Now);
            Start = DateOnly.FromDateTime(DateTime.Now);
            End = DateOnly.FromDateTime(DateTime.Now);
            Due = DateOnly.FromDateTime(DateTime.Now);

        }

        public void SetReps()
        {
            List<BusRep> holder = new List<BusRep>();

            foreach (BusPartner bp in Partners)
            {
                holder.AddRange(ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 0, bp.Bus_Partner_ID)));
            }
            Reps = holder.ToArray();
        }


        public void FilterAwardDate(DateOnly AwardDate)
        {
           
          List<Grant> GHolder = new List<Grant>();
            foreach (Grant g in Grants)
            {
                if (g.Award_Date == AwardDate)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();

        }

        public void FilterSubmissionDate(DateOnly SubmissionDate)
        {

            List<Grant> GHolder = new List<Grant>();
            foreach (Grant g in Grants)
            {
                if (g.Submission_Date == SubmissionDate)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();

        }

        public void FilterGrantStatus(string Status)
        {

            List<Grant> GHolder = new List<Grant>();
            foreach (Grant g in Grants)
            {
                if (g.Grant_Status == Status)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();

        }

        public void FilterPartnerStatus(string Status)
        {
            List<BusPartner> Holder = new List<BusPartner>();
            foreach (BusPartner p in Partners)
            {
                if (p.Status_Flag == Status)
                {
                    Holder.Add(p);
                }

            }
            Partners = Holder.ToArray();
            SetReps();
        }

        public void FilterPartnerOrgType(string OrgType)
        {
            List<BusPartner> Holder = new List<BusPartner>();
            foreach (BusPartner p in Partners)
            {
                if (p.Org_Type == OrgType)
                {
                    Holder.Add(p);
                }

            }
            Partners = Holder.ToArray();
            SetReps();
        }

        public void FilterProjectsDue()
        {
            List<BusProject> Holder = new List<BusProject>();
            foreach (BusProject bp in BusProjects)
            {
                if (DateOnly.FromDateTime(DateTime.Now) > bp.Due_Date)
                {
                    Holder.Add(bp);
                }

            }
            BusProjects = Holder.ToArray();
        }

        public void FilterProjectDueDate(DateOnly DueDate)
        {
            List<BusProject> Holder = new List<BusProject>();
            foreach (BusProject bp in BusProjects)
            {
                if (bp.Due_Date == DueDate)
                {
                    Holder.Add(bp);
                }

            }
            BusProjects = Holder.ToArray();
        }

        public void FilterProjectStartDate(DateOnly StartDate)
        {
            List<BusProject> Holder = new List<BusProject>();
            foreach (BusProject bp in BusProjects)
            {
                if (bp.Start_Date == StartDate)
                {
                    Holder.Add(bp);
                }

            }
            BusProjects = Holder.ToArray();
        }

        public void FilterProjectEndDate(DateOnly EndDate)
        {
            List<BusProject> Holder = new List<BusProject>();
            foreach (BusProject bp in BusProjects)
            {
                if (bp.End_Date == EndDate)
                {
                    Holder.Add(bp);
                }

            }
            BusProjects = Holder.ToArray();
        }

        public void FilterGrantLeadFaculty(string FacultyID)
        {

            List<Grant> GHolder = new List<Grant>();
            foreach (Grant g in Grants)
            {
                if (g.Lead_Faculty_ID == FacultyID)
                {
                    GHolder.Add(g);
                }

            }
            Grants = GHolder.ToArray();

        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                
                if (HttpContext.Session.GetString("UserType") == "Admin")
                {
                    SetVars();
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public void Load()
        {
            SetVars();
            if (SelectedFilter.Contains("Grant Award Date"))
            {
                if (Award != DateOnly.MinValue)
                {
                    FilterAwardDate(Award);
                }
            }

            if (SelectedFilter.Contains("Grant Submission Date"))
            {
                if (Submission != DateOnly.MinValue)
                {
                    FilterSubmissionDate(Submission);
                }
            }

            if (SelectedFilter.Contains("Grant Status"))
            {
                if (GStatus != "")
                {
                    FilterGrantStatus(GStatus);
                }
            }

            if (SelectedFilter.Contains("Business Partner Status"))
            { 
                if(PStatus != "")
                {
                    FilterPartnerStatus(PStatus);
                }
            
            }

            if (SelectedFilter.Contains("Organization Type"))
            {
                if (OrgType != "")
                {
                    FilterPartnerOrgType(OrgType);
                }
            
            }

            if (SelectedFilter.Contains("Project Due Date"))
            {
                if (Due != DateOnly.MinValue)
                {
                    FilterProjectDueDate(Due);
                }
            
            }

            if (SelectedFilter.Contains("Project Start Date"))
            {
                if (Start != DateOnly.MinValue)
                {
                    FilterProjectDueDate(Start);
                }
            }

            if (SelectedFilter.Contains("Project Due Date"))
            {

                if (Due != DateOnly.MinValue)
                {
                    FilterProjectDueDate(Due);
                }
            }

            if (SelectedFilter.Contains("Project End Date"))
            {
                if (End != DateOnly.MinValue)
                {
                    FilterProjectEndDate(End);
                }
            
            }

            if (SelectedFilter.Contains("Lead Faculty"))
            {
                if (LeadFacultyID != null)
                {
                    FilterGrantLeadFaculty(LeadFacultyID);
                }
            
            }

        }


        public IActionResult OnPost()
        {
            Load();
            return Page();
        }

        public IActionResult OnPostSelectBusProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "BusProject");
            HttpContext.Session.SetString("ItemID", ID);

            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrantProject(string ID)
        {
            HttpContext.Session.SetString("ItemType", "GrantProject");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectGrant(string ID)
        {
            HttpContext.Session.SetString("ItemType", "Grant");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectProjectNote(string ID)
        {
            HttpContext.Session.SetString("ItemType", "ProjectNote");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

        public IActionResult OnPostSelectMeetingNote(string ID)
        {
            HttpContext.Session.SetString("ItemType", "MeetingNote");
            HttpContext.Session.SetString("ItemID", ID);
            return RedirectToPage("AccessItem");
        }

    }
}
