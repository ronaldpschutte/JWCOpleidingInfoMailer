using CsvHelper;
using CsvHelper.Configuration;
using JWCOpleidingInfoMailer.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace JWCOpleidingInfoMailer.Services
{
    public class PotentialsService
    {

        public List<PotentialStudentCSV> Students { get; set; }

        private readonly IWebHostEnvironment _environment;
        public PotentialsService(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
        }

        public async Task UploadFile(IBrowserFile file) 
        {
            try
            {
                Students = new List<PotentialStudentCSV>();
                if (file != null)
                {
                    if (file != null)
                    {
                        Stream stream = file.OpenReadStream();
                        //var path = $"{_environment.WebRootPath}\\Students\\{file.Name}";
                        //FileStream fs = File.Create(path);
                        //await stream.CopyToAsync(fs);

                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ";",
                        };

                        using (var reader = new StreamReader(stream))
                        using (var csv = new CsvReader(reader, config))
                        {
                            await csv.ReadAsync();
                            csv.ReadHeader();
                            while (await csv.ReadAsync())
                            {
                                var student = csv.GetRecord<PotentialStudentCSV>();
                                Students.Add(student);
                            }
                        }


                        stream.Close();
                        //fs.Close();
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
