using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.Identity.Client;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class BusPartner
    {
        public string Bus_Partner_ID { get; set; }
        public string Bus_Name { get; set; }
        public string Org_Type { get; set; }
        public string Org_Type_ID { get; set; }
        public string Status_Flag { get; set; }
        public string Partner_Status_ID { get; set; }
        private string Info_ID { get; set; }


        public void SetVars()
        {
            Org_Type = ObjectConverter.ToOrgType(DatabaseControls.SelectFilter(13, 13, Org_Type_ID))[0].Org_Type;
            Status_Flag = ObjectConverter.ToBusPartnerStatus(DatabaseControls.SelectFilter(1, 1, Partner_Status_ID))[0].Status_Flag;
        }

        //getter for infoID
        public string getInfoID()
        {
            return Info_ID;
        }

        private string MakeID()
        {
            //Makes the primary key 
            string ID = "";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                ID += rand.Next(10);
            }
            return ID;
        }

        public BusPartner(string Name, string StatusID, string Org_TypeID, string InfoID)
        { 
            //sets attributes and foreign keys
            Bus_Partner_ID = MakeID();
            Bus_Name = Name;
            Org_Type_ID = Org_TypeID;
            Partner_Status_ID = StatusID;
            Info_ID = InfoID;
            SetVars();
        }

    }
}
