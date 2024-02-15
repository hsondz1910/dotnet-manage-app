using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Essay
{
    public partial class UserManagement : Form
    {
        private User tbUser;
        private linqToSQLDataContext db;
        private int selectedUserId;

        public UserManagement()
        {
            InitializeComponent();
            tbUser = new User();
            db = new linqToSQLDataContext();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            // var Lst = (from u in db.Users select u).ToList();
            var Lst = (from u in db.Users
                       select new
                       {
                           u.UserID,
                           u.Username,
                           u.Password,
                           u.Name,
                           u.Age,
                           u.PhoneNumber,
                           u.Role,
                           u.Status,
                           LoginTime = u.LoginTime.HasValue ? u.LoginTime.Value.ToString() : null
                       }).ToList();
            grd.DataSource = Lst;

            ClearDataBindings();

            txtUserID.DataBindings.Add("text", Lst, "UserID");
            txtUsername.DataBindings.Add("text", Lst, "Username");
            txtPassword.DataBindings.Add("text", Lst, "Password");

            if (btnRadioManager != null && btnRadioManager.Checked)
            {
                btnRadioManager.DataBindings.Add("text", Lst, "Role");
            }
            else if (btnRadioUser != null && btnRadioUser.Checked)
            {
                btnRadioUser.DataBindings.Add("text", Lst, "Role");
            }

            txtName.DataBindings.Add("text", Lst, "Name");
            txtAge.DataBindings.Add("text", Lst, "Age");
            txtPhone.DataBindings.Add("text", Lst, "PhoneNumber");

            if (btnRadioActive != null && btnRadioActive.Checked)
            {
                btnRadioActive.DataBindings.Add("text", Lst, "Status");
            }
            else if (btnRadioLock != null && btnRadioLock.Checked)
            {
                btnRadioLock.DataBindings.Add("text", Lst, "Status");
            }

            grd.Columns["LoginTime"].DataPropertyName = "LoginTime";

            grd.CellClick += grd_CellClick;
            grd.CellDoubleClick += new DataGridViewCellEventHandler(grd_CellDoubleClick);
        }

        private void ClearDataBindings()
        {
            txtUserID.DataBindings.Clear();
            txtUsername.DataBindings.Clear();
            txtPassword.DataBindings.Clear();

            if (btnRadioManager != null && btnRadioManager.Checked)
            {
                btnRadioManager.DataBindings.Clear();
            }
            else if (btnRadioUser != null && btnRadioUser.Checked)
            {
                btnRadioUser.DataBindings.Clear();
            }

            txtName.DataBindings.Clear();
            txtAge.DataBindings.Clear();
            txtPhone.DataBindings.Clear();

            if (btnRadioActive != null && btnRadioActive.Checked)
            {
                btnRadioActive.DataBindings.Clear();
            }
            else if (btnRadioLock != null && btnRadioLock.Checked)
            {
                btnRadioLock.DataBindings.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            linqToSQLDataContext db = new linqToSQLDataContext();
            tbUser.Username = txtUsername.Text;
            tbUser.Password = txtPassword.Text;

            if (btnRadioManager != null && btnRadioManager.Checked)
            {
                tbUser.Role = btnRadioManager.Text;
            }
            else if (btnRadioUser != null && btnRadioUser.Checked)
            {
                tbUser.Role = btnRadioUser.Text;
            }


            tbUser.Name = txtName.Text;

            tbUser.Age = int.Parse(txtAge.Text);

            tbUser.PhoneNumber = int.Parse(txtPhone.Text);


            if (btnRadioActive != null && btnRadioActive.Checked)
            {
                tbUser.Status = btnRadioActive.Text;
            }
            else if (btnRadioLock != null && btnRadioLock.Checked)
            {
                tbUser.Status = btnRadioLock.Text;
            }

            db.Users.InsertOnSubmit(tbUser);
            db.SubmitChanges();
            UserManagement_Load(sender, e);
            MessageBox.Show("Successed to add!");
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input and the backspace key
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem txtUserID.Text có phải là một số nguyên hợp lệ hay không
            if (int.TryParse(txtUserID.Text, out int userId))
            {
                try
                {
                    // Sử dụng userId thay vì chuyển đổi lại từ txtUserID.Text
                    tbUser = db.Users.Where(u => u.UserID == userId).Single();
                    tbUser.Name = txtName.Text;

                    db.Users.DeleteOnSubmit(tbUser);
                    db.SubmitChanges();
                    UserManagement_Load(sender, e);
                    MessageBox.Show("Successed to delete");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Invalid User ID. Please enter a valid integer.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem txtUserID.Text có phải là một số nguyên hợp lệ hay không
            if (int.TryParse(txtUserID.Text, out int userId))
            {
                // Kiểm tra tồn tại của người dùng
                var existingUser = db.Users.FirstOrDefault(u => u.UserID == userId);

                if (existingUser != null)
                {
                    try
                    {
                        // Sử dụng existingUser thay vì chuyển đổi lại từ txtUserID.Text
                        existingUser.Username = txtUsername.Text;
                        existingUser.Password = txtPassword.Text;
                        existingUser.Name = txtName.Text;
                        existingUser.Age = int.Parse(txtAge.Text);
                        existingUser.PhoneNumber = int.Parse(txtPhone.Text);

                        // Kiểm tra trạng thái của các RadioButton
                        if (btnRadioManager != null && btnRadioManager.Checked)
                        {
                            existingUser.Role = btnRadioManager.Text;
                        }
                        else if (btnRadioUser != null && btnRadioUser.Checked)
                        {
                            existingUser.Role = btnRadioUser.Text;
                        }

                        if (btnRadioActive != null && btnRadioActive.Checked)
                        {
                            existingUser.Status = btnRadioActive.Text;
                        }
                        else if (btnRadioLock != null && btnRadioLock.Checked)
                        {
                            existingUser.Status = btnRadioLock.Text;
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SubmitChanges();
                        UserManagement_Load(sender, e);
                        MessageBox.Show("Successed to edit");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error editing user: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("User does not exist. Please check the User ID again.");
                }
            }
            else
            {
                MessageBox.Show("Invalid User ID. Please enter a valid integer.");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Role roleForm = new Role();

            roleForm.Show();

            roleForm.SetSelectedTab("tabAdmin");

            this.Close();
        }

        private void Radio_RoleCheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadio = (RadioButton)sender;

            if (selectedRadio.Checked)
            {
                foreach (Control control in rolePanel.Controls)
                {
                    if (control is RadioButton otherRadio && otherRadio != selectedRadio)
                    {
                        otherRadio.Checked = false;
                    }
                }
            }
        }

        private void Radio_StatusCheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadio = (RadioButton)sender;

            if (selectedRadio.Checked)
            {
                foreach (Control control in statusPanel.Controls)
                {
                    if (control is RadioButton otherRadio && otherRadio != selectedRadio)
                    {
                        otherRadio.Checked = false;
                    }
                }
            }
        }

        private void btnRadioManager_CheckedChanged(object sender, EventArgs e)
        {
            Radio_RoleCheckedChanged(sender, e);
        }

        private void btnRadioUser_CheckedChanged(object sender, EventArgs e)
        {
            Radio_RoleCheckedChanged(sender, e);
        }

        private void btnRadioActive_CheckedChanged(object sender, EventArgs e)
        {
            Radio_StatusCheckedChanged(sender, e);
        }

        private void btnRadioLock_CheckedChanged(object sender, EventArgs e)
        {
            Radio_StatusCheckedChanged(sender, e);
        }

        private void grd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = grd.Rows[e.RowIndex];

                // Kiểm tra giá trị của cột "Role" của hàng được chọn
                string roleValue = selectedRow.Cells["Role"].Value.ToString();

                // Tích vào nút radio button tương ứng với giá trị của cột "Role"
                switch (roleValue)
                {
                    case "Manager":
                        btnRadioManager.Checked = true;
                        break;
                    case "User":
                        btnRadioUser.Checked = true;
                        break;
                }

                // Kiểm tra giá trị của cột "Status" của hàng được chọn
                string statusValue = selectedRow.Cells["Status"].Value.ToString();

                // Tích vào nút radio button tương ứng với giá trị của cột "Status"
                switch (statusValue)
                {
                    case "Normal":
                        btnRadioActive.Checked = true;
                        break;
                    case "Locked":
                        btnRadioLock.Checked = true;
                        break;
                }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            linqToSQLDataContext db = new linqToSQLDataContext();
            var Lst = (from u in db.Users where (u.UserID).ToString().Contains(txtSearch.Text) select u).ToList();
            grd.DataSource = Lst;

            ClearDataBindings();

            txtUserID.DataBindings.Add("text", Lst, "UserID");
            // txtUsername.DataBindings.Add("text", Lst, "Username");
            // txtPassword.DataBindings.Add("text", Lst, "Password");

            if (btnRadioManager != null && btnRadioManager.Checked)
            {
                btnRadioManager.DataBindings.Add("text", Lst, "Role");
            }
            else if (btnRadioUser != null && btnRadioUser.Checked)
            {
                btnRadioUser.DataBindings.Add("text", Lst, "Role");
            }

            txtName.DataBindings.Add("text", Lst, "Name");
            txtAge.DataBindings.Add("text", Lst, "Age");
            txtPhone.DataBindings.Add("text", Lst, "PhoneNumber");

            if (btnRadioActive != null && btnRadioActive.Checked)
            {
                btnRadioActive.DataBindings.Add("text", Lst, "Status");
            }
            else if (btnRadioLock != null && btnRadioLock.Checked)
            {
                btnRadioLock.DataBindings.Add("text", Lst, "Status");
            }
        }

        private void grd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            // Kiểm tra xem sự kiện được kích hoạt ở dòng nào
            if (e.RowIndex >= 0 && e.RowIndex < grd.Rows.Count)
            {
                int selectedUserId;

                // Kiểm tra xem có hàng được chọn không
                if (grd.SelectedRows.Count > 0)
                {
                    if (int.TryParse(grd.Rows[e.RowIndex].Cells["UserID"].Value.ToString(), out selectedUserId))
                    {
                        // Mở form UserDetails và truyền ID để hiển thị thông tin tương ứng
                        UserDetails userDetailsForm = new UserDetails(selectedUserId);
                        userDetailsForm.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}
