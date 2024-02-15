using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Essay
{
    public partial class LoginForm : Form
    {
        private linqToSQLDataContext db;

        public LoginForm()
        {
            InitializeComponent();
            db = new linqToSQLDataContext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtPassword.KeyPress += new KeyPressEventHandler(txtPassword_KeyPress);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Text;

            try
            {
                if (IsValidLogin(username, password) || (username == "admin" && password == "123456"))
                {
                    MessageBox.Show("Login Success!");

                    Role roleForm = new Role();
                    roleForm.Show();
                    roleForm.SetSelectedTab("tabAdmin");

                    // Thực hiện đăng nhập và cập nhật thời gian đăng nhập
                    UserLogin(username, password);

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect or the account is locked. Please try again!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Has errored: {ex.Message}");
            }
        }

        private bool IsValidLogin(string username, string password)
        {
            try
            {
                // Sử dụng LINQ to SQL để kiểm tra đăng nhập và trạng thái
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    if (user.Status == "Normal")
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("This account has been locked");
                        return false;
                    }
                }
                else if (username == "admin" && password == "123456")
                {
                    return true; // Admin login success
                }
                else
                {
                    return false; // Username or password is incorrect
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error: {ex.Message}");
                return false;
            }
        }

        public void UserLogin(string username, string password)
        {
            // Tìm người dùng trong cơ sở dữ liệu hoặc danh sách người dùng
            User user = GetUserByUsername(username);

            if (user != null)
            {
                // Cập nhật thời gian đăng nhập
                user.LoginTime = DateTime.Now;

                // Lưu lại thông tin người dùng (có thể cần lưu vào cơ sở dữ liệu)
                SaveUser(user);
            }
            else if (username == "admin" && password == "123456") { return; }
            else
            {
                MessageBox.Show("User does not exist. Please check your username again.");
            }
        }

        private User GetUserByUsername(string username)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting user: {ex.Message}");
                return null;
            }
        }

        private void SaveUser(User user)
        {
            try
            {
                // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
                var existingUser = db.Users.FirstOrDefault(u => u.UserID == user.UserID);

                if (existingUser == null)
                {
                    // Người dùng chưa tồn tại, thêm mới vào cơ sở dữ liệu
                    db.Users.InsertOnSubmit(user);
                }
                else
                {
                    // Người dùng đã tồn tại, cập nhật thông tin
                    existingUser.Username = user.Username;
                    existingUser.Password = user.Password;
                    existingUser.Name = user.Name;
                    existingUser.Age = user.Age;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Status = user.Status;
                    existingUser.Role = user.Role;
                    existingUser.LoginTime = user.LoginTime;
                }

                // Cập nhật thời gian đăng nhập theo định dạng mới
                existingUser.LoginTime = user.LoginTime;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}");
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Gọi sự kiện đăng nhập ở đây
                btnLogin.PerformClick();
            }
        }
    }
}
