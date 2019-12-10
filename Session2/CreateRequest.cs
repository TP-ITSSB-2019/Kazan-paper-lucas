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
    public partial class CreateRequest : Form
    {
        int AssetID;
        EmergencyMaintenanceInfo currentInfo;
        Employee LoggedInUser;
        public CreateRequest(Employee employee, int assetid)
        {
            AssetID = assetid;
            LoggedInUser = employee;
            Initialize();
        }
        public async void Initialize()
        {
            Task<EmergencyMaintenanceInfo> dbtask = DBController.GetAssetInfo(AssetID);
            InitializeComponent();
            currentInfo = await dbtask;
            asset_sn_label.Text = currentInfo.AssetSN;
            asset_name_label.Text = currentInfo.AssetName;
            department_label.Text = currentInfo.Department;
            priority_combo.DataSource = currentInfo.Priorities;
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Back_button_Click(object sender, EventArgs e)
        {
            this.Hide();
            var MA = new ManageAssets(LoggedInUser);
            MA.Closed += (s, args) => this.Close();
            MA.Show();
        }

        private async void Send_button_Click(object sender, EventArgs e)
        {
            currentInfo.Considerations = otherconsideration_richbox.Text;
            currentInfo.SelectedPriorities = priority_combo.SelectedItem.ToString();
            currentInfo.Description = desc_richbox.Text;
            await DBController.addEmergency(currentInfo);
            Back_button_Click(sender, e);
        }
    }
    public class EmergencyMaintenanceInfo
    {
        public int AssetID { get; set; }
        public string AssetSN { get; set; }
        public string AssetName { get; set; }
        public List<string> Priorities { get; set; }
        public string SelectedPriorities { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Considerations { get; set; }
    }
}
