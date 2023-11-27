
using ExternalAPIs;
using PatientApiDAL.Models;
using PatientApiDAL.Repository;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using PatientAPI;
using PatientAPI.Controllers;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace XunitMoq
{
    public class PatientAPITests : IClassFixture<PatientAPITests>
    {
        Mock<IPatientRepository> _repo = new Mock<IPatientRepository>();
        Mock<ILogger<PatientAPI.Controllers.PatientController>> _logger = new Mock<ILogger<PatientAPI.Controllers.PatientController>>();
        Mock<ExternalApiPatientData> _patientData = new Mock<ExternalApiPatientData>();
        Mock<PatientService>? _patientBackgroundServices = null;
        ExternalApiPatientData _data = new ExternalApiPatientData();
        Encryption _encryption = new Encryption();
        
        [Fact]
        public async Task PerformInternalBackGroundProcesses()
        {
            const string SSN = "111111111";
            
            PatientDetails patient = new PatientDetails()
            {
                id = 10,
                first_name = "Mike",
                middle_name = "A",
                last_name = "Samson",
                dob = DateTime.Now,
                ssn = SSN,
                address = "anywhere street",
                city = "any city",
                state = "CA",
                zip = "94577"
            };

            _patientBackgroundServices = new Mock<PatientService>(
                _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            await controller.RegisterNewPatient(patient);
        }

        [Fact]
        public async Task VerifyAllAPIEndpoints()
        {
            _patientBackgroundServices = new Mock<PatientService>(
                _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            await controller.TestInternal_VerifyAllExternalApiEndpointsWorking();
        }

        [Fact]
        public async Task GetAllDatabaseTables()
        {
            _patientBackgroundServices = new Mock<PatientService>(
              _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);

            await controller.TestInternal_GetAllDatabaseTables();
        }

        [Fact]
        public async Task VerifyStoreInDB()
        {
            PatientLabVisit visit = new PatientLabVisit()
            {
                id = 1,
                patient_id = 5,
                lab_name = "my lab",
                lab_test_request = "my test"
            };

            _patientBackgroundServices = new Mock<PatientService>(
              _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            await controller.TestInternal_VerifyStoreInDB(visit);
        }

        [Fact]
        public async Task AddPatientToDatabase()
        {
            const string SSN = "111111111";
            
            PatientDetails patient = new PatientDetails()
            {
                first_name = "Mike",
                middle_name = "A",
                last_name = "Samson",
                dob = DateTime.Now,
                ssn = SSN,
                address = "anywhere street",
                city = "any city",
                state = "CA",
                zip = "94577"
            };

            _patientBackgroundServices = new Mock<PatientService>(
              _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            int? patientId = await controller.RegisterNewPatient(patient);
        }

        [Fact]
        public async Task TestExternalApiPatientData()
        {
            const string SSN = "111111111";
            const int LAB_VISIT_ID = 1;

            var labVisits = await _data.GetLabVisitsBySSN(SSN);
            var labResults = await _data.GetLabResultsByLabVisitId(LAB_VISIT_ID);
            var medications = await _data.GetMedicationsBySSN(SSN);
            var vaccinations = await _data.GetVaccinationsBySSN(SSN);
        }

        [Fact]
        public async Task IsExistingPatient()
        {
            const string SSN = "111111111";

            PatientDetails patient = new PatientDetails()
            {
                first_name = "Mike",
                middle_name = "A",
                last_name = "Samson",
                dob = DateTime.Now,
                ssn = SSN,
                address = "anywhere street",
                city = "any city",
                state = "CA",
                zip = "94577"
            };

            _patientBackgroundServices = new Mock<PatientService>(
              _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            int? patientId = await controller.IsExistingPatient(SSN);
        }

        [Fact]
        public async Task GetPatientData()
        {
            const int PATIENT_ID = 1;

            _patientBackgroundServices = new Mock<PatientService>(
              _logger.Object, _repo.Object, _patientData.Object);

            var controller = new PatientAPI.Controllers.PatientController(_logger.Object, _repo.Object,
     _patientData.Object, _patientBackgroundServices.Object);
            var patientDetails = await controller.GetPatientData(PATIENT_ID);
        }

        [Fact]
        public async Task TestEncryptDecrypt()
        {
            const string test = "This is a test";
            string encrypted = Encryption.Encrypt(test);
            string decrypted = Encryption.Decrypt(encrypted);
            Assert.True(decrypted == test, $"Incorrect decryption, expect {test}");
        }
    }
}