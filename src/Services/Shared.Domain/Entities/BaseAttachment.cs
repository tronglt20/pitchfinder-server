using Microsoft.AspNetCore.Http;

namespace Shared.Domain.Entities
{
    public class BaseAttachment : BaseEntity<int>
    {
        public BaseAttachment()
        {

        }


        /*public BaseAttachment(IFormFile formfile, string fileKey)
        {
            Name = formfile.FileName;
            KeyName = fileKey;
            Size = formfile.Length;
            Extension = Path.GetExtension(formfile.FileName);
        }*/

        public string Name { get; set; }
        public string KeyName { get; set; }
        public float Size { get; set; }
        public string Extension { get; set; }
    }
}
