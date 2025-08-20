using BlogTalks.EmailSenderAPI.Models;

namespace BlogTalks.EmailSenderAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request);
    }
}
