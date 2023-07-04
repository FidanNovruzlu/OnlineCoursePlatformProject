using DreamsWebApp.ViewModels.MailSenderVM;

namespace DreamsWebApp.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(MailRequestVM mailRequestVM);
    Task SendEmail(string emailAddress, string subject, string body, bool isHtml);

}
