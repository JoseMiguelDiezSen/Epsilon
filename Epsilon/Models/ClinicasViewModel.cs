using Epsilon.ViewModels;

namespace Epsilon.Models
{
    public class ClinicasViewModel
    {
        public int IdClinica { get; set; }
        public string? NombreClinica { get; set; }
        public string? DireccionClinica { get; set; }
        public string? LocalidadClinica { get; set; }
        public string? TelefonoClinica { get; set; }
        public string? EMailClinica { get; set; }
        public string? DirectorClinica { get; set; }

        //public IEnumerable<ViewMedicos> Pacientes { get; set; } = Enumerable.Empty<ViewMedicos>();
    }
}
