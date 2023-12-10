namespace Shared.API.ViewModels
{
    public class AttachmentResponse
    {
        public AttachmentResponse(int fileId, string fileName, string keyName, string presignedUrl)
        {
            Id = fileId;
            Name = fileName;
            KeyName = keyName;
            PresignedUrl = presignedUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string KeyName { get; set; }
        public string PresignedUrl { get; set; }
    }
}
