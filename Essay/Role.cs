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
    public partial class Role : Form
    {
        public Role()
        {
            InitializeComponent();
        }

        

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            // Tạo một thể hiện mới của UserManagement form
            UserManagement userManagementForm = new UserManagement();

            // Hiển thị UserManagement form
            userManagementForm.Show();

            // Đóng Role form nếu bạn muốn đóng nó khi chuyển đến UserManagement form
            this.Close();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {

            }
            else if (tabControl.SelectedIndex == 1)
            {

            }
            else if (tabControl.SelectedIndex == 2)
            {

            }
        }

        private void Role_Load(object sender, EventArgs e)
        {

        }

        internal void SetSelectedTab(string v)
        {
            // Tìm tab có tên tương ứng và đặt SelectedTab
            foreach (TabPage tab in tabControl.TabPages)
            {
                if (tab.Name == "adminTab")
                {
                    tabControl.SelectedTab = tab;
                    break;
                }
            }
        }

        private void btnSignOut1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
