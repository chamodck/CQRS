using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogOfTheWeek.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRegisterRoleEnum
{
    [Description("DogOwner")]
    DogOwner = 1,
    [Description("DogLover")]
    DogLover = 2,
}