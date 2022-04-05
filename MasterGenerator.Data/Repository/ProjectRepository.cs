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
    
    public class ProjectRepository :IProjectRepository
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
    }
}
