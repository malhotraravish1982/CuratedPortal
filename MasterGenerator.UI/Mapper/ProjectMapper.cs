using MasterGenerator.Data.Entity;

namespace MasterGenerator.UI.Mapper
{
    public static class ProjectMapper
    {

        public static List<Project> MapFromRangeData(IList<IList<object>> values)
        {
            var projects = new List<Project>();
            foreach (var value in values)
            {
                Project project = new()
                {
                    ProjectId = Convert.ToInt32(value[0]),
                    CustomerName = value[1].ToString(),
                    ProjectName = value[2].ToString(),
                    PONumber = value[3].ToString(),
                    PODate = value[4].ToString(),
                    TotalQuantity = value[5].ToString(),
                    Currency = value[6].ToString(),
                    POAmount = value[7].ToString(),
                    CSStatus = value[8].ToString(),
                    DisplayStatus = value[9].ToString(),
                    ShipmentMethod = value[10].ToString(),
                    ProductionCompletion = value[11].ToString(),
                    VesselETA = value[12].ToString(),
                    VesselETD = value[13].ToString(),
                    EstimatedDeliveryDate = value[14].ToString(),
                    ActualDeliveryDate = value[15].ToString(),
                    PreProductionManager = value[16].ToString(),
                    OnTimeStatus = value[17].ToString()
                };
                projects.Add(project);
            }
            return projects;
        }
        public static IList<IList<object>> MapToRangeData(Project project)
        {
            var objectList = new List<object>() { 
                project.ProjectId, 
                project.CustomerName, 
                project.ProjectName, 
                project.PONumber, 
                project.PODate, 
                project.TotalQuantity, 
                project.Currency, 
                project.POAmount,
                project.CSStatus,
                project.DisplayStatus,
                project.ShipmentMethod,
                project.ProductionCompletion,
                project.VesselETA,
                project.VesselETD,
                project.EstimatedDeliveryDate,
                project.ActualDeliveryDate,
                project.PreProductionManager,
                project.OnTimeStatus
            };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
