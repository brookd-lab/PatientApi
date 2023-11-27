using PatientApiDAL.Data;
using PatientApiDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApiDAL.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<IEnumerable<PatientDetails>> IPatientRepository.GetAllPatientDetails()
        {
            var patients = await _context.Patient_Details.ToListAsync();
            return patients;
        }
        async Task<IEnumerable<PatientLabResult>> IPatientRepository.GetAllPatientLabResults()
        {
            var labResults = await _context.Patient_Lab_Result.ToListAsync();
            return labResults;
        }
        async Task<IEnumerable<PatientLabVisit>> IPatientRepository.GetAllPatientLabVisits()
        {
            var labVisits = await _context.Patient_Lab_Visit.ToListAsync();
            return labVisits;
        }
        async Task<IEnumerable<PatientMedication>> IPatientRepository.GetAllPatientMedications()
        {
            var medications = await _context.Patient_Medication.ToListAsync();
            return medications;
        }
        async Task<IEnumerable<PatientVaccinationData>> IPatientRepository.GetAllPatientVaccinationData()
        {
            var vaccinationData = await _context.Patient_Vaccination_Data.ToListAsync();
            return vaccinationData;
        }
        async Task<IEnumerable<PatientVisitHistory>> IPatientRepository.GetAllVisitHistory()
        {
            var visitHistory = await _context.Patient_Visit_History.ToListAsync();
            return visitHistory;
        }

        async Task<PatientDetails?> IPatientRepository.GetPatientDetailsBySSN(string ssn)
        {
            var patient = await _context.Patient_Details.Where(patients => patients.ssn == ssn).FirstOrDefaultAsync();
            return patient;
        }

        async Task<PatientDetails?> IPatientRepository.GetPatientDetailsByPatientID(int patientId)
        {
            var patient = await _context.Patient_Details.Where(patients => patients.id == patientId).FirstOrDefaultAsync();
            return patient;
        }

        async Task<int> IPatientRepository.AddNewPatientToDB(PatientDetails patient)
        {
            //Check if patient exists in DB before inserting
            bool patientExists = await _context.Patient_Details.FirstOrDefaultAsync(
                 p =>
                     p.first_name == patient.first_name &&
                     p.middle_name == patient.middle_name &&
                     p.last_name == patient.last_name &&
                     p.ssn == patient.ssn
            ) != null;

            patient.id = 0; //turn off primary key to allow insert

            // if patient does not exist, add to DB
            if (patientExists == false)
            {
                await _context.Patient_Details.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
            return patient.id;
        }

        // return false if error
        async Task<bool> IPatientRepository.StoreLabVisitsInDB(List<PatientLabVisit> patientLabVisits)
        {
            foreach (var labVisit in patientLabVisits)
            {
                //check if patient lab visit exists before adding to DB 
                bool patientLabVisitExists = await _context.Patient_Lab_Visit.FirstOrDefaultAsync(
                  p =>
                      p.patient_id == labVisit.patient_id &&
                      p.lab_name == labVisit.lab_name &&
                      p.lab_test_request == labVisit.lab_test_request
                ) != null;

                //if lab vist does not exist add to DB
                if (patientLabVisitExists == false)
                {
                    labVisit.id = 0; //turn off primary key to allow insert
                    await _context.Patient_Lab_Visit.AddAsync(labVisit);
                    await _context.SaveChangesAsync();
                    if (labVisit.id == 0)
                    {   
                        return false; //error
                    }
                }
            }
            return true; //no error
        }

        //return false if error
        async Task<bool> IPatientRepository.StoreLabResultsInDB(List<PatientLabResult> patientLabResults)
        {
            foreach (var labResult in patientLabResults)
            {
                labResult.id = 0; //turn off primary key to allow insert

                //check if lab exists before adding to DB
                bool patientLabResultExists = await _context.Patient_Lab_Result.FirstOrDefaultAsync(
                    p =>
                        p.lab_visit_id == labResult.lab_visit_id &&
                        p.test_name == labResult.test_name &&
                        p.test_observation == labResult.test_observation
                ) != null;

                //if lab does not exist add to DB
                if (patientLabResultExists == false)
                {
                    await _context.Patient_Lab_Result.AddAsync(labResult);
                    await _context.SaveChangesAsync();
                    if (labResult.id == 0)
                    {
                        return false; //error
                    }
                }
            }
            return true; //no error
        }

        //return false if error
        async Task<bool> IPatientRepository.StoreMedicationsInDB(List<PatientMedication> patientMedications)
        {
            foreach (var medication in patientMedications)
            {
                //check if medication record exists before inserting in DB
                medication.id = 0; //turn off primary key to allow insert

                bool medicationExists = await _context.Patient_Medication.FirstOrDefaultAsync(
                   p =>
                       p.visit_id == medication.visit_id &&
                       p.medicine_name == medication.medicine_name &&
                       p.dosage == medication.dosage &&
                       p.frequency == medication.frequency &&
                       p.prescribed_by == medication.prescribed_by
                ) != null;

                //if medication does not exist, add to DB
                if (medicationExists == false)
                {
                    await _context.Patient_Medication.AddAsync(medication);
                    await _context.SaveChangesAsync();

                    if (medication.id == 0)
                    {
                        return false; //error
                    }
                }

                
            }
            return true; //no error
        }

        //return false if error
        async Task<bool> IPatientRepository.StoreVaccinationsInDB(List<PatientVaccinationData> patientVaccinations)
        {
            foreach (var vaccination in patientVaccinations)
            {
                //check if vaccination record exists before inserting in DB
                vaccination.id = 0; //turn off primary key to allow insert

                bool vaccinationExists = await _context.Patient_Vaccination_Data.FirstOrDefaultAsync(
                   p =>
                       p.patient_id == vaccination.patient_id &&
                       p.vaccine_name == vaccination.vaccine_name &&
                       p.vaccine_date == vaccination.vaccine_date &&
                       p.vaccine_validity == vaccination.vaccine_validity &&
                       p.administered_by == vaccination.administered_by
                ) != null;

                //if vaccination does not exist add to DB
                if (vaccinationExists == false) { 
                    await _context.Patient_Vaccination_Data.AddAsync(vaccination);
                    await _context.SaveChangesAsync();
                }

                if (vaccination.id == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
