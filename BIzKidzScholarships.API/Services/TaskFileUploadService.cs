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

        public PresignedPostURLDataModel Generate_Presigned_URL()
        {
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2))
                {
                    string key = "uploads/" + new Guid();

                    CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                    presignedPostRequest.BucketName = "bizkidz-task-bucket";
                    presignedPostRequest.Key = key;
                    presignedPostRequest.Expires = DateTime.Now.AddMinutes(2);

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
