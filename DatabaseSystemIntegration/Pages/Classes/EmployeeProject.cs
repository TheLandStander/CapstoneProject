using DatabaseSystemIntegration.Pages.Tools;

namespace DatabaseSystemIntegration.Pages.Classes
{
    public class EmployeeProject
    {
        public string Employee_Project_ID { get; set; }
        public string Employee_ID { get; set; }

        public string Assigning_Admin_ID { get; set; }
        public string Bus_Project_ID { get; set; }

        public string Project_Name { get; set; }
        public string Employee_Name { get; set; }

        public string Assigning_Admin_Name { get; set; }

        public void SetVars()
        {
            Project_Name = ObjectConverter.ToBusProject(DatabaseControls.SelectFilter(2, 2, Bus_Project_ID))[0].Project_Name;
            Employee_Name = ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 4, Employee_ID))[0].Employee_Name;
            Assigning_Admin_Name = ObjectConverter.ToEmployee(DatabaseControls.SelectFilter(4, 4, Assigning_Admin_ID))[0].Employee_Name;
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

        public EmployeeProject(string AssigningID, string EmpID, string ProjectID)
        {   //set all they  keys in the bridge table
            Employee_Project_ID = MakeID();
            Assigning_Admin_ID = AssigningID;
            Employee_ID = EmpID;
            Bus_Project_ID = ProjectID;
            SetVars();
        }
    }
}
