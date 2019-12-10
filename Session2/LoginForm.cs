using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2
{
    public partial class LoginForm : Form
    {
        List<Employee> ListofEmployees;
        public LoginForm()
        {
            Initialize();
        }
        public async void Initialize()
        {
            Task<List<Employee>> dbTask = DBController.GetEmployees();
            InitializeComponent();
            ListofEmployees = await dbTask;
        }
        private async Task<Employee> checkForUser(string username, string password)
        {
            var user = (from u in ListofEmployees
                        where u.Username == username
                        where u.Password == password
                        select u).ToList();
            bool isEmpty = !user.Any();
            if (isEmpty)
            {
                return null;
            }
            else
            {
                return user.FirstOrDefault();
            }
        }
        private async void Login_button_Click(object sender, EventArgs e)
        {
            Employee loggedIn = await checkForUser(username_box.Text, password_box.Text);
            if(loggedIn != null)
            {
                this.Hide();
                var MA = new ManageAssets(loggedIn);
                MA.Closed += (s, args) => this.Close();
                MA.Show();
            }
            else
            {
                MessageBox.Show("invalid login", "eh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Kill_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            username_box.Text = "lyn";
            password_box.Text = "1234";
            Login_button_Click(sender,e);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            username_box.Text = "josefa";
            password_box.Text = "1324";
            Login_button_Click(sender,e);
        }
    }
}
