using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain;

public interface IEntityBase<T>
{
    T Id { get; set; }
    bool IsActive { get; set; }
}
public interface IAuditEntity
{
    DateTime CreatedOn { get; set; }
    DateTime? UpdatedOn { get; set; }

    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}