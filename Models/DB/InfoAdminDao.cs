using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;
using Web.Models.ViewModels;

namespace Web.Models.DB
{
    public class InfoAdminDao
    {
        ShopBanDoTheThao db = null;
        public InfoAdminDao()
        {
            db = new ShopBanDoTheThao();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public Member GetInfoAdmin()
        {
            Member model = db.Members.Where(x => x.IDMemType == 1).SingleOrDefault();
            return model;
        }

        public bool Update(Member model)
        {
            var admin = db.Members.Where(x => x.IDMemType==model.IDMemType && x.IDMember ==model.IDMember).SingleOrDefault();
            admin.FullName = model.FullName;
            if (admin.PassWord == model.PassWord)
            {
                admin.PassWord = model.PassWord;
            }
            else
            {
                admin.PassWord = MaHoa.MaHoaSangMD5(model.UserName + model.PassWord);
            }
            admin.Address = model.Address;
            admin.Email = model.Email;
            admin.PhoneNumber = model.PhoneNumber;
            admin.Avatar = model.Avatar;
            admin.IDCard = model.IDCard;
            db.SaveChanges();
            return true;
        }
    }
}