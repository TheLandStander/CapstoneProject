using DatabaseSystemIntegration.Pages.Classes;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Azure.Core.GeoJson;
using System.Reflection;
using System.Data;

namespace DatabaseSystemIntegration.Pages.Tools
{
    //Noah Kurtz, **WORKED INDIVIDUALLY**
    public class ObjectConverter
    {


        /*
         feed it a 2d string array that represents the table (using my db controls class
         make an array that has the same length as the number of rows in the table
        make a second string array to handle all the column values for each row
        after iterating through each row, fill the fields array & in the outerloop make the new object
        and add it to the array.

        must fill the Primary key outside of the constructor (constructor creates ID, if read from the database prevent it from overriding)**
         *   */


        public static BusPartner[] ToBusPartner(SqlDataReader data)
        {
            List<BusPartner> list = new List<BusPartner>();
            while (data.Read())
            {
                BusPartner obj = new BusPartner(data.GetValue(1).ToString(), data.GetValue(2).ToString(),
                data.GetValue(3).ToString(), data.GetValue(4).ToString());
                obj.Bus_Partner_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static BusPartnerStatus[] ToBusPartnerStatus(SqlDataReader data)
        {
            List<BusPartnerStatus> list = new List<BusPartnerStatus>();
            while (data.Read())
            {
                BusPartnerStatus obj = new BusPartnerStatus(data.GetValue(1).ToString());
                obj.Bus_Partner_Status_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static BusRep[] ToBusRep(SqlDataReader data)
        {
            List<BusRep> list = new List<BusRep>();
            while (data.Read())
            {
                BusRep obj = new BusRep(data.GetValue(1).ToString(), data.GetValue(2).ToString(), data.GetValue(3).ToString());
                obj.Bus_Rep_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static BusProject[] ToBusProject(SqlDataReader data)
        {
            List<BusProject> list = new List<BusProject>();
            while (data.Read())
            {
                BusProject obj = new BusProject(data.GetValue(1).ToString(), data.GetValue(2).ToString(), 
                    DateOnly.ParseExact(data.GetValue(3).ToString().Replace(" 12:00:00 AM", ""), "M/d/yyyy"), 
                    DateOnly.ParseExact(data.GetValue(5).ToString().Replace(" 12:00:00 AM", ""), "M/d/yyyy"),
                    data.GetValue(6).ToString());
                obj.Bus_Project_ID = data.GetValue(0).ToString();
                obj.End_Date = DateOnly.ParseExact(data.GetValue(4).ToString().Replace(" 12:00:00 AM", ""), "M/d/yyyy");
                
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static Employee[] ToEmployee(SqlDataReader data)
        {
            List<Employee> list = new List<Employee>();
            while (data.Read())
            {
                Employee obj = new Employee(data.GetValue(1).ToString(), 
                Convert.ToBoolean(data.GetValue(2).ToString()), data.GetValue(3).ToString());
                obj.Employee_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static EmployeeProject[] ToEmployeeProject(SqlDataReader data)
        {
            List<EmployeeProject> list = new List<EmployeeProject>();
            while (data.Read())
            {
                EmployeeProject obj = new EmployeeProject(data.GetValue(1).ToString(), data.GetValue(2).ToString(), data.GetValue(3).ToString());
                obj.Employee_Project_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static Faculty[] ToFaculty(SqlDataReader data)
        {
            List<Faculty> list = new List<Faculty>();
            while (data.Read())
            {
                Faculty obj = new Faculty(data.GetValue(1).ToString(), data.GetValue(2).ToString());
                obj.Faculty_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static FacultyProject[] ToFacultyProject(SqlDataReader data)
        {
            List<FacultyProject> list = new List<FacultyProject>();
            while (data.Read())
            {
                FacultyProject obj = new FacultyProject(data.GetValue(1).ToString(), data.GetValue(2).ToString());
                obj.Faculty_Project_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static Grant[] ToGrant(SqlDataReader data)
        {
            List<Grant> list = new List<Grant>();
            while (data.Read())
            {
                Grant obj = new Grant(data.GetValue(1).ToString(), Double.Parse(data.GetValue(2).ToString()), 
                    DateOnly.ParseExact(data.GetValue(3).ToString().Replace(" 12:00:00 AM", ""), "M/d/yyyy"),
                    DateOnly.ParseExact(data.GetValue(4).ToString().Replace(" 12:00:00 AM", ""), "M/d/yyyy"), 
                    data.GetValue(5).ToString(), data.GetValue(6).ToString(),
                    data.GetValue(7).ToString(), data.GetValue(8).ToString(), data.GetValue(9).ToString());
                obj.Grant_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static GrantCategory[] ToGrantCategory(SqlDataReader data)
        {
            List<GrantCategory> list = new List<GrantCategory>();
            while (data.Read())
            {
                GrantCategory obj = new GrantCategory(data.GetValue(1).ToString());
                obj.Grant_Category_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static GrantProject[] ToGrantProject(SqlDataReader data)
        {
            List<GrantProject> list = new List<GrantProject>();
            while (data.Read())
            {
                GrantProject obj = new GrantProject(data.GetValue(1).ToString(),data.GetValue(2).ToString());
                obj.Grant_Project_ID = data.GetValue(0).ToString();
                obj.SetVars(data.GetValue(0).ToString());
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static GrantStatus[] ToGrantStatus(SqlDataReader data)
        {
            List<GrantStatus> list = new List<GrantStatus>();
            while (data.Read())
            {
                GrantStatus obj = new GrantStatus(data.GetValue(1).ToString());
                obj.Grant_Status_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static MeetingMinutes[] ToMeetingMinutes(SqlDataReader data)
        {
            List<MeetingMinutes> list = new List<MeetingMinutes>();
            while (data.Read())
            {
                MeetingMinutes obj = new MeetingMinutes(data.GetValue(1).ToString(), 
                 DateTime.Parse(data.GetValue(2).ToString()), data.GetValue(3).ToString());
                obj.Meeting_Minutes_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static OrgType[] ToOrgType(SqlDataReader data)
        {
            List<OrgType> list = new List<OrgType>();
            while (data.Read())
            {
                OrgType obj = new OrgType(data.GetValue(1).ToString());
                obj.Org_Type_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static PersonalInfo[] ToPersonalInfo(SqlDataReader data)
        {
            List<PersonalInfo> list = new List<PersonalInfo>();
            while (data.Read())
            {
                PersonalInfo obj = new PersonalInfo(data.GetValue(1).ToString(), data.GetValue(2).ToString(),
                    data.GetValue(3).ToString(), data.GetValue(4).ToString());
                obj.setInfo_ID(data.GetValue(0).ToString());
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static ProjectNotes[] ToProjectNotes(SqlDataReader data)
        {
            List<ProjectNotes> list = new List<ProjectNotes>();
            while (data.Read())
            {
                ProjectNotes obj = new ProjectNotes(data.GetValue(1).ToString(),
                DateTime.Parse(data.GetValue(2).ToString()), data.GetValue(3).ToString());
                obj.Notes_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static Classes.Task[] ToTask(SqlDataReader data)
        {
            List<Classes.Task> list = new List<Classes.Task>();
            while (data.Read())
            {
                Classes.Task obj = new Classes.Task(data.GetValue(1).ToString(), data.GetValue(2).ToString(), Convert.ToBoolean(data.GetValue(3).ToString()), data.GetValue(4).ToString());
                obj.Task_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static ChildTask[] ToChildTask(SqlDataReader data)
        {
            List<ChildTask> list = new List<ChildTask>();
            while (data.Read())
            {
                ChildTask obj = new ChildTask(data.GetValue(1).ToString(), data.GetValue(2).ToString(), Convert.ToBoolean(data.GetValue(3).ToString()), data.GetValue(4).ToString());
                obj.Child_Task_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }

        public static Classes.Message[] ToMessage(SqlDataReader data)
        {
            List<Classes.Message> list = new List<Classes.Message>();
            while (data.Read())
            {
                Classes.Message obj = new Classes.Message(data.GetValue(1).ToString(), data.GetValue(2).ToString(),
                DateTime.Parse(data.GetValue(3).ToString()), data.GetValue(4).ToString(), data.GetValue(5).ToString());
                obj.Message_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            return list.ToArray();
        }


    }
}
