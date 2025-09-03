using System.Net;
using System.Net.Mail;
using Infrastructure.EmailAuth;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor) : IEmailSender
{
    private AuthMessageSenderOptions Options { get; } = optionsAccessor.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(Options.GmailKey)) throw new Exception("Could not get email key");
        await Execute(subject, htmlMessage, email);
    }

    private async Task Execute(string subject, string message, string toEmail)
    {
        var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
        {
            UseDefaultCredentials = false,
            Port = 587,
            Credentials = new NetworkCredential(Options.AdminEmail, Options.GmailKey),
            EnableSsl = true,
        });
        
        Email.DefaultSender = sender;
        var email = await Email.From(Options.AdminEmail).To(toEmail).Subject(subject).Body(message, true).SendAsync();
    }
}
