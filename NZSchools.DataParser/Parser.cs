using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZSchools.DataParser.Models;
using NZSchools.DataParser.Services.ExcelReaderService;
using NZSchools.DataParser.Services.RestService;
using NZSchools.DataParser.Services.JsonService;

namespace NZSchools.DataParser
{
    public class Parser
    {
        private const string WebServiceUrl = "http://localhost:55561/api/";
        private const string DirectoryFilePath = "Data\\Directory.xls";

        private readonly IExcelReaderService _excel;
        private readonly IJsonService _json;
        private readonly IRestService _rest;

        public Parser(IExcelReaderService excel, IJsonService json, IRestService rest)
        {
            _excel = excel;
            _json = json;
            _rest = rest;
        }

        public async Task Begin()
        {
            var directories = _excel.Read<Directory>(DirectoryFilePath, 3);
            if (directories == null)
                return;

            var apiDirectories = await _rest.GetApi<Directory>(WebServiceUrl, "directories");
            if (apiDirectories == null)
                return;

            CompareDirectories(directories.ToList(), apiDirectories.Where(x => x.Status).ToList());
        }
        
        private async void CompareDirectories(List<Directory> currentList, List<Directory> apiList)
        {
            var maxId = apiList.Any() ? apiList.Max(x => x.Id) + 1 : 1;
            var maxChangeId = apiList.Any() ? apiList.Max(x => x.ChangeId) + 1 : 1;

            foreach (var c in currentList)
            {
                if (!apiList.Any(x => x.SchoolId == c.SchoolId))
                {
                    c.Id = maxId++;
                    c.ChangeId = maxChangeId++;
                    c.Status = true;
                    if (await _rest.PostApi(WebServiceUrl, "directories", _json.Serialize(c)))
                        Logger.Info("Inserted: " + c.SchoolId);
                }
            }

            foreach (var a in apiList)
            {
                if (!currentList.Any(x => x.SchoolId == a.SchoolId))
                {
                    a.ChangeId = maxChangeId++;
                    a.Status = false;
                    if (await _rest.PutApi(WebServiceUrl, "directories", _json.Serialize(a)))
                        Logger.Info("Removed: " + a.SchoolId);
                }
                else
                {
                    try
                    {
                        var c = currentList.SingleOrDefault(x => x.SchoolId == a.SchoolId);

                        if (c.Asian != a.Asian || c.Authority != a.Authority || c.CensusAreaUnit != a.CensusAreaUnit
                            || c.City != a.City || c.Decile != a.Decile || c.Definition != a.Definition || c.EducationRegion != a.EducationRegion
                            || c.Email != a.Email || c.EuropeanPākehā != a.EuropeanPākehā || c.Fax != a.Fax || c.GenderOfStudents != a.GenderOfStudents
                            || c.GeneralElectorate != a.GeneralElectorate || c.InternationalStudents != a.InternationalStudents
                            || c.Latitude != a.Latitude || c.Longitude != a.Longitude || c.Māori != a.Māori || c.MāoriElectorate != a.MāoriElectorate
                            || c.Melaa != a.Melaa || c.MinistryOfEducationLocalOffice != a.MinistryOfEducationLocalOffice
                            || c.Name != a.Name || c.Other != a.Other || c.Pasifika != a.Pasifika || c.PostalAddress1 != a.PostalAddress1
                            || c.PostalAddress2 != a.PostalAddress2 || c.PostalAddress3 != a.PostalAddress3 || c.PostalCode != a.PostalCode
                            || c.Principal != a.Principal || c.RegionalCouncil != a.RegionalCouncil || c.SchoolType != a.SchoolType
                            || c.SchoolWebsite != a.SchoolWebsite || c.Street != a.Street || c.Suburb != a.Suburb || c.Telephone != a.Telephone
                            || c.TerritorialAuthorityWithAucklandLocalBoard != a.TerritorialAuthorityWithAucklandLocalBoard
                            || c.TotalSchoolRoll != a.TotalSchoolRoll || c.UrbanArea != a.UrbanArea || c.Ward != a.Ward)
                        {
                            c.ChangeId = maxChangeId++;
                            c.Id = a.Id;

                            if (await _rest.PutApi(WebServiceUrl, "directories", _json.Serialize(c)))
                                Logger.Info("Updated: " + a.SchoolId);
                        }
                    }
                    catch (InvalidOperationException ioe)
                    {
                        Logger.Error(ioe);
                        Logger.Error("Found more than one result: " + a.SchoolId);
                        continue;
                    }
                }
            }            
        }
    }
}
