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

        public PartnerStatus Status { get; set; }

        public void SetVars()
        {
        Status = ObjectConverter.ToPartnerStatus(DatabaseControls.SelectFilter(3,3, PartnerStatusID))[0];
        }

        public MeetingMinutes[] GetMeetingMinutes()
        {
            return ObjectConverter.ToMeetingMinutes(DatabaseControls.SelectFilter(8, 2, PartnerID));
        }

        public Project[] GetAssociatedProjects()
        {
            List<Users> Representatives = new List<Users>();
            List<Project> Projects = new List<Project>();
            Users[] AllUsers = ObjectConverter.ToUsers(DatabaseControls.SelectNoFilter(19));
            foreach (Users u in AllUsers)
            {
                if (u.GetPartner().PartnerID == PartnerID)
                {
                    Representatives.Add(u);
                }
            }
            foreach (Users u in Representatives)
            {
                AssignedProject[] UserProjects = ObjectConverter.ToAssignedProject(DatabaseControls.SelectFilter(0, 19, u.UserID));

                foreach (AssignedProject ap in UserProjects)
                {
                    if (!Projects.Contains(ap.GetProject()))
                        {
                        Projects.Add(ap.GetProject());
                        }
                }
            }

            return Projects.ToArray();
        }

        public void AddMeeting(string Notes, DateOnly Date)
        {
            MeetingMinutes m = new MeetingMinutes(Notes, Date, PartnerID);
            DatabaseControls.Insert(m);
        }

        public void UpdateStatus(string StatusID)
        {
            DatabaseControls.UpdatePartnerStatus(PartnerID, StatusID);
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
