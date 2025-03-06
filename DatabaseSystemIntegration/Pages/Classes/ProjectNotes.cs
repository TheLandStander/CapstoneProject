using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ProjectNotes
    {
        public string Notes_ID { get; set; }
        public string Project_Note { get; set;}
        public DateTime Date_Recorded { get; set; }
        public string Bus_Project_ID { get; set; }
        public string Project_Name { get; set; }


        public void SetVars()
        {
            Project_Name = ObjectConverter.ToBusProject(DatabaseControls.SelectFilter(2, 2, Bus_Project_ID))[0].Project_Name;
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

        public ProjectNotes(string Note, DateTime recordDate, string ProjectID)
        {
            // set attributes and foreign keys
            Notes_ID = MakeID();
            Project_Note = Note;
            Date_Recorded = recordDate;
            Bus_Project_ID = ProjectID;
            SetVars();
        }
    }
}
