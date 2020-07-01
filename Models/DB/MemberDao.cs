using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Models.DB
{
    public class MemberDao
    {
        ShopBanDoTheThao db = null;
        public MemberDao()
        {
            db = new ShopBanDoTheThao();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public IEnumerable<Member> ListAllPage(string name, int page, int pageSize)
        {
            IQueryable<Member> model = db.Members.Where(x => x.IDMemType == 4 && x.FullName != null);
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.FullName.Contains(name));
            return model.OrderBy(x => x.IDMember).Skip((page - 1) * pageSize).Take(pageSize);

        }

        public Member FindID(int id)
        {
            return db.Members.Find(id);
        }

        public long Insert(Member member)
        {
            member.IDMemType = 3;
            member.PassWord = MaHoa.MaHoaSangMD5(member.UserName + member.PassWord);
            db.Members.Add(member);
            db.SaveChanges();
            return member.IDMember;
        }

        public void Edit(Member member)
        {
            var temp = db.Members.Find(member.IDMember);
            temp.UserName = member.UserName;
            temp.FullName = member.FullName;
            temp.Email = member.Email;
            temp.Address = member.Address;
            temp.PhoneNumber = member.PhoneNumber;
            temp.Avatar = member.Avatar;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var member = db.Members.Find(id);
            db.Members.Remove(member);
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Count(string name)
        {
            IQueryable<Member> model = db.Members.Where(x => x.IDMemType == 4);
            if (!string.IsNullOrEmpty(name))
                model = model.Where(x => x.FullName.Contains(name));
            return model.Count();
        }
    }
}