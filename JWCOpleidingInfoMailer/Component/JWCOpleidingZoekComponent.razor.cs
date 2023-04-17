using CsvHelper;
using JWCOpleidingInfoMailer.Models;
using JWCOpleidingInfoMailer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using MudBlazor;

namespace JWCOpleidingInfoMailer.Component {
    public partial class JWCOpleidingZoekComponent : ComponentBase 
    {
        [Inject]
        public PotentialsService potentialsService { get; set; }
        [Inject]
        public SBBService sBBService { get; set; }

        [CascadingParameter]
        public PotentialStudentRow? Student {get; set;}
        protected async override System.Threading.Tasks.Task OnInitializedAsync() {
            try 
            {
                //Validate SBB file

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
