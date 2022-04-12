using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterGenerator.Data.Context;
using MasterGenerator.Data.Entity;
using MasterGenerator.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGenerator.Data.Repository
{

    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public IEnumerable<ProjectModel> GetProjects()
        {
            return _context.Project
            .ProjectTo<ProjectModel>(_mapper.ConfigurationProvider).AsQueryable();

        }      
        public IEnumerable<DealDetailsModel> GetDealDetails()
        {
            return _context.DealDetails
            .ProjectTo<DealDetailsModel>(_mapper.ConfigurationProvider).AsQueryable();
        }
        public async Task<bool> AddProjectRange(List<Project> projects)
        {
            try
            {
                await _context.Project.AddRangeAsync(projects);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<List<string?>> GetProjectStatus()
        {
            return await _context.Project.Where(x => !string.IsNullOrEmpty(x.DisplayStatus)).Select(x => x.DisplayStatus).Distinct().ToListAsync();
        }
        public async Task<Project?> GetProjectByProjectId(int projectId)
        {
            return await _context.Project.Where(x => x.ProjectId == projectId).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
