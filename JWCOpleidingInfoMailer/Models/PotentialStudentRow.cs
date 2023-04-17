namespace JWCOpleidingInfoMailer.Models
{
    public class PotentialStudentRow
    {

        public string? Voornaam { get; set; }
        public string? Tussenvoegsel{ get; set; }
        public string? Achternaam { get; set; }
        public string? Email { get; set; }
        public string? Crebonummer { get; set; }
        public string? OpleidingNaam { get; set;}
        public string? OpleidingNiveau { get; set; }
        public Boolean? Match { get; set; }
    }
}
