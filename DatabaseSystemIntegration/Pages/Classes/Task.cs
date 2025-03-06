namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Task
    {
        public string Task_ID { get; set; }
        public string Task_Name { get; set; }
        public string Task_Description { get; set; }
        public string Bus_Project_ID { get; set; }
        public string Project_Name { get; set; }

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

        public Task(string Name, string desc, string ProjectID)
        {
            //Set attributes and foreign keys
            Task_ID = MakeID();
            Task_Name = Name;
            Task_Description = desc;
            Bus_Project_ID = ProjectID;
        }
    }
}
