using CsvHelper.Configuration;
using CsvHelper;
using JWCOpleidingInfoMailer.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace JWCOpleidingInfoMailer.Services
{
    public interface IJWCOpleidingService
    {
        List<JWCOpleiding> JWCOpleidingen { get; set; }

        Task ReadSBBFile();
    }

    public class JWCOpleidingService : IJWCOpleidingService
    {

        public List<JWCOpleiding> JWCOpleidingen { get; set; } = new List<JWCOpleiding> { };

        private readonly IWebHostEnvironment _environment;
        public JWCOpleidingService(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        //create method to read csv file with csvhelper
        public async Task ReadSBBFile()
        {

            try
            {
                if (JWCOpleidingen.Count == 0)
                {
                    var path = $"{_environment.WebRootPath}\\JWC\\JWCOpleidingen.csv";


                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";",
                    };

                    using (var reader = new StreamReader(path))
                    using (var csv = new CsvReader(reader, config))
                    {
                        await csv.ReadAsync();
                        csv.ReadHeader();
                        while (await csv.ReadAsync())
                        {
                            var opleiding = csv.GetRecord<JWCOpleiding>();
                            JWCOpleidingen.Add(opleiding);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
