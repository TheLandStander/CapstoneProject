namespace DatabaseSystemIntegration.Pages.Classes
{
    public class OrgType
    {
        public string Org_Type_ID { get; set; }
        public string Org_Type { get; set; }

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

        public OrgType(string Type)
        {
            // set attributes
            Org_Type_ID = MakeID();
            Org_Type = Type;
        }
    }
}
