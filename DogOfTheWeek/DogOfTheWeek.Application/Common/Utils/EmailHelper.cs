using DogOfTheWeek.Application.Common.Interfaces;
using DogOfTheWeek.Application.Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Utils;

public class EmailHelper : IEmailHelper
{
    private readonly AppData _appData;
    public EmailHelper(IOptions<AppData> options)
    {
        this._appData = options.Value;
    }
    public bool SendEmailConfirm(string userEmail, string confirmationLink)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_appData.MAIL);
        mailMessage.To.Add(new MailAddress(userEmail));

        mailMessage.Subject = "Confirm your email";
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = confirmationLink;

        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential(_appData.MAIL, _appData.MAIL_PASSWORD);
        client.Host = _appData.MAIL_HOST;
        client.Port = _appData.MAIL_PORT;
        client.EnableSsl = _appData.ENABLE_SSL;
        try
        {
            client.Send(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public bool SendEmailPasswordReset(string userEmail, string confirmationLink)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_appData.MAIL);
        mailMessage.To.Add(new MailAddress(userEmail));

        mailMessage.Subject = "Reset Password";
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = confirmationLink;

        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential(_appData.MAIL, _appData.MAIL_PASSWORD);
        client.Host = _appData.MAIL_HOST;
        client.Port = _appData.MAIL_PORT;
        client.EnableSsl = _appData.ENABLE_SSL;
        try
        {
            client.Send(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
}