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
    public partial class ManageAssets : Form
    {
        Employee LoggedInUser;
        List<AvailableAssets> ListofAvailableAssets;
        List<RequestingEM> ListofRequestingEMs;
        public ManageAssets(Employee employee)
        {
            LoggedInUser = employee;
            Initialize();
        }
        public async void Initialize()
        {
            if (LoggedInUser.isAdmin == true)
            {

                InitializeComponent();
            }
            else
            {
                Task<List<AvailableAssets>> dbTask = DBController.getEmployeeAssets((int)LoggedInUser.ID);
                InitializeComponent();
                label1.Text = "Available Assets: ";
                multi_func_button.Text = "Send Emergency Maintenance Request";
                ListofAvailableAssets = await dbTask;
                dataGridView1.DataSource = ListofAvailableAssets;
                dataGridView1.Columns[0].Visible = false;
            }
        }

        private void Multi_func_button_Click(object sender, EventArgs e)
        {
            if (LoggedInUser.isAdmin == true)
            {

            }
            else
            {
                this.Hide();
                var MA = new CreateRequest(LoggedInUser, ListofAvailableAssets[dataGridView1.CurrentCell.RowIndex].AssetID);
                MA.Closed += (s, args) => this.Close();
                MA.Show();
            }
        }

        private void Back_button_Click(object sender, EventArgs e)
        {
            this.Hide();
            var LF = new LoginForm();
            LF.Closed += (s, args) => this.Close();
            LF.Show();
        }
    }
    public class AvailableAssets
    {
        public int AssetID { get; set; }
        public string AssetSN { get; set; }
        public string AssetName { get; set; }
        public DateTime? LastClosedEM { get; set; }
        public int NumberOfEM { get; set; }

    }
    public class RequestingEM
    {
        public int AssetID { get; set; }
        public int AssetSN { get; set; }
        public string AssetName { get; set; }
        public DateTime RequestDate { get; set; }
        public int EmployeeName { get; set; }
        public string Department { get; set; }

        public RequestingEM(int assetID, int assetSN, string assetName, DateTime requestDate, int employeeName, string department)
        {
            AssetID = assetID;
            AssetSN = assetSN;
            AssetName = assetName;
            RequestDate = requestDate;
            EmployeeName = employeeName;
            Department = department;
        }
    }
}
