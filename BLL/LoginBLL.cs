using DAL;
using DTO;

namespace BLL
{
    public class LoginBLL
    {
        private LoginDAL dal = new LoginDAL();

        public TaiKhoanDTO GetTaiKhoan(string username, string password)
        {
            return dal.GetTaiKhoan(username, password);
        }
    }
}
