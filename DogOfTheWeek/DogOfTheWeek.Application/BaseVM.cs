using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application;

public class BaseVM
{
    public virtual bool IsActive { get; set; } = true;
    public virtual long Id { get; set; }
    public virtual DateTime CreatedOn { get; set; }
    public virtual string? CreatedBy { get; set; }
    public virtual DateTime? UpdatedOn { get; set; }
    public virtual string? UpdatedBy { get; set; }
}
