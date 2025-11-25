using Amazon;
using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Amazon.S3;
using Amazon.S3.Model;
using BizKidzScholarships.Data.Models;

namespace BizKidzScholarships.API.Services
{
    public class TaskFileUploadService
    {
        public TaskFileUploadService(ICurrentUser user, IMapper mapper, BizKidzDbContext context)
        {
            //_user = user;
            //_context = context;
            //_mapper = mapper;
        }

        public PresignedPostURLDataModel Generate_Presigned_URL(string ext)
        {
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2))
                {
                    string key = "uploads/" + Guid.NewGuid() + "." + ext;

                    CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                    presignedPostRequest.BucketName = "bizkidz-task-bucket";
                    presignedPostRequest.Key = key;
                    presignedPostRequest.Expires = DateTime.UtcNow.AddMinutes(10);

                    var response = client.CreatePresignedPost(presignedPostRequest);

                    return new PresignedPostURLDataModel() { Url = response.Url, Key = key, Fields = response.Fields };
                }
            }
            catch (Exception ex)
            {
                return new PresignedPostURLDataModel() { Url = ex.Message };
                ;
            }
        }

    }
}
