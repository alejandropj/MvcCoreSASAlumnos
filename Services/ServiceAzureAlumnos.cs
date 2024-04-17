using Azure.Data.Tables;
using MvcCoreSASAlumnos.Models;
using Newtonsoft.Json.Linq;
using System;
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
            
            using (HttpClient client = new HttpClient())
            {
                string request = "token/" + curso;
                client.BaseAddress = new Uri(this.UrlApiToken);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response =
                    await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject objetoJson = JObject.Parse(data);
                    string token = objetoJson.GetValue("token").ToString();

                    return token;
                }
                else
                {
                    return null;
                }
            }

            //using (WebClient client = new WebClient())
            //{
            //    string request = "token/" + curso;
            //    client.Headers["content-type"] = "application/json";
            //    Uri uri = new Uri(this.UrlApiToken + request);
            //    string data = await client.DownloadStringTaskAsync(uri);
            //    JObject objetoJson = JObject.Parse(data);
            //    string token = objetoJson.GetValue("token").ToString();

            //    return token;
            //}
        }
        public async Task<List<Alumno>> GetAlumnosAsync(string curso)
        {

            //string token = "http://127.0.0.1:10002/devstoreaccount1/alumnos?tn=alumnos&spk=NET&epk=NET&sv=2020-12-06&se=2024-04-17T09%3A31%3A54Z&sp=r&sig=lT5%2FEhuzXVhKFqdWU%2ByIAQL9DpmNBlnjIvtAw%2FRO8sY%3D";

            string token = await this.GetTokenAsync(curso);
            //creamos el uri con el token

            Uri uriToken = new Uri(token);
            //para acceder al recurso, simplemente creamos el recurso con su uri
            this.tablaAlumnos = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();
            //realizamos una consulta con filter para recuperar todos los alumnos
            var query = this.tablaAlumnos.QueryAsync<Alumno>(filter: "");
            await foreach (var item in query)
            {
                alumnos.Add(item);
            }

            return alumnos;

        }
    }
}
