using Google.Type;
using System.Xml.Linq;

namespace WebAppCondominio.Models
{
	public class Visit
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Vehicle { get; set; }
		public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Date { get; set; }
    }
}