using System;
using System.Collections.Generic;
using System.Text;

namespace NZSchools.Services.JsonService
{
    public interface IJsonService
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
    }
}
