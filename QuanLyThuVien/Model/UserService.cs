using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class UserService
    {
        QLTV_BETAEntities ObjContext;
        public UserService()
        {
            ObjContext = new QLTV_BETAEntities();
        }
        public bool Add(UserDTO objNewUser)
        {
            bool IsAdded = false;
            try
            {
                var ObjUser = new ACCOUNT(); // khởi tạo user mới
                var ObjParameter = ObjContext.PARAMETERS.First(); // lấy giá trị đầu từ bảng parameters

                // gán giá trị của User mới vào biến ObjUser
                ObjUser.MaDG = "DG" + ObjParameter.IDDocGia.ToString("000");
                ObjUser.TaiKhoan = objNewUser.TaiKhoan;
                ObjUser.MatKhau = objNewUser.MatKhau;

                ObjContext.ACCOUNTs.Add(ObjUser); // thêm User mới vào bảng ACCOUNT

                // lưu thay đổi và kiểm tra dữ liệu đã được thêm chưa
                var NoOfRowsAffected = ObjContext.SaveChanges(); 
                IsAdded = NoOfRowsAffected > 0;

                // cập nhật ID độc giả lên 1 đơn vị sau khi thêm
                ObjParameter.IDDocGia+=1;
                ObjContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsAdded;
        }
    }
}
