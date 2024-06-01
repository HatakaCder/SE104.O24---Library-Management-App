using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Utils
{
    public static class DataAccess
    {
        public static List<T> SelectData<T>() where T : class
        {
            List<T> list = new List<T>();

            using(QLTV_BETAEntities context = new QLTV_BETAEntities())
            {
                DbSet<T> dbSet = context.Set<T>();
                list = dbSet.ToList();
            }

            return list;
        }
    }
}
