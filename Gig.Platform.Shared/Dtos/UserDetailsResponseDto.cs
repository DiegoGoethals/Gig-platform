namespace Gig.Platform.Shared.Dtos
{
    public class UserDetailsResponseDto
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public ICollection<string> Skills { get; set; }
        public ICollection<ReviewResponseDto> Reviews { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
