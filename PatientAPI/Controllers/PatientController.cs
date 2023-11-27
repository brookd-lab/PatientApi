using ExternalAPIs;
using PatientApiDAL.Models;
using PatientApiDAL.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace PatientAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {   
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientRepository _repo;
        private readonly ExternalApiPatientData _apiData; // Data comes form External APIs
        private readonly PatientService _patientService;

        public PatientController(
            ILogger<PatientController> logger,
            IPatientRepository repo,
            ExternalApiPatientData apiData,
            PatientService patientService)
        {
            _logger = logger;
            _repo = repo;
            _apiData = apiData;
            _patientService = patientService;
        }

        #region external api tests

        //Test
        [HttpGet("Test_GetAllDatabaseTablesTest")]
        public async Task TestInternal_GetAllDatabaseTables()
        {
            var patientDetailsList = await _repo.GetAllPatientDetails();
            var patientLabResults = await _repo.GetAllPatientLabResults();
            var patientLabVisits = await _repo.GetAllPatientLabVisits();
            var patientMedications = await _repo.GetAllPatientMedications();
            var patientVaccinationData = await _repo.GetAllPatientVaccinationData();
            var patientVisitHistory = await _repo.GetAllVisitHistory();
        }

        //Test
        [HttpGet("TestInternal_VerifyAllExternalApiEndpointsWorking")]
        public async Task TestInternal_VerifyAllExternalApiEndpointsWorking()
        {
            const string SSN = "111111111";
            const int LABVISITID = 1;

            var data = new ExternalApiPatientData();
            var labVisits = await data.GetLabVisitsBySSN(SSN);
            var labResults = await data.GetLabResultsByLabVisitId(LABVISITID);
            var medications = await data.GetMedicationsBySSN(SSN);
            var vaccinations = await data.GetVaccinationsBySSN(SSN);
        }

        //Test
        [HttpPost("TestInternal_VerifyStoreInDB")]
        public async Task TestInternal_VerifyStoreInDB(
            PatientLabVisit labVisit)
        {
            var labVisits = new List<PatientLabVisit>();
            labVisits.Add(labVisit);
            var labResults = new List<PatientLabResult>();
            var medications = new List<PatientMedication>();
            var vaccinations = new List<PatientVaccinationData>();
            var labVisitSuccess = await _repo.StoreLabVisitsInDB(labVisits);
            var labResultSuccess = await _repo.StoreLabResultsInDB(labResults);
            var patientMedicationSuccess = await _repo.StoreMedicationsInDB(medications);
            var patientVaccinationsSuccess = await _repo.StoreVaccinationsInDB(vaccinations);
        }

        #endregion

        #region Internal Api Methods for Front-End Consumption

        // If ssn is in database return patientID else return 0
        [HttpGet("isExistingPatient")]
        public async Task<int> IsExistingPatient(string ssn)
        {
            var patient = await _repo.GetPatientDetailsBySSN(ssn);
            return patient == null ? 0 : patient.id;
        }

        // return patient details from patientID
        [HttpGet("getPatientData")]
        public async Task<PatientDetails?> GetPatientData(int patientID)
        {
            var patient = await _repo.GetPatientDetailsByPatientID(patientID);
            return patient;
        }

        /// <summary>
        /// If patient Exists, return 0,
        ///   else Return newly created patientId after adding patient
        // calls PerformExternalBackgroundProcesses
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>newPatientId</returns>
        [HttpPost("registerNewPatient")]
        public async Task<int?> RegisterNewPatient(PatientDetails patient)
        {
            var patientId = await IsExistingPatient(patient.ssn);
            var patientDetails = await GetPatientData(patientId);
            int newPatientId = 0;

            if (patientId == 0 && patientDetails == null)
            {
                newPatientId = await _repo.AddNewPatientToDB(patient);
                await _patientService.PerformBackgroundProcesses(patient);
            }

            return newPatientId;
        }
    }

    #endregion
}