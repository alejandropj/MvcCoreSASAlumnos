using MvcCoreSASAlumnos.Models;
using System.Xml.Linq;

namespace MvcCoreSASAlumnos.Helpers
{
    public class HelperXml
    {
        private XDocument document;
        public HelperXml()
        {
            string assemblyPath = "MvcCoreSASAlumnos.Documents.alumnos_tables.xml";
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(assemblyPath);
            this.document = XDocument.Load(stream);
        }

        public List<Alumno> GetAlumnos()
        {
            var consulta = from datos in document.Descendants("alumno")
                           select datos;
            List<Alumno> alumnosList = new List<Alumno>();
            foreach (XElement tag in consulta)
            {
                Alumno alumno = new Alumno();
                alumno.IdAlumno = int.Parse(tag.Element("idalumno").Value);
                alumno.Curso = tag.Element("curso").Value;
                alumno.Nombre = tag.Element("nombre").Value;
                alumno.Apellidos = tag.Element("apellidos").Value;
                alumno.Nota = int.Parse(tag.Element("nota").Value);
                alumnosList.Add(alumno);
            }
            return alumnosList;
        }
    } 
}
