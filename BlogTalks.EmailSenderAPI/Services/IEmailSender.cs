using BlogTalks.EmailSenderAPI.Models;

namespace BlogTalks.EmailSenderAPI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailDto request);
    }
}
