using CsvHelper;
using JWCOpleidingInfoMailer.Models;
using JWCOpleidingInfoMailer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using MudBlazor;
using JWCOpleidingInfoMailer.Component;
using static MudBlazor.Colors;

namespace JWCOpleidingInfoMailer.Pages {
    public partial class Index {
        [Inject] 
        private IDialogService DialogService { get; set; }
        [Inject]
        public PotentialsService potentialsService { get; set; }
        [Inject]
        public JWCOpleidingService jwcService { get; set; }
        [Inject]
        public SBBService sBBService { get; set; }
        private List<PotentialStudentRow> GridStudents { get; set; } = new List<PotentialStudentRow>();
        private PotentialStudentRow SelectedStudent { get; set; }
        private JWCOpleiding SelectedOpleiding { get; set; }
        IList<IBrowserFile> files = new List<IBrowserFile>();

        protected async override System.Threading.Tasks.Task OnInitializedAsync() {
            try 
            {
                await jwcService.ReadSBBFile();

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
           
        private async Task UploadFile(IBrowserFile file)
        {
            try
            {
                files.Add(file);
                await sBBService.ReadSBBFile();
                await potentialsService.UploadFile(file);

                foreach (var student in potentialsService.Students)
                {
                    var Opleiding = sBBService.SBBOpleidingen.FirstOrDefault(c => c.Crebonummer == student.Crebonummer);
                    var gr = new PotentialStudentRow()
                    {
                        Voornaam = student.Voornaam,
                        Tussenvoegsel = student.Tussenvoegsel,
                        Achternaam = student.Achternaam,
                        Email = student.Email,
                        Crebonummer = student.Crebonummer,
                        OpleidingNaam = Opleiding?.Kwalificatie ?? "Crebonummer niet gevonden",
                        OpleidingNiveau = Opleiding?.Niveau

                    };
                    //Lookup SBB table for match
                    GridStudents.Add(gr);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void OpenForm(TableRowClickEventArgs<PotentialStudentRow> tr)
        {
            SelectedStudent = (PotentialStudentRow)tr.Item;

            StateHasChanged();
        }    
        private async Task<IEnumerable<JWCOpleiding>> Search(string name)
        {
            if (string.IsNullOrEmpty(name))
                return jwcService.JWCOpleidingen;
            return jwcService.JWCOpleidingen.Where(x => 
            x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase) &&
            x.Level == int.Parse(SelectedStudent.OpleidingNiveau));
        }

    }
}
