using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Models;

public class AppData
{
    public string CLIENT_APP_URL { get; set; }
    public string ADMIN_PORTAL_URL { get; set; }
    public string MAIL { get; set; }
    public string MAIL_PASSWORD { get; set; }
    public string MAIL_HOST { get; set; }
    public int MAIL_PORT { get; set; }
    public bool ENABLE_SSL { get; set; }
}
