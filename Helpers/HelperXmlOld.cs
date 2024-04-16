namespace MvcCoreSASAlumnos.Helpers
{
    public enum Folders { Images = 0, Documents = 1 };
    public class HelperXmlOld
    {
        IWebHostEnvironment environment;
        public HelperXmlOld(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            else
            {
                carpeta = "documents";

            }
            string path = Path.Combine(this.environment.ContentRootPath, carpeta, fileName);
            return path;
        }
    }
}
