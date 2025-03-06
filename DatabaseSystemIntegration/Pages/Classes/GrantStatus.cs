namespace DatabaseSystemIntegration.Pages.Classes
{ 
    public class GrantStatus
    {
        public string Grant_Status_ID { get; set; }
        public string Grant_Status { get; set; }

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

        public GrantStatus(string Status)
        {
            //set attributes
            Grant_Status_ID = MakeID();
            Grant_Status = Status;
        
        }
    }

}
