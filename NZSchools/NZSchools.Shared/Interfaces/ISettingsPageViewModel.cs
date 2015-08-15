using System;
using System.Collections.Generic;
using System.Text;

namespace NZSchools.Interfaces
{
    public interface ISettingsPageViewModel
    {
        int GetDefaultRegion();
        bool GetGps();
    }
}
