using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogOfTheWeek.Application.Common.Models
{
    public class Response<T>
    {
        public Response()
        {
            Success = true;
        }
        public Response(T data) : this()
        {
            Result = data;
        }
        public Response(T data, string error) : this(data)
        {
            Error = error;
            Success = false;
        }
        public Response(T data, string error, IDictionary<string, string[]> errors) : this(data)
        {
            Error = error;
            Errors = errors;
            Success = false;
        }
        public Response(T data, IDictionary<string, string[]> errors) : this(data)
        {
            Errors = errors;
            Success = false;
        }

        [Obsolete]
        public string Error { get; internal set; }
        public IDictionary<string, string[]> Errors { get; internal set; }
        public T Result { get; internal set; }
        public bool Success { get; internal set; }
    }
}
