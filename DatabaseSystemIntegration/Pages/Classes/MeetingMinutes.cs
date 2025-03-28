using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class MeetingMinutes
    {
        public string MeetingMinutesID { get; set; }
        public string MeetingNotes { get; set; }
        public DateOnly Date { get; set; }
        public string PartnerID { get; set; }
        public Partner PartnerInvolved { get; set; }

        public void SetVars()
        {
            PartnerInvolved  = ObjectConverter.ToPartner(DatabaseControls.SelectFilter(2, 2, PartnerID))[0];
        }

        public MeetingMinutes(string meetingNotes, DateOnly date, string partnerID)
        {
            MeetingMinutesID = DatabaseControls.MakeID();  // Set primary key
            MeetingNotes = meetingNotes;
            Date = date;
            PartnerID = partnerID;
        }
    }


}
