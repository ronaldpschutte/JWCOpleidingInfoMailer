using JWCOpleidingInfoMailer.Models;
using JWCOpleidingInfoMailer.Services;
using Microsoft.AspNetCore.Components;

namespace JWCOpleidingInfoMailer.Pages {
    public partial class SBBFile {
        [Inject]
        public SBBService sBBService { get; set; }
        private List<PotentialStudentRow> GridStudents { get; set; } = new List<PotentialStudentRow>();

        protected async override System.Threading.Tasks.Task OnInitializedAsync() 
        {
            try 
            {
                await sBBService.ReadSBBFile();

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }
        private void FileUpload() 
        {

        }
    }
}
