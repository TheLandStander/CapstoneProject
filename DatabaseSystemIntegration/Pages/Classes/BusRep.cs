using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class BusRep
    {
        public string Bus_Rep_ID { get; set; }
        public string Rep_Name { get; set; }

        public string Bus_Partner_ID { get; set; }
        public string Bus_Name { get; set; }

        private string Info_ID { get; set; }

        public string GetInfoID()
        {
            return this.Info_ID;
        }

        public void SetInfoID(string InfoID)
        {
            this.Info_ID = InfoID;
        }

        public void SetVars()
        {
            Bus_Name = ObjectConverter.ToBusPartner(DatabaseControls.SelectFilter(0, 0, Bus_Partner_ID))[0].Bus_Name;
        
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

        public BusRep(string RepName, string ParnerID, string InfoID)
        {
            //set attributes and foreign keys
            Bus_Rep_ID = MakeID();
            Rep_Name = RepName;
            Bus_Partner_ID = ParnerID;
            Info_ID = InfoID;
            SetVars();
        }

    }
}
