using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Intrinsics.X86;

namespace ExternalAPIs
{
    public class Program
    {
        //Unit Testing of External Apis
        static async Task Main(string[] args)
        {
            const string SSN = "111111111";
            const int LABVISITID = 1;

            var data = new ExternalApiPatientData();
            var labVisits = await data.GetLabVisitsBySSN(SSN);
            var labResults = await data.GetLabResultsByLabVisitId(LABVISITID);
            var medications = await data.GetMedicationsBySSN(SSN);
            var vaccinations = await data.GetVaccinationsBySSN(SSN);
        }

   

       
    }
}











