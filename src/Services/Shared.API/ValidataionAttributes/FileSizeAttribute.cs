using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.API.ValidataionAttributes
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public FileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            IEnumerable<IFormFile> files = value switch
            {
                IFormFile file => new[] { file },
                IEnumerable<IFormFile> fileList => fileList,
                _ => throw new ArgumentException("This attribute can only be applied to properties that are either IFormFile or IEnumerable<IFormFile>.")
            };

            foreach (var file in files)
            {
                if (file.Length > _maxFileSize)
                {
                    ErrorMessage = $"The file size of {file.FileName} cannot exceed {_maxFileSize} bytes.";
                    return false;
                }
            }

            return true;
        }
    }
}
