using Amazon;
using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Amazon.S3;
using Amazon.S3.Model;

namespace BizKidzScholarships.API.Services
{
    public class FileUploadService
    {
        public FileUploadService(ICurrentUser user, IMapper mapper, BizKidzDbContext context)
        {
            //_user = user;
            //_context = context;
            //_mapper = mapper;
        }

        public string Generate_Presigned_URL(string objectKey)
        {
            using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2)) { 
                CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                presignedPostRequest.BucketName = "bizkidz-task-bucket";
                presignedPostRequest.Key = objectKey;
                presignedPostRequest.Expires = DateTime.Now.AddMinutes(2);

                var response = client.CreatePresignedPost(presignedPostRequest);

                return objectKey;
            }
        }

    }
}
