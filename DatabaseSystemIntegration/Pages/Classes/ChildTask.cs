using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class ChildTask
    {
        public string Child_Task_ID { get; set; }
        public string Task_Name { get; set; }
        public string Task_Description { get; set; }
        public string Parent_Task_ID { get; set; }
        public string Parent_Task_Name { get; set; }
        public bool Completed { get; set; }

        private void setVars()
        {
            Parent_Task_Name = ObjectConverter.ToTask(DatabaseControls.SelectFilter(16,16 ,Parent_Task_ID))[0].Task_Name;
                
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

        public ChildTask(string Name, string Desc,bool Complete ,string Parent_Task)
        {
            Child_Task_ID = MakeID();
            Task_Name = Name;
            Task_Description = Desc;
            Completed = Complete;
            Parent_Task_ID = Parent_Task;
            setVars();
        
        }
    }
}
