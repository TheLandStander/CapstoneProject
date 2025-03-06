namespace DatabaseSystemIntegration.Pages.Classes
{
    public class GrantProject
    {
        public string Grant_Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Description { get; set; }
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
