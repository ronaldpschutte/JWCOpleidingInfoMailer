using CsvHelper;
using JWCOpleidingInfoMailer.Models;
using JWCOpleidingInfoMailer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using MudBlazor;
using static MudBlazor.Colors;
using static MudBlazor.CategoryTypes;

namespace JWCOpleidingInfoMailer.Pages {
    public partial class Index {
        [Inject]
        private IDialogService DialogService { get; set; }
        [Inject]
        public IPotentialsService potentialsService { get; set; }
        [Inject]
        public IJWCOpleidingService jwcService { get; set; }
        [Inject]
        public ISBBService sBBService { get; set; }

        private MudAutocomplete<JWCOpleiding> JWCSearch;
        private MudTable<PotentialStudentRow> gridStudents;
        private MudTextField<string> SBBOpleidingNaam;
        private List<PotentialStudentRow> GridStudents { get; set; } = new List<PotentialStudentRow>();
        private PotentialStudentRow SelectedStudent { get; set; }
        private int selectedRowNumber = -1;
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
        private void ShowDetail(TableRowClickEventArgs<PotentialStudentRow> tr)
        {
            SelectedStudent = (PotentialStudentRow)tr.Item;
            if (JWCSearch != null)
            {
                SelectedOpleiding = null;
                JWCSearch.Text = "";
            }
            StateHasChanged();
        }
        private async Task<IEnumerable<JWCOpleiding>> Search(string nameSearch)
        {
            if (string.IsNullOrEmpty(nameSearch))
                return jwcService.JWCOpleidingen;
            return jwcService.JWCOpleidingen.Where(x =>
            x.Name.Contains(nameSearch, StringComparison.InvariantCultureIgnoreCase) &&
            x.Level == int.Parse(SelectedStudent.OpleidingNiveau));
        }


        private string SelectedRowClassFunc(PotentialStudentRow element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                
                return string.Empty;
            }
            else if (gridStudents.SelectedItem != null && gridStudents.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }
        private void JWCOpleidingSelected(JWCOpleiding opleiding)
        {
            if (SelectedStudent != null)
            {
                SelectedOpleiding = opleiding;
                SelectedStudent.JWCOpleidingId = opleiding.Id;

                StateHasChanged();
            }
        }
    }
}
