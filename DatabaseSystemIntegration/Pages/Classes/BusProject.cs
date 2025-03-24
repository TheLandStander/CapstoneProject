namespace DatabaseSystemIntegration.Pages.Classes
{
    public class BusProject
    {
        public string Bus_Project_ID { get; set; }
        public string Project_Name { get; set; }

        public string Description { get; set; }

        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public DateOnly Due_Date { get; set; }

        public string Grant_Project_ID { get; set; }

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

        public BusProject(string Name, string Desc, DateOnly Start, DateOnly Due, string grant_Project_ID)
        {
            // set attributes
            Bus_Project_ID = MakeID();
            Project_Name = Name;
            Description = Desc;
            Start_Date = Start;
            Due_Date = Due;
            Grant_Project_ID = grant_Project_ID;
        }
    }
}
