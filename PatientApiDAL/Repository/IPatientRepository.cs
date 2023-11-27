using PatientApiDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApiDAL.Repository
{
    public interface IPatientRepository
    {
        #region test external apis
        Task<IEnumerable<PatientDetails>> GetAllPatientDetails();
        Task<IEnumerable<PatientLabResult>> GetAllPatientLabResults();
        Task<IEnumerable<PatientLabVisit>> GetAllPatientLabVisits();
        Task<IEnumerable<PatientMedication>> GetAllPatientMedications();
        Task<IEnumerable<PatientVaccinationData>> GetAllPatientVaccinationData();
        Task<IEnumerable<PatientVisitHistory>> GetAllVisitHistory();

        #endregion

        #region internal api, store to DB 
        Task<PatientDetails?> GetPatientDetailsBySSN(string ssn);
        Task<PatientDetails?> GetPatientDetailsByPatientID(int patientId);
        Task<int> AddNewPatientToDB(PatientDetails patient);
        Task<bool> StoreLabVisitsInDB(List<PatientLabVisit> patientLabVisits);
        Task<bool> StoreLabResultsInDB(List<PatientLabResult> patientLabResults);
        Task<bool> StoreMedicationsInDB(List<PatientMedication> patientMedications);
        Task<bool> StoreVaccinationsInDB(List<PatientVaccinationData> patientVaccinations);

        #endregion
    }
}
