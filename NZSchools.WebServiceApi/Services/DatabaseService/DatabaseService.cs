using NZSchools.WebServiceApi.DAL;
using NZSchools.WebServiceApi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NZSchools.WebServiceApi.Services.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SchoolContext _db;

        public DatabaseService()
        {
            _db = new SchoolContext();
        }

        public async Task<IEnumerable<Directory>> GetDirectories()
        {
            return await _db.Directories.ToListAsync();
        }

        public async Task<Directory> GetDirectoryById(int id)
        {
            return await _db.Directories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateDirectory(Directory directory)
        {
            var find = await _db.Directories.FindAsync(directory.Id);
            if (find == null)
                return false;

            find.Asian = directory.Asian;
            find.Authority = directory.Authority;
            find.CensusAreaUnit = directory.CensusAreaUnit;
            find.ChangeId = directory.ChangeId;
            find.City = directory.City;
            find.Decile = directory.Decile;
            find.Definition = directory.Definition;
            find.EducationRegion = directory.EducationRegion;
            find.Email = directory.Email;
            find.EuropeanPākehā = directory.EuropeanPākehā;
            find.Fax = directory.Fax;
            find.GenderOfStudents = directory.GenderOfStudents;
            find.GeneralElectorate = directory.GeneralElectorate;
            find.InternationalStudents = directory.InternationalStudents;
            find.Latitude = directory.Latitude;
            find.Longitude = directory.Longitude;
            find.Māori = directory.Māori;
            find.MāoriElectorate = directory.MāoriElectorate;
            find.Melaa = directory.Melaa;
            find.MinistryOfEducationLocalOffice = directory.MinistryOfEducationLocalOffice;            
            find.Name = directory.Name;
            find.Other = directory.Other;
            find.Pasifika = directory.Pasifika;
            find.PostalAddress1 = directory.PostalAddress1;
            find.PostalAddress2 = directory.PostalAddress2;
            find.PostalAddress3 = directory.PostalAddress3;
            find.PostalCode = directory.PostalCode;
            find.Principal = directory.Principal;
            find.RegionalCouncil = directory.RegionalCouncil;
            find.SchoolId = directory.SchoolId;
            find.SchoolType = directory.SchoolType;
            find.SchoolWebsite = directory.SchoolWebsite;
            find.Status = directory.Status;
            find.Street = directory.Street;
            find.Suburb = directory.Suburb;
            find.Telephone = directory.Telephone;
            find.TerritorialAuthorityWithAucklandLocalBoard = directory.TerritorialAuthorityWithAucklandLocalBoard;
            find.TotalSchoolRoll = directory.TotalSchoolRoll;
            find.UrbanArea = directory.UrbanArea;
            find.Ward = directory.Ward;            
            
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDirectory(Directory directory)
        {
            var find = await _db.Directories.FindAsync(directory.Id);
            if (find != null)
                return false;

            _db.Directories.Add(directory);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDirectory(int id)
        {
            var find = await _db.Directories.FindAsync(id);
            if (find == null)
                return false;

            _db.Directories.Remove(find);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}