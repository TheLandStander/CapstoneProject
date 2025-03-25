using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class GrantProject
    {
        public string Grant_Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Description { get; set; }
        public string Bus_Project_ID { get; set; }
        public string Bus_Project_Name { get; set; }

        public void SetVars(string GrantProjectID)
        {
            Console.Write(Grant_Project_ID);
            if (DatabaseControls.SelectFilter(2, 9, Grant_Project_ID).HasRows)
            {
                BusProject project = ObjectConverter.ToBusProject(DatabaseControls.SelectFilter(2, 9,GrantProjectID))[0];
                Bus_Project_ID = project.Bus_Project_ID;
                Bus_Project_Name = project.Project_Name;
            }
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

        public GrantProject(string Name, string Desc)
        {
            //set attributes
            Grant_Project_ID = MakeID();
            Project_Name = Name;
            Description = Desc;
        }
    }
}
