using System.Security.Cryptography;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class PersonalInfo
    {
        private string Info_ID { get; set; }
        private string PrimaryContact { get; set; }
        private string SecondaryContact { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }

        public string getUsername()
        {

            return UserName;
        }

        public string getInfoID()
        {

            return Info_ID;
        }

        public string getPassword()
        {

            return this.Password;
        }

        public string GetPrimaryContact()
        {

            return this.PrimaryContact;
        }

        public string GetSecondaryContact()
        {

            return this.SecondaryContact;
        }

        public void SetPrimaryContact(string contact)
        {

            this.PrimaryContact = contact;
        }

        public void SetSecondaryContact(string contact)
        {

            this.SecondaryContact = contact;
        }

        public void setPassword(string password)
        {

            this.Password = password;
        }

        //Allows database records to be read & sync IDs upon creation 
        public void setInfo_ID(string InfoID)
        {
            this.Info_ID = InfoID;
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

        public PersonalInfo(string PrimaryCont, string SecondaryCont, string User, string Pass)
        {
            //set primary key & attributes
            Info_ID = MakeID();
            PrimaryContact = PrimaryCont;
            SecondaryContact = SecondaryCont;
            UserName = User;
            Password = Pass;
        }
    }
}
