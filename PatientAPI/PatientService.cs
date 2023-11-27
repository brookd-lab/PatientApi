using ExternalAPIs;
using PatientApiDAL.Models;
using PatientApiDAL.Repository;
using Microsoft.AspNetCore.Mvc;
using PatientAPI.Controllers;
using System.Threading;

namespace PatientAPI
{
    public class PatientService
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientRepository _repo;
        private readonly ExternalApiPatientData _apiData; // Data comes form External APIs
        
        public PatientService(
            ILogger<PatientController> logger,
            IPatientRepository repo,
            ExternalApiPatientData apiData)
        {
            _logger = logger;
            _repo = repo;
            _apiData = apiData;
        }

        /// <summary>
        /// Calls:
        ///   GetLabVisitsBySSN/StoreLabVisitsInDB,
        ///   GetLabResultsByLabVisitId/StoreLabResultsInDB
        ///   GetMedicationsBySSN/StoreMedicationsInDB
        ///   GetVaccinationsBySSN/StoreVaccinationsInDB
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public async Task PerformBackgroundProcesses(PatientDetails patient)
        {
            var labVisits = await _apiData.GetLabVisitsBySSN(patient.ssn);

            if (labVisits != null && labVisits.Count > 0)
            {
                //Get Lab Results by Lab Visit ID and store in DB
                foreach (PatientLabVisit labVisit in labVisits)
                {
                    var labResults = await _apiData.GetLabResultsByLabVisitId(labVisit.id);
                    // if null, no lab results found, so do not store in DB
                    if (labResults != null && labResults.Count > 0)
                    {
                        var labResultSuccess = await _repo.StoreLabResultsInDB(labResults);
                        if (!labResultSuccess) _logger.LogError($"Error storing Lab Results in DB for lab visit id: {labVisit.id}");
                    }
                }
            }

            //True if successful storing Lab Visits in DB, False if failure
            var labVisitSuccess = await _repo.StoreLabVisitsInDB(labVisits);

            //TODO: find a better way like returning a list of exceptions so that the 
            //UI can respond.
            if (!labVisitSuccess)
                _logger.LogError($"Error storing Lab Visits in DB for patient ssn: {patient.ssn}");


            //get medications by ssn and store in dB
            var medications = await _apiData.GetMedicationsBySSN(patient.ssn);
            // if null, no medications found, so do not store in DB
            if (medications != null && medications.Count > 0)
            {
                var patientMedicationSuccess = await _repo.StoreMedicationsInDB(medications);
                if (!patientMedicationSuccess) _logger.LogError($"Error storing Patient Medications in DB for ssn: {patient.ssn}");
            }

            //get vaccinations by ssn and store in DB
            var vaccinations = await _apiData.GetVaccinationsBySSN(patient.ssn);
            if (vaccinations != null && vaccinations.Count > 0) //if null, no vaccinations found, so do not store in DB
            {
                var patientVaccinationsSuccess = await _repo.StoreVaccinationsInDB(vaccinations);
                if (!patientVaccinationsSuccess) _logger.LogError($"Error storing Patient Vaccinations in DB for ssn: {patient.ssn}");
            }
        } 
    }
}
