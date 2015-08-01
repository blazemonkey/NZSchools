using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NZSchools.WebServiceApi.DAL
{
    // DropCreateDatabaseAlways
    public class SchoolInitializer : DropCreateDatabaseIfModelChanges<SchoolContext>
    {

        public SchoolInitializer()
        {

        }
    }
}