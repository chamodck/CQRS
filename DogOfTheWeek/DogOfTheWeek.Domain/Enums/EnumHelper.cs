using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain.Enums;

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name!)!
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault()!;
    }

    public static String? GetDescription(this Enum value)
    {
        var description = GetAttribute<DescriptionAttribute>(value);
        return description != null ? description.Description : null;
    }
    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
            typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null)!;
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null)!;
            }
        }
        return default(T)!;
    }
}
