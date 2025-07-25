﻿namespace Gig.Platform.Shared.Dtos
{
    public class RegistrationRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] FileData { get; set; }
        public string FileExtension { get; set; }
        public string Bio { get; set; }
    }
}
