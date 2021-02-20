using Microsoft.AspNetCore.Http;
using System;

namespace Entities.Models
{
    public class UploadBalance
    {
        public string TransactionDate { get; set; }
        public IFormFile File { get; set; }

    }
}
