using DatabaseSystemIntegration.Pages.Classes;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Azure.Core.GeoJson;
using System.Reflection;
using System.Data;
using System.Collections;

namespace DatabaseSystemIntegration.Pages.Tools
{
    //Noah Kurtz, **WORKED INDIVIDUALLY**
    public class ObjectConverter
    {
        public static Partner[] ToPartner(SqlDataReader data)
        {
            List<Partner> list = new List<Partner>();
            while (data.Read())
            {
                Partner obj = new Partner(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    data.GetValue(3).ToString(),
                    data.GetValue(4).ToString(),
                    data.GetValue(5).ToString(),
                    data.GetValue(6).ToString()
                    );
                obj.PartnerID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (Partner i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static PartnerStatus[] ToPartnerStatus(SqlDataReader data)
        {
            List<PartnerStatus> list = new List<PartnerStatus>();
            while (data.Read())
            {
                PartnerStatus obj = new PartnerStatus(
                    data.GetValue(1).ToString()
                );
                obj.Partner_Status_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
           return list.ToArray();
        }

        public static MeetingMinutes[] ToMeetingMinutes(SqlDataReader data)
        {
            List<MeetingMinutes> list = new List<MeetingMinutes>();
            while (data.Read())
            {
                MeetingMinutes obj = new MeetingMinutes(
                    data.GetValue(1).ToString(),
                    DateOnly.FromDateTime((DateTime)data.GetValue(2)),
                    data.GetValue(3).ToString()
                );
                obj.MeetingMinutesID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (MeetingMinutes i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

       public static PersonalInfo[] ToPersonalInfo(SqlDataReader data)
        {
            List<PersonalInfo> list = new List<PersonalInfo>();
            while (data.Read())
            {
                PersonalInfo obj = new PersonalInfo(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    data.GetValue(3).ToString()
                );
                obj.setInfo_ID(data.GetValue(0).ToString());
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
           return list.ToArray();
        }


        public static UserType[] ToUserType(SqlDataReader data)
        {
            List<UserType> list = new List<UserType>();
            while (data.Read())
            {
                UserType obj = new UserType(
                    data.GetValue(1).ToString()
                );
                obj.UserTypeID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
           return list.ToArray();
        }

        public static UserStatus[] ToUserStatus(SqlDataReader data)
        {
            List<UserStatus> list = new List<UserStatus>();
            while (data.Read())
            {
                UserStatus obj = new UserStatus(
                    data.GetValue(1).ToString()
                );
                obj.UserStatusID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
           return list.ToArray();
        }

       public static Users[] ToUsers(SqlDataReader data)
        {
            List<Users> list = new List<Users>();
            while (data.Read())
            {
                Users obj = new Users(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    data.GetValue(3).ToString(),
                    data.GetValue(4).ToString(),
                    data.GetValue(5).ToString()
                );
                obj.UserID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (Users i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static Role[] ToRole(SqlDataReader data)
        {
            List<Role> list = new List<Role>();
            while (data.Read())
            {
                Role obj = new Role(
                    data.GetValue(1).ToString()
                );
                obj.RoleID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
           return list.ToArray();
        }

        public static UserRole[] ToUserRole(SqlDataReader data)
        {
            List<UserRole> list = new List<UserRole>();
            while (data.Read())
            {
                UserRole obj = new UserRole(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString()
                );
                obj.UserRoleID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (UserRole i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static ProjectStatus[] ToProjectStatus(SqlDataReader data)
        {
            List<ProjectStatus> list = new List<ProjectStatus>();
            while (data.Read())
            {
                ProjectStatus obj = new ProjectStatus(
                    data.GetValue(1).ToString()
                );
                obj.ProjectStatusID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            return list.ToArray();
        }

        public static Project[] ToProject(SqlDataReader data)
        {
            List<Project> list = new List<Project>();
            while (data.Read())
            {
                Project obj = new Project(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    DateOnly.FromDateTime((DateTime)data.GetValue(3)),
                    DateOnly.FromDateTime((DateTime)data.GetValue(5)),
                    data.GetValue(7).ToString(),
                    data.GetValue(8).ToString()
                );
                obj.ProjectID = data.GetValue(0).ToString();
                obj.EndDate = DateOnly.FromDateTime((DateTime)data.GetValue(4));
                obj.ProjectLeadID = data.GetValue(6).ToString();
                list.Add(obj);
            }
            data.Close();
            data.DisposeAsync();
            foreach (Project i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

       public static AssignedProject[] ToAssignedProject(SqlDataReader data)
        {
            List<AssignedProject> list = new List<AssignedProject>();
            while (data.Read())
            {
                AssignedProject obj = new AssignedProject(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString()
                );
                obj.AssignedProjectID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (AssignedProject i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static Tasks[] ToTask(SqlDataReader data)
        {
            List<Tasks> list = new List<Tasks>();
            while (data.Read())
            {
                Tasks obj = new Tasks(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    DateOnly.FromDateTime((DateTime)data.GetValue(3)),
                    DateOnly.FromDateTime((DateTime)data.GetValue(4)),
                    Convert.ToBoolean(((byte[])data.GetValue(6))[0]),
                    data.GetValue(7).ToString()
                );
                obj.TaskID = data.GetValue(0).ToString();
                obj.EndDate = DateOnly.FromDateTime((DateTime)data.GetValue(5));
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (Tasks i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static ChildTask[] ToChildTask(SqlDataReader data)
        {
            List<ChildTask> list = new List<ChildTask>();
            while (data.Read())
            {
                ChildTask obj = new ChildTask(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    DateOnly.FromDateTime((DateTime)data.GetValue(3)),
                    DateOnly.FromDateTime((DateTime)data.GetValue(4)),
                    Convert.ToBoolean(((byte[])data.GetValue(6))[0]),
                    data.GetValue(8).ToString()
                );
                obj.ChildTaskID = data.GetValue(0).ToString();
                obj.UserID = data.GetValue(7).ToString();
                obj.EndDate = DateOnly.FromDateTime((DateTime)data.GetValue(5));
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();

            return list.ToArray();
        }

       public static AssignedTask[] ToAssignedTask(SqlDataReader data)
        {
            List<AssignedTask> list = new List<AssignedTask>();
            while (data.Read())
            {
                AssignedTask obj = new AssignedTask(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString()
                );
                obj.AssignedTaskID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (AssignedTask i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static Message[] ToMessages(SqlDataReader data)
        {
            List<Message> list = new List<Message>();
            while (data.Read())
            {
                Message obj = new Message(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    Convert.ToDateTime(data.GetValue(3)),
                    data.GetValue(4).ToString(),
                    data.GetValue(5).ToString()
                );
                obj.Message_ID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (Message i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static GrantStatus[] ToGrantStatus(SqlDataReader data)
        {
            List<GrantStatus> list = new List<GrantStatus>();
            while (data.Read())
            {
                GrantStatus obj = new GrantStatus(
                    data.GetValue(1).ToString()
                );
                obj.StatusID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            return list.ToArray();
        }
        public static GrantCategory[] ToGrantCategory(SqlDataReader data)
        {
            List<GrantCategory> list = new List<GrantCategory>();
            while (data.Read())
            {
                GrantCategory obj = new GrantCategory(
                    data.GetValue(1).ToString()
                );
                obj.CategoryID = data.GetValue(0).ToString();
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            return list.ToArray();
        }

        public static Grant[] ToGrants(SqlDataReader data)
        {
            List<Grant> list = new List<Grant>();
            while (data.Read())
            {
                Grant obj = new Grant(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    Convert.ToDecimal(data.GetValue(3)),
                    data.GetValue(8).ToString(),
                    data.GetValue(9).ToString()
                );
                obj.GrantID = data.GetValue(0).ToString();
                obj.StartDate = DateOnly.FromDateTime((DateTime)data.GetValue(4));
                obj.SubmissionDate = DateOnly.FromDateTime((DateTime)data.GetValue(5));
                obj.AwardDate = DateOnly.FromDateTime((DateTime)data.GetValue(6));
                obj.DueDate = DateOnly.FromDateTime((DateTime)data.GetValue(7));
                list.Add(obj);
            }
            data.DisposeAsync();
            data.Close();
            foreach (Grant i in list)
            {
                i.SetVars();
            }
            return list.ToArray();
        }

        public static ProjectNotes[] ToNotes(SqlDataReader data)
        {
            List<ProjectNotes> list = new List<ProjectNotes>();
            while (data.Read())
            {
                ProjectNotes obj = new ProjectNotes(
                    data.GetValue(1).ToString(),
                    data.GetValue(2).ToString(),
                    data.GetValue(3).ToString(),
                    DateOnly.FromDateTime((DateTime)data.GetValue(4)),
                    data.GetValue(5).ToString()
                );

                obj.NotesID = data.GetValue(0).ToString();
                list.Add(obj);
            } 
            data.DisposeAsync();
            data.Close();
            return list.ToArray();
        }


    }
}
