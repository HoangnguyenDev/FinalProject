using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public class DataContext
    {
        public DataContext()
        {
        }

        public bool CheckOCR(int ID, string text, string pathAvatar, string pathPlate, string pathFull)
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                var single = context.GoLeaves.Where(p => p.OwnerID == ID).OrderByDescending(p => p.GoDT).Single();
                if (single != null)
                    if (single.GoOCR == text)
                    {
                        single.LeaveDT = DateTime.Now;
                        single.LeaveFull = pathFull;
                        single.LeavePlate = pathPlate;
                        single.leaveAvatar = pathAvatar;
                        single.OutOCR = text;
                        context.SaveChangesAsync();
                        return true;

                    }
                return false;
            }
        }
        public string Getname(int ID)
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                return "";
            }
        }
        public int GetCountGoOut()
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                var count = context.GoLeaves.Where(p => p.GoDT.Value.Day == DateTime.Now.Day)
                    .Where(p => p.GoDT.Value.Month == DateTime.Now.Month)
                    .Where(p => p.GoDT.Value.Year == DateTime.Now.Year)
                    .Count();
                count = count != null ?  count:0;
                return count;
            }
        }
        public void CreateMember(string text, string pathAvatar, string pathPlate, string pathFull)
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                Member member = new Member
                {
                    CreateDT = DateTime.Now,
                    UpdateDT = DateTime.Now,
                    Address = "",
                    UniversityID = 0,
                    IsDeleted = false,

                };
                context.Members.Add(member);
                GoLeave goLeave = new GoLeave
                {
                    GoAvatar = pathAvatar,
                    GoDT = DateTime.Now,
                    GoFull = pathFull,
                    GoPlate = pathPlate,
                    GoOCR = text,
                    OwnerID = member.ID
                };
                context.GoLeaves.Add(goLeave);
                context.SaveChanges();
            }
        }
        public void CreateGoLeave(int ID,string text, string pathAvatar, string pathPlate, string pathFull)
        {
            try
            {
                using (var context = new admin_dangkythitoeicEntities())
                {
                    var old = context.GoLeaves
                        .OrderByDescending(p => p.GoDT)
                        .First();
                    if (DateTime.Now.Subtract(old.GoDT.Value).Seconds > 4)
                    {
                        GoLeave goLeave = new GoLeave
                        {
                            GoAvatar = pathAvatar,
                            GoDT = DateTime.Now,
                            GoFull = pathFull,
                            GoPlate = pathPlate,
                            GoOCR = text,
                            OwnerID = ID
                        };
                        context.GoLeaves.Add(goLeave);
                        context.SaveChanges();
                    }
                }
            }
            catch { }
        }
        public void CreateReason(string goText, string leaveText, 
            string goPathAvatar, string leavePathAvatar,
            string goPathPlate, string leavePathPlate, 
            string goPathFull, string leavePathFull )
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                Member member = new Member
                {
                    CreateDT = DateTime.Now,
                    UpdateDT = DateTime.Now,
                    Address = "",
                    UniversityID = 0,
                    IsDeleted = false,

                };
                context.Members.Add(member);
                GoLeave goLeave = new GoLeave
                {
                    GoAvatar = goPathAvatar,
                    GoDT = DateTime.Now,
                    GoFull = goPathFull,
                    GoPlate = goPathPlate,
                    GoOCR = goPathPlate,
                    LeavePlate = leavePathPlate,
                    leaveAvatar = leavePathAvatar,
                    LeaveDT = DateTime.Now,
                    LeaveFull = leavePathFull,
                    IsFinish = true,
                    OutOCR = leaveText,
                    OwnerID = member.ID,
                };
                context.GoLeaves.Add(goLeave);
                context.SaveChanges();
            }
        }
        

        public LeaveError CheckGoLeave(int ID, string text, string pathAvatar, string pathPlate, string pathFull)
        {
            try
            {
                using (var context = new admin_dangkythitoeicEntities())
                {
                    var single = context.GoLeaves.Where(p => p.OwnerID == ID && !p.IsFinish).OrderByDescending(p => p.GoDT).FirstOrDefault();
                    if (single != null)
                    {
                        if (single.GoOCR == text)
                        {
                            single.leaveAvatar = pathAvatar;
                            single.LeaveDT = DateTime.Now;
                            single.LeaveFull = pathFull;
                            single.LeavePlate = pathPlate;
                            single.OutOCR = text;
                            single.IsFinish = true;
                            context.Entry(single).State = EntityState.Modified;
                            context.SaveChanges();
                            return LeaveError.SUCCESSS;
                        }
                        else
                            return LeaveError.WRONGOCG;
                    }
                    else
                        return LeaveError.NOTFOUND;


                }
            }
            catch { }
            return LeaveError.UNKNWON;
        }
        public void LoadTraningFace(out List<string> listString, out List<int> listInt)
        {
            listString = new List<string>();
            listInt = new List<int>();
            using (var context = new admin_dangkythitoeicEntities())
            {
                try
                {
                    var db = context.GoLeaves.ToList();
                    foreach (var item in db)
                    {
                        listString.Add(item.GoAvatar);
                        listInt.Add(Convert.ToInt32(item.OwnerID));
                    }
                }
                catch { }
            }
        }
    }
    public enum LeaveError
    {
        NOTFOUND,
        WRONGOCG,
        SUCCESSS,
        UNKNWON,
    }
    public enum MODE
    {
        IN,
        OUT,
    }
}
