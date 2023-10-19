using Google.Type;
using System.Xml.Linq;

namespace WebAppCondominio.Models
{
	public class Visitas
	{
		public string? Cedula { get; set; }
		public string? Nombre { get; set; }
		public string? Vehiculo { get; set; }
		public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Color { get; set; }
        public string? Fecha { get; set; }
    }
}