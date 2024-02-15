using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;
using System.Xml.Linq;

namespace Essay
{
    public partial class UserDetails : Form
    {

        private int userId;
        private string extension = ".jpg";

        public UserDetails(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadImageName();
            cbBoxImage.SelectedValueChanged += cbBoxImage_SelectedValueChanged;
        }

        private void UserDetails_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        void LoadImageName()
        {
            List<String> listImage = new List<String>() { "user", "scene", "windows"};
            cbBoxImage.DataSource = listImage;
        }

        private void LoadUserInfo()
        {
            linqToSQLDataContext db = new linqToSQLDataContext();
            var user = db.Users.Where(u => u.UserID == userId).FirstOrDefault();

            if (user != null)
            {
                lblName.Text = "Name: " + user.Name;
                lblRole.Text = "Role: " + user.Role;
                lblAge.Text = "Age: " + user.Age;
                lblPhone.Text = "Phone: " + user.PhoneNumber;
                lblStatus.Text = "Status: " + user.Status;
            }
            else
            {
                MessageBox.Show("Invalid User");
                this.Close();
            }
        }

        private void bbtnBack_Click(object sender, EventArgs e)
        {
            UserManagement userManagerForm = new UserManagement();
            userManagerForm.Show();
            this.Close();
        }

        private void btnEditPic_Click(object sender, EventArgs e)
        {
            // Use the existing PictureBox on your form instead of creating a new one
            pictureBox.Image = new Bitmap(Application.StartupPath + "\\Resources\\user.jpg");
        }

        private void cbBoxImage_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedValue != null)
            {
                try
                {
                    // Use Image.FromFile to load the image, and handle exceptions
                    pictureBox.Image = Image.FromFile(Application.StartupPath + "\\Resources\\" + cb.SelectedValue.ToString() + extension);
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., show a message or log it)
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
        }
    }
}
