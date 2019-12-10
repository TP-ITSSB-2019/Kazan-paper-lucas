using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2
{
    class DBController
    {
        static Session2Entities db = new Session2Entities();
        public static async Task<List<Employee>> GetEmployees()
        {
            var query = (from e in db.Employees select e).ToList();
            return query;
        }
        public static async Task<List<AvailableAssets>> getEmployeeAssets(int empID)
        {
            List<AvailableAssets> returnlist = new List<AvailableAssets>();
            var query = (from a in db.Assets
                         where a.EmployeeID == empID
                         select a).ToList();
            
            foreach (var item in query)
            {
                AvailableAssets availableAssets = new AvailableAssets();
                availableAssets.AssetID = (int)item.ID;
                availableAssets.AssetSN = item.AssetSN;
                availableAssets.AssetName = item.AssetName;
                DateTime? dt = null;
                int numberOfEM = 0;
                try
                {
                    var query2 = (from a in query
                                  join em in db.EmergencyMaintenances on a.ID equals em.AssetID
                                  where a.ID == item.ID
                                  orderby em.EMEndDate descending
                                  select em.EMEndDate).ToList();
                    dt = query2.First();
                    numberOfEM = query2.Count();
                }catch(Exception e)
                {
                }
                availableAssets.LastClosedEM = dt;
                availableAssets.NumberOfEM = numberOfEM;
                returnlist.Add(availableAssets);
            }
            return returnlist;
        }
        public static async Task<EmergencyMaintenanceInfo> GetAssetInfo(int assetID)
        {
            EmergencyMaintenanceInfo emergencyMaintenanceInfo = new EmergencyMaintenanceInfo();
            var priorities = (from p in db.Priorities
                         select p.Name).ToList();
            var asset = (from e in db.EmergencyMaintenances
                         where e.ID == assetID
                         select e).First();
            emergencyMaintenanceInfo.AssetSN = asset.Asset.AssetSN;
            emergencyMaintenanceInfo.AssetID = (int)asset.AssetID;
            emergencyMaintenanceInfo.AssetName = asset.Asset.AssetName;
            emergencyMaintenanceInfo.Department = asset.Asset.DepartmentLocation.Department.Name;
            emergencyMaintenanceInfo.Priorities = priorities;
            return emergencyMaintenanceInfo;
        }
        public static async Task addEmergency(EmergencyMaintenanceInfo info)
        {
            var ID = (from em in db.EmergencyMaintenances
                      orderby em.ID descending
                      select em.ID).First()+1;
            var priorityID = (from p in db.Priorities
                              where p.Name == info.SelectedPriorities
                              select p.ID).First();
            EmergencyMaintenance EM = new EmergencyMaintenance();
            EM.ID = ID;
            EM.PriorityID = priorityID;
            EM.AssetID = info.AssetID;
            EM.DescriptionEmergency = info.Description;
            EM.EMReportDate = DateTime.Now;
            EM.EMStartDate = DateTime.Now;
            EM.OtherConsiderations = info.Considerations;
            EM.EMTechnicianNote = null;
            db.EmergencyMaintenances.Add(EM);
            await db.SaveChangesAsync();
        }
    }
}
