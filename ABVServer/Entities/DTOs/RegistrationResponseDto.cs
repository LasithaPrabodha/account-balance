using System.Collections.Generic;

namespace Entities.DTOs
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public List<string> Errors { get; set; }
    }
}
