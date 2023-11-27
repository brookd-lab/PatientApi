using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;
using ExternalAPIs.Models;
using System.Configuration;
using System.Reflection;
using PatientApiDAL.Models;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.IO;

namespace ExternalAPIs
{
    public class ExternalApiPatientData
    {
        private const string authentUrl = "https://testapi.mindware.us";
        private const string baseLabVisitUrl = "https://testapi.mindware.us/patient-lab-visits";
        private const string baseLabResultsUrl = "https://testapi.mindware.us/Patient-lab-results";
        private const string baseMedicationsUrl = "https://testapi.mindware.us/patient-medications";
        private const string baseVaccinationsUrl = "https://testapi.mindware.us/Patient-vaccinations";

        private Authent auth;

        private string jwt;

        private readonly HttpClient _client;

        #region Json Helpers

        /// <summary>
        /// Authenticate, set jwt Token, call external api get request, and return response text 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>labVisits</returns>
        private async Task<string> GetResponseJson(string url)
        {
            using (var _client = new HttpClient())
            {
                _client.BaseAddress = new Uri(authentUrl);
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

                var identifier = appConfig.AppSettings.Settings["identifier"];
                var password = appConfig.AppSettings.Settings["password"];

                auth = new Authent()
                {
                    identifier = Encryption.Decrypt(identifier.Value),
                    password = Encryption.Decrypt(password.Value)
                };

                var response = await _client.PostAsJsonAsync(@"/Auth/Local", auth);
                string fullToken = response.Content.ReadAsStringAsync().Result;
                var jsonDoc = JsonDocument.Parse(fullToken);
                //Set JWT Token
                jwt = jsonDoc.RootElement.GetProperty("jwt").ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);
                
                //Get API Endpoint
                var getApiEndpointResponse = await _client.GetAsync(url);
                if (!getApiEndpointResponse.IsSuccessStatusCode)
                {
                    return null;
                    //return $"Error retrieving external api url: {url}";
                }

                var responseText = await getApiEndpointResponse.Content.ReadAsStringAsync();
                return responseText;
            }
        }

        //public string GetPropertyFromJson(JsonDocument json, string searchText)
        //{
        //    var property = json.RootElement.GetProperty(searchText).ToString();
        //    return property;
        //}

        #endregion

        /// <summary>
        /// Get Lab Visits by SSN and returns list of lab results
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>labVisits</returns>
        public async Task<List<PatientLabVisit>?> GetLabVisitsBySSN(string? ssn)
        {
            string url = $"{baseLabVisitUrl}?SSN={ssn}";
            var labVisitsJson = await GetResponseJson(url);
            if (labVisitsJson == null) //Not found
                return null;
            var labVisits = JsonSerializer.Deserialize<List<PatientLabVisit>>(labVisitsJson);
            return labVisits;
        }

        /// <summary>
        /// Get lab results by Lab Visit ID and returns list of lab results
        /// </summary>
        /// <param name="labVisitId"></param>
        /// <returns>labResults</returns>
        public async Task<List<PatientLabResult>?> GetLabResultsByLabVisitId(int labVisitId)
        {
            string url = $"{baseLabResultsUrl}?lab_visit_id={labVisitId}";
            var patentLabResultsJson = await GetResponseJson(url);
            if (patentLabResultsJson == null) //Not found
                return null;
            var labResults = JsonSerializer.Deserialize<List<PatientLabResult>>(patentLabResultsJson);
            return labResults;
        }

        /// <summary>
        /// Get medications by SSN and return list of medications
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>medications</returns>
        public async Task<List<PatientMedication>?> GetMedicationsBySSN(string? ssn)
        {
            string url = $"{baseMedicationsUrl}?SSN={ssn}";
            var medicationsJson = await GetResponseJson(url);
            if (medicationsJson == null) //Not found
                return null;
            var medications = JsonSerializer.Deserialize<List<PatientMedication>>(medicationsJson);
            return medications;
        }

        /// <summary>
        /// Get vaccinations by SSN and return list of vaccinations
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>vaccinations</returns>
        public async Task<List<PatientVaccinationData>?> GetVaccinationsBySSN(string? ssn)
        {
            string url = $"{baseVaccinationsUrl}?SSN={ssn}";
            var vaccinationsJson = await GetResponseJson(url);
            if (vaccinationsJson == null) //Not found
                return null;
            var vaccinations = JsonSerializer.Deserialize<List<PatientVaccinationData>>(vaccinationsJson);
            return vaccinations;
        }
    }
}
