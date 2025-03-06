using System.Reflection.Metadata;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class GrantCategory
    {
        public string Grant_Category_ID { get; set; }
        public string Grant_Category_Name { get; set; }

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

        public GrantCategory(string Name) 
        {
            //set primary key & attributes
            Grant_Category_ID = MakeID();
            Grant_Category_Name = Name;
        }
    }
}
