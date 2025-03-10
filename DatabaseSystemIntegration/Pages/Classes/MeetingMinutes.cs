using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class MeetingMinutes
    {
        public string Meeting_Minutes_ID { get; set; }
        public string Meeting_Notes { get; set; }
        public DateTime Meeting_Date { get; set; }
        public string Bus_Rep_ID { get; set; }
        public string Rep_Name { get; set; }
        public string Bus_Name { get; set; }

        public void SetVars()
        {
            Rep_Name = ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 3,Bus_Rep_ID))[0].Rep_Name;
            Bus_Name = ObjectConverter.ToBusRep(DatabaseControls.SelectFilter(3, 3, Bus_Rep_ID))[0].Bus_Name;
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

        public MeetingMinutes(string Notes, DateTime date, string RepID)
        {
            // set attributes and foreign keys
            Meeting_Minutes_ID = MakeID();
            Meeting_Notes = Notes;
            Meeting_Date = date;
            Bus_Rep_ID = RepID;
            SetVars();
        }


    }
}
