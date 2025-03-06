namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Faculty
    {
        public string Faculty_ID { get; set; }
        public string Faculty_Name { get; set; }
        private string Info_ID { get; set; }

        //getter for infoID
        public string getInfoID()
        {
            return Info_ID;
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

        public Faculty(string name, string InfoID)
        {
            //set attributes and foreign keys
            Faculty_ID = MakeID();
            Faculty_Name = name;
            Info_ID = InfoID;
        }
    }
}
