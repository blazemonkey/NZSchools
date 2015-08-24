using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NZSchools.Models
{
    public class Directory : ModelBase
    {
        private bool _isFavourites;

        [PrimaryKey]
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Principal { get; set; }
        public string SchoolWebsite { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalAddress3 { get; set; }
        public string PostalCode { get; set; }
        public string UrbanArea { get; set; }
        public string SchoolType { get; set; }
        public string Definition { get; set; }
        public string Authority { get; set; }
        public string GenderOfStudents { get; set; }
        public string TerritorialAuthorityWithAucklandLocalBoard { get; set; }
        public string RegionalCouncil { get; set; }
        public string MinistryOfEducationLocalOffice { get; set; }
        public string EducationRegion { get; set; }
        public string GeneralElectorate { get; set; }
        public string MāoriElectorate { get; set; }
        public string CensusAreaUnit { get; set; }
        public string Ward { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Decile { get; set; }
        public int TotalSchoolRoll { get; set; }
        public int EuropeanPākehā { get; set; }
        public int Māori { get; set; }
        public int Pasifika { get; set; }
        public int Asian { get; set; }
        public int Melaa { get; set; }
        public int Other { get; set; }
        public int InternationalStudents { get; set; }
        public int ChangeId { get; set; }
        public bool Status { get; set; }
        public bool IsFavourites 
        {
            get { return _isFavourites; }
            set
            {
                _isFavourites = value;
                RaisePropertyChanged("IsFavourites");
            }
        }
        [Ignore]
        public string AddressCombined
        {
            get 
            {
                var combined = Street;
                if (!string.IsNullOrEmpty(Suburb))
                    combined = combined + ", " + Suburb;

                combined = combined + ", " + City;

                return combined;
            }
        }
        [Ignore]
        public string PostalCombined
        {
            get
            {                
                return string.Format("{0}, {1}, {2}", PostalAddress1, PostalAddress3, PostalCode);
            }
        }
    }
}
