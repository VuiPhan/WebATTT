using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Models.DB
{
    public class EmployeeDao
    {
        ShopBanDoTheThao db = null;
        public EmployeeDao()
        {
            db = new ShopBanDoTheThao();
        }

        public IEnumerable<Member> ListAllPage(string name, int page, int pageSize, int type)
        {
            IQueryable<Member> model = null;
            if (type == 0)
            {
                model = db.Members.Where(x => x.IDMemType == 2 || x.IDMemType == 3);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            else if (type == 1)
            {
                model = db.Members.Where(x => x.IDMemType == 2);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            else
            {
                model = db.Members.Where(x => x.IDMemType == 3);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            return model.OrderBy(x => x.IDMember).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Member FindID(int id)
        {
            return db.Members.Find(id);
        }

        public long Insert(Member employee, int IdMemType)
        {
            employee.IDMemType = IdMemType;
            employee.PassWord = MaHoa.MaHoaSangMD5(employee.PassWord);
            db.Members.Add(employee);
            db.SaveChanges();
            return employee.IDMember;
        }

        public void Edit(Member employee)
        {
            var temp = db.Members.Find(employee.IDMember);
            temp.IDMemType = employee.IDMemType;
            temp.UserName = employee.UserName;
            if (temp.PassWord == employee.PassWord)
            {
                temp.PassWord = employee.PassWord;
            }
            else
            {
                temp.PassWord = MaHoa.MaHoaSangMD5(employee.PassWord);
            }
            temp.FullName = employee.FullName;
            temp.IDCard = employee.IDCard;
            temp.Email = employee.Email;
            temp.Address = employee.Address;
            temp.PhoneNumber = employee.PhoneNumber;
            temp.Salary = employee.Salary;
            temp.Avatar = employee.Avatar;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var employee = db.Members.Find(id);
            db.Members.Remove(employee);
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

        public int Count(string name, int type)
        {
            IQueryable<Member> model = null;
            if (type == 0)
            {
                model = db.Members.Where(x => x.IDMemType == 2 || x.IDMemType == 3);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            else if (type == 1)
            {
                model = db.Members.Where(x => x.IDMemType == 2);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            else
            {
                model = db.Members.Where(x => x.IDMemType == 3);
                if (!string.IsNullOrEmpty(name))
                    model = model.Where(x => x.FullName.Contains(name));
            }
            return model.Count();
        }
    }
}