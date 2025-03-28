using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.Identity.Client;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class PartnerStatus
    {
        public string Partner_Status_ID { get; set; }
        public string Status_Flag { get; set; }

        public PartnerStatus(string Status)
        {   // set attributes
            Partner_Status_ID = DatabaseControls.MakeID();
            Status_Flag = Status;

        }
    }
}
