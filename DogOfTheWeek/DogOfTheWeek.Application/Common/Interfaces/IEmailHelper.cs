using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Interfaces;

public interface IEmailHelper
{
    public bool SendEmailConfirm(string userEmail, string confirmationLink);
    public bool SendEmailPasswordReset(string userEmail, string confirmationLink);
}
