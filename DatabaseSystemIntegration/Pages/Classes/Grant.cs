using DatabaseSystemIntegration.Pages.Tools;
using Microsoft.Identity.Client;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Grant
    {
        public string Grant_ID { get; set; }
        public string Grant_Name { get; set; }
        public double Amount { get; set; }
        public DateOnly Submission_Date { get; set; }
        public DateOnly Award_Date { get; set; }

        public string Grant_Status_ID { get; set; }
        public string Grant_Status{ get; set; }
        public string Grant_Category_ID { get; set; }
        public string Grant_Category_Name { get; set; }
        public string Bus_Partner_ID { get; set; }
        public string Bus_Name { get; set; }
        public string Grant_Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Lead_Faculty_ID { get; set; }
        public string Lead_Faculty_Name { get; set; }


        public void SetVars()
        {
            Grant_Status = ObjectConverter.ToGrantStatus(DatabaseControls.SelectFilter(11, 11, Grant_Status_ID))[0].Grant_Status;
            Grant_Category_Name = ObjectConverter.ToGrantCategory(DatabaseControls.SelectFilter(8, 8, Grant_Category_ID))[0].Grant_Category_Name;
            Bus_Name = ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0, 0, Bus_Partner_ID))[0].Bus_Name;
            Project_Name = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, Grant_Project_ID))[0].Project_Name;
            Lead_Faculty_Name = ObjectConverter.ToFaculty(DatabaseControls.SelectFilter(6, 6, Lead_Faculty_ID))[0].Faculty_Name;
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

        public Grant(string name, double amt, DateOnly subDate, DateOnly awdDate, string LeadID,string StatusID,
            string CategoryID, string PartnerID, string ProjectID)
        {
            //set attributes and foreign keys
            Grant_ID = MakeID();
            Grant_Name = name;
            Amount = amt;
            Submission_Date = subDate;
            Award_Date = awdDate;
            Grant_Status_ID = StatusID;
            Grant_Category_ID = CategoryID;
            Bus_Partner_ID = PartnerID;
            Grant_Project_ID = ProjectID;
            Lead_Faculty_ID = LeadID;
            SetVars();
        }
    }
}
