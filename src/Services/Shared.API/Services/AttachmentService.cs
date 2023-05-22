using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PitchFinder.S3.Interfaces;
using Shared.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Shared.API.Services
{
    public class AttachmentService<T, Tcontext> where T : BaseAttachment
                                                where Tcontext : DbContext
    {
        private readonly IS3Service _s3Service;
        private readonly IAttachmentRepository<T, Tcontext> _attachmentRepo;
        public T Attachment { get; set; }
        public AttachmentService(IS3Service s3Service
            , IAttachmentRepository<T, Tcontext> attachmentRepo)
        {
            _s3Service = s3Service;
            _attachmentRepo = attachmentRepo;
            Attachment = (T)Activator.CreateInstance(typeof(T));
        }

        public async Task<T> UploadAsync(IFormFile formFile)
        {
            var fileKey = await _s3Service.UploadAsync(formFile);

            Attachment.Name = formFile.FileName;
            Attachment.KeyName = fileKey;
            Attachment.Size = formFile.Length;
            Attachment.Extension = Path.GetExtension(formFile.FileName);

            await _attachmentRepo.InsertAsync(Attachment);
            return Attachment;
        }


        public async Task<string> GetPresignedUrl(string keyName)
        {
            return await _s3Service.GetPresignedUrl(keyName);
        }

    }
}
