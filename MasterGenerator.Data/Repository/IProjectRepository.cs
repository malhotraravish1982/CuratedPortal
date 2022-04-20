using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectModel> GetProjects();
        IEnumerable<DealDetailsModel> GetDealDetails();
        Task<bool> AddProjectRange(List<Project> projects);
        Task<List<string?>> GetProjectStatus();
        Task<Project?> GetProjectByProjectId(int projectId);
        IEnumerable<ProjectModel> GetProjectsByCustomerNames(List<string> name);
        IEnumerable<ProjectModel> GetProjectsByVisibleFeildPermission(List<string> name);
        Task<List<string>> GetCustomerFeildByUserId(int id);
       // IEnumerable<ProjectModel>? GetProjectsByCustomerNamess(Task<List<string>> customerNameList);
    }
}
