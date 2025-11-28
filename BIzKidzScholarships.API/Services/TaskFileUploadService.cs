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

        public ResponseModel? StartUploadHandshake(PresignedRequestModel req)
        {
            PresignedHandshakeModel handshake = new PresignedHandshakeModel();

            // first, get a Presigned URL from Amazon
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(RegionEndpoint.USEast2))
                {
                    string key = "uploads/" + Guid.NewGuid() + "." + req.extension;

                    CreatePresignedPostRequest presignedPostRequest = new CreatePresignedPostRequest();
                    presignedPostRequest.BucketName = "bizkidz-task-bucket";
                    presignedPostRequest.Key = key;
                    presignedPostRequest.Expires = DateTime.UtcNow.AddMinutes(10);

                    var response = client.CreatePresignedPost(presignedPostRequest);

                    handshake.PresignedUrlPayload = new PresignedPostURLDataModel() { Url = response.Url, Key = key, Fields = response.Fields };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel() { Success = false, ErrorMessage = ex.Message };
            }

            // Create the request in the DB to track actions needed after upload is confirmed
            var request = new ActionRequest()
            {
                UserId = _user.Id,
                ActionType = req.ActionType,
                Status = RequestStatus.Pending,
                Created = DateTimeOffset.UtcNow,
                Updated = DateTimeOffset.UtcNow,
                Expiration = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    // Attach request Entity to DbSet
                    _context.ActionRequests.Add(request);

                    if (request.RequestId == Guid.Empty)
                        throw new Exception("Unable to generate Handshake Request ID.");

                    handshake.RequestId = request.RequestId;

                    // SaveChanges()
                    _context.SaveChanges();

                    // Commit()
                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();

                    return new ResponseModel() { Success = false, ErrorMessage = ex.Message };
                }
                finally
                {
                    t.Dispose();
                }
            }

            // send the handshake required data to the user
            return handshake;
        }

    }
}
