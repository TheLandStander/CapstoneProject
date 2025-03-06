using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class FacultyProject
    {
        public string Faculty_Project_ID { get; set; }
        public string Faculty_ID { get; set; }
        public string Grant_Project_ID { get; set; }
        public string Faculty_Name { get; set; }
        public string Grant_Project_Name { get; set; }
        public string Grant_Project_Desc { get; set; }


        public void SetVars()
        {
            Faculty_Name = ObjectConverter.ToFaculty(DatabaseControls.SelectFilter(6, 6, Faculty_ID))[0].Faculty_Name;
            Grant_Project_Name = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, Grant_Project_ID))[0].Project_Name;
            Grant_Project_Desc = ObjectConverter.ToGrantProject(DatabaseControls.SelectFilter(9, 9, Grant_Project_ID))[0].Description;
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

        public FacultyProject(string FacID, string ProjectID)
        {   //set all they  keys in the bridge table
            Faculty_Project_ID = MakeID();
            Faculty_ID = FacID;
            Grant_Project_ID = ProjectID;
            SetVars();
        }
    }
}
