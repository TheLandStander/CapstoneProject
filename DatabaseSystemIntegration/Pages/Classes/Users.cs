using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Users
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string UserTypeID { get; set; }
        public string UserStatusID { get; set; }
        public string InfoID { get; set; }
        public string PartnerID { get; set; }

        public UserRole UserRole { get; set; }

        public UserStatus status { get; set; }

        public UserType type { get; set; }

        public void SetVars()
        {
            if (DatabaseControls.SelectFilter(16, 19, UserID).HasRows)
            {
                UserRole = ObjectConverter.ToUserRole(DatabaseControls.SelectFilter(16, 19, UserID))[0];
            }

            status = ObjectConverter.ToUserStatus(DatabaseControls.SelectFilter(17, 17, UserStatusID))[0];
            type = ObjectConverter.ToUserType(DatabaseControls.SelectFilter(18, 18, UserTypeID))[0];
        }

        public Partner GetPartner() 
        {
            return ObjectConverter.ToPartner(DatabaseControls.SelectFilter(2, 2, PartnerID))[0];

        }

        public Users(string name, string userTypeID, string userStatusID, string infoID, string partnerID)
        {
            UserID = DatabaseControls.MakeID();  // Set primary key
            Name = name;
            UserTypeID = userTypeID;
            UserStatusID = userStatusID;
            InfoID = infoID;
            PartnerID = partnerID;
        }
    }

}
