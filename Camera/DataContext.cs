using System;
using System.Collections.Generic;
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
                        single.LeaveOcg = pathPlate;
                        single.leaveAvatar = pathAvatar;
                        single.LeaveOcg = text;
                        context.SaveChangesAsync();
                        return true;

                    }
                return false;
            }
        }
        public int GetCountGoOut()
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                int count = context.GoLeaves.Where(p => p.GoDT.Value.Date == DateTime.Now.Date).Count();
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
                    GoOcg = pathPlate,
                    GoOCR = text,
                    OwnerID = member.ID
                };
                context.GoLeaves.Add(goLeave);
                context.SaveChanges();
            }
        }
        public void CreateGoLeave(int ID,string text, string pathAvatar, string pathPlate, string pathFull)
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                GoLeave goLeave = new GoLeave
                {
                    GoAvatar = pathAvatar,
                    GoDT = DateTime.Now,
                    GoFull = pathFull,
                    GoOcg = pathPlate,
                    GoOCR = text,
                    OwnerID = ID
                };
                context.GoLeaves.Add(goLeave);
                context.SaveChangesAsync();
            }
        }
        public void CheckGoLeave(int ID, string text, string pathAvatar, string pathPlate, string pathFull)
        {
            using (var context = new admin_dangkythitoeicEntities())
            {
                GoLeave goLeave = new GoLeave
                {
                    GoAvatar = pathAvatar,
                    GoDT = DateTime.Now,
                    GoFull = pathFull,
                    GoOcg = pathPlate,
                    GoOCR = text,
                    OwnerID = ID
                };
                context.GoLeaves.Add(goLeave);
                context.SaveChangesAsync();
            }
        }
        public void LoadTraningFace(out List<string> listString, out List<int> listInt)
        {
            listString = new List<string>();
            listInt = new List<int>();
            using (var context = new admin_dangkythitoeicEntities())
            {
                var db = context.GoLeaves.ToList();
                foreach(var item in db)
                {
                    listString.Add(item.GoAvatar);
                    listInt.Add(Convert.ToInt32(item.OwnerID));
                }
            }
        }
    }
}
