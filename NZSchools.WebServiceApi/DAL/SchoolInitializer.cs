using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NZSchools.WebServiceApi.DAL
{
    // DropCreateDatabaseIfModelChanges
    public class SchoolInitializer : DropCreateDatabaseAlways<SchoolContext>
    {

        public SchoolInitializer()
        {

        }
    }
}