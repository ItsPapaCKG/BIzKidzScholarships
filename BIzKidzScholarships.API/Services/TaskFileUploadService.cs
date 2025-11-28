using Amazon;
using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Amazon.S3;
using Amazon.S3.Model;
using BizKidzScholarships.Data.Models;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;

namespace BizKidzScholarships.API.Services
{
    public class TaskFileUploadService
    {
        private ICurrentUser _user;
        private BizKidzDbContext _context;
        private IMapper _mapper;

        public TaskFileUploadService(ICurrentUser user, IMapper mapper, BizKidzDbContext context)
        {
            _user = user;
            _context = context;
            _mapper = mapper;
        }

    }
}
