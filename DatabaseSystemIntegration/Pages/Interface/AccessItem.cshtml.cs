using DatabaseSystemIntegration.Pages.Classes;
using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace DatabaseSystemIntegration.Pages.Interface
{
    public class AccessItemModel : PageModel
    {
        [BindProperty]
        public string ItemType { get; set; }

        [BindProperty]
        public string ItemID { get; set; }

        [BindProperty]
        public BusProject BProject { get; set; }

        [BindProperty]
        public BusRep[] BusReps { get; set; }

        [BindProperty]
        public BusPartner[] BusPartners { get; set; }

        [BindProperty]
        public GrantProject GProject { get; set; }

        [BindProperty]
        public Grant ThisGrant { get; set; }

        [BindProperty]
        public Grant[] AssociatedGrants { get; set; }

        [BindProperty]
        public ProjectNotes[] ProjectNotes { get; set; }

        [BindProperty]
        public MeetingMinutes[] MeetingNotes { get; set; }

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
            }
        }

        public void LoadGrantProject()
        {
            if (ItemType == "GrantProject")
            {
                GProject = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, ItemID))[0];
                AssociatedGrants = ObjectConverter.ToGrant(DatabaseControls.SelectFilter(10, 9, ItemID));

                List<BusRep> RepHolder = new List<BusRep>();
                List<BusPartner> PartnerHolder = new List<BusPartner>();
                List<MeetingMinutes> MMHolder = new List<MeetingMinutes>();

                foreach (Grant G in AssociatedGrants)
                {
                   PartnerHolder.Add(ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0,0,G.Bus_Partner_ID))[0]);

                }

                foreach (BusPartner BP in PartnerHolder)
                {
                   RepHolder.Add(ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 0, BP.Bus_Partner_ID))[0]);
                }

                foreach (BusRep BR in RepHolder)
                {
                    MMHolder.Add(ObjectConverter.ToMeetingMinutes(DatabaseControls.SelectFilter(12, 3, BR.Bus_Rep_ID))[0]);
                }

                BusPartners = PartnerHolder.ToArray();
                BusReps = RepHolder.ToArray();
                MeetingNotes = MMHolder.ToArray();
            }
        }




        public void OnGet()
        {
           ItemType = HttpContext.Session.GetString("ItemType");
           ItemID = HttpContext.Session.GetString("ItemID");
        }
    }
}
