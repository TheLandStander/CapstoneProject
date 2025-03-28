using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Partner
    {
        public string PartnerID { get; set; }
        public string BusName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PartnerStatusID { get; set; }
        public MeetingMinutes[] Meetings { get; set; }

        public PartnerStatus Status { get; set; }

        public void SetVars()
        {
        Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectFilter(14,14, PartnerStatusID))[0];
        Meetings = ObjectConverter.ToMeetingMinutes(DatabaseControls.SelectFilter(8, 8, PartnerID));
        }

        public void AddMeeting(string Notes, DateOnly Date)
        {
            MeetingMinutes m = new MeetingMinutes(Notes, Date, PartnerID);
            DatabaseControls.Insert(m);
        }

        public Partner(string busName, string address, string phone, string email, string description, string partnerStatusID)
        {
            PartnerID = DatabaseControls.MakeID();
            BusName = busName;
            Address = address;
            Phone = phone;
            Email = email;
            Description = description;
            PartnerStatusID = partnerStatusID;
        }
    }

}
