using Azure.Data.Tables;
using MvcCoreSASAlumnos.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MvcCoreSASAlumnos.Services
{
    public class ServiceAzureAlumnos
    {
        private TableClient tablaAlumnos;
        private string UrlApiToken;

        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            this.UrlApiToken = configuration
                .GetValue<string>("ApiUrls:ApiTokenSaS");
        }

        public async Task<string> GetTokenAsync(string curso)
        {
            using(WebClient client = new WebClient())
            {
                string request = "token/" + curso;
                client.Headers["content-type"] = "application/json";
                Uri uri = new Uri(this.UrlApiToken + request);
                string data = await client.DownloadStringTaskAsync(uri);
                JObject objetoJson = JObject.Parse(data);
                string token = objetoJson.GetValue("token").ToString();
                return token;
            }
        }
        public async Task<List<Alumno>>
            GetAlumnosAsync(string curso)
        {
            string token = await this.GetTokenAsync(curso);
            Uri uriToken = new Uri(token);
            this.tablaAlumnos = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();
            var query = this.tablaAlumnos.QueryAsync<Alumno>
                (filter: "");
            await foreach (var item in query)
            {
                alumnos.Add(item);
            }

            return alumnos;
        }
    }
}
