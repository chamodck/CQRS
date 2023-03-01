using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain;

public abstract class BaseEntity<T> : IEntityBase<T>, IAuditEntity
{
    public BaseEntity()
    {
        CreatedOn = DateTime.Now;
        IsActive = true;
    }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public T Id { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}