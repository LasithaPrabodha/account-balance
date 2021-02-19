using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class UploadBalance
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public IFormFile File { get; set; }

    }
}
