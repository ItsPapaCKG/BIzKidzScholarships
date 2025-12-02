using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Entities;

namespace BizKidzScholarships.API.Extensions
{
    public static class AutoMapperRegistration
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<UserProfile, UserProfileDTO>();
                cfg.CreateMap<UserProfileDTO, UserProfile>()
                    .ForMember(m => m.User, a => a.Ignore());

                cfg.CreateMap<DashboardTaskDTO, TaskItem>();
                cfg.CreateMap<TaskItem, DashboardTaskDTO>();

                cfg.CreateMap<UserProfile, UpdateUserProfileDTO>();
                cfg.CreateMap<UpdateUserProfileDTO, UserProfile>()
                    .ForMember(m => m.UserId, a => a.Ignore());
            });
        }
    }
}
