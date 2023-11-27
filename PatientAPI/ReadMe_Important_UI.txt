UI Flow

Point to Controller with CORS

//PatientId = 0 if non-existent or returns PatientID
PatientId = isExistingPatient(ssn)
Patient = GetPatientData(PatientId)

NewPatientId = RegisterNewPatient(Patient)
	Calls: IsExistingPatient(SSN)
	Calls: GetPatientData(PatientId)
	if either are true, returns 0

if (PatientID == 0) {
	AddPatientToDB(Patient)
	PerformBackgroundProcesses(Patient)
}

** Check the log for errors
