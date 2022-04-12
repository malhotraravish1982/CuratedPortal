using MasterGenerator.Data.Entity;
using MasterGenerator.Data.Repository;

namespace MasterGenerator.UI.Mapper
{
    public class ProjectMapper
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public ProjectMapper(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        public static List<Project> MapFromRangeData(IList<IList<object>> values)
        {
            var projects = new List<Project>();
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value[0].ToString()))
                {
                    var projectId = value[0].ToString();
                    //var existingProject = await _unitOfWork.IProjectRepository.GetProjectByProjectId(Convert.ToInt32(projectId));

                    Project project = new()
                    {
                        //Id = existingProject != null ? existingProject.Id : 0,
                        ProjectId = value.Count >= 1 ? Convert.ToInt32(projectId) : 0,
                        CustomerName = value.Count >= 2 ? value[1].ToString().Trim() : String.Empty,
                        ProjectName = value.Count >= 3 ? value[2].ToString().Trim() : String.Empty,
                        PONumber = value.Count >= 4 ? value[3].ToString().Trim() : String.Empty,
                        PODate = value.Count >= 5 ? value[4].ToString().Trim() : String.Empty,
                        TotalQuantity = value.Count >= 6 ? value[5].ToString().Trim() : String.Empty,
                        Currency = value.Count >= 7 ? value[6].ToString().Trim() : String.Empty,
                        POAmount = value.Count >= 8 ? value[7].ToString().Trim() : String.Empty,
                        CSStatus = value.Count >= 9 ? value[8].ToString().Trim() : String.Empty,
                        DisplayStatus = value.Count >= 10 ? value[9].ToString().Trim() : String.Empty,
                        ShipmentMethod = value.Count >= 11 ? value[10].ToString().Trim() : String.Empty,
                        ProductionCompletion = value.Count >= 12 ? value[11].ToString().Trim() : String.Empty,
                        VesselETA = value.Count >= 13 ? value[12].ToString().Trim() : String.Empty,
                        VesselETD = value.Count >= 14 ? value[13].ToString().Trim() : String.Empty,
                        EstimatedDeliveryDate = value.Count >= 15 ? value[14].ToString().Trim() : String.Empty,
                        ActualDeliveryDate = value.Count >= 16 ? value[15].ToString().Trim() : String.Empty,
                        PreProductionManager = value.Count >= 17 ? value[16].ToString().Trim() : String.Empty,
                        OnTimeStatus = value.Count >= 18 ? value[17].ToString().Trim() : String.Empty
                    };
                    projects.Add(project);
                }
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
