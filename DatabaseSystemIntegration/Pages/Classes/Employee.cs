namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Employee
    {
        public string Employee_ID { get; set; }
        public string Employee_Name { get; set; }

        public bool is_Admin { get; set; }
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

        public Employee(string name,bool Admin,string InfoID)
        {
            //set attributes and foreign keys 
            Employee_ID = MakeID();
            //convert int to boolean
            is_Admin = Admin;
            Employee_Name = name;
            Info_ID = InfoID;
        } 
    }
}
