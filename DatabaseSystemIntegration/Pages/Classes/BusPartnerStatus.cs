using Microsoft.Identity.Client;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class BusPartnerStatus
    {
        public string Bus_Partner_Status_ID { get; set; }
        public string Status_Flag { get; set; }

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

        public BusPartnerStatus(string Status)
        {   // set attributes
            Bus_Partner_Status_ID = MakeID();
            Status_Flag = Status;

        }
    }
}
