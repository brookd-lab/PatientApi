UI Flow

Point to Controller with CORS
PatientID = IsExistingPatient(SSN)
if 0, non-existing, so RegisterNewPatient(PatientData) 
  PatientData comes from UI, New Patient Form
else 
	PatientData = GetPatientData(PatientID)
	PerformBackgroundProcesses(PatientData)

** Check the log for errors
