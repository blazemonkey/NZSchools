﻿using NZSchools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NZSchools.Services.SqlLiteService
{
    public interface ISqlLiteService
    {
        Task<IEnumerable<Directory>> GetDirectories();
        Task<IEnumerable<Directory>> GetFavourites();
        Task<IEnumerable<Region>> GetRegions();
        Task UpdateFavourites(Directory directory);
    }
}
