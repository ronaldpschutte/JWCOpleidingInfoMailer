using CsvHelper.Configuration;
using CsvHelper;
using JWCOpleidingInfoMailer.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace JWCOpleidingInfoMailer.Services
{
    public interface ISBBService
    {
        List<SBBCreboBeschrijving> SBBOpleidingen { get; set; }

        Task ReadSBBFile();
        Task UploadFile(IBrowserFile file);
    }

    public class SBBService : ISBBService
    {

        public List<SBBCreboBeschrijving> SBBOpleidingen { get; set; } = new List<SBBCreboBeschrijving> { };

        private readonly IWebHostEnvironment _environment;
        public SBBService(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        //create method to read csv file with csvhelper
        public async Task ReadSBBFile()
        {

            try
            {
                var path = $"{_environment.WebRootPath}\\SBBFile\\crebolijst2022.csv";


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
                        var opleiding = csv.GetRecord<SBBCreboBeschrijving>();
                        SBBOpleidingen.Add(opleiding);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }



        public async Task UploadFile(IBrowserFile file)
        {

            if (file != null)
            {
                Stream stream = file.OpenReadStream();
                var path = $"{_environment.WebRootPath}\\SBBFile\\{file.Name}";
                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
            }
        }

    }
}
