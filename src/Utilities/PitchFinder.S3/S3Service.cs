using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using PitchFinder.S3.Dtos;
using PitchFinder.S3.Interfaces;

namespace PitchFinder.S3
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;
        private readonly S3Settings _s3Settings;

        public S3Service(IAmazonS3 client, S3Settings s3Settings)
        {
            _client = client;
            _s3Settings = s3Settings;
        }

        public async Task<GetObjectResponse> GetAsync(string keyName)
        {
            var result = await _client.GetObjectAsync(_s3Settings.BucketName, keyName);
            return result;
        }

        public async Task<DeleteObjectResponse> DeleteAsync(string keyName)
        {
            var result = await _client.DeleteObjectAsync(_s3Settings.BucketName, keyName);
            return result;
        }

        public async Task<string> UploadAsync(IFormFile formFile)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _s3Settings.BucketName,
                    Key = Guid.NewGuid().ToString(),
                    InputStream = formFile.OpenReadStream(),
                    ContentType = formFile.ContentType,
                };

                await _client.PutObjectAsync(request);
                return request.Key;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> GetPresignedUrl(string keyName)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = _s3Settings.BucketName,
                Key = keyName,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            return _client.GetPreSignedURL(request);
        }
    }
}
