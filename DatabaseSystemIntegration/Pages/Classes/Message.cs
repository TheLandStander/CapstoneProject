using DatabaseSystemIntegration.Pages.Tools;
using System.Reflection;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class Message
    {
        public string Message_ID { get; set; }
        public string Sender_ID { get; set; }
        public string Receiver_ID { get; set; }

        public DateTime Send_Date { get; set; }
        public string Message_Subject { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }

        public void SetVars()
        {
            Sender = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectFilter(14, 14, Sender_ID))[0].getUsername();
            Recipient = ObjectConverter.ToPersonalInfo(DatabaseControls.SelectFilter(14, 14, Receiver_ID))[0].getUsername();
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

        public Message(string subject, string content, DateTime sendDate, string sender, string receiver)
        {
            this.Message_ID = MakeID();
            this.Sender_ID = sender;
            this.Receiver_ID = receiver;
            this.Message_Subject = subject;
            this.Content = content;
            this.Send_Date = sendDate;
            SetVars();
        }


    }
}
