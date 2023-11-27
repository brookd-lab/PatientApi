Set the app.config based on your desired key and make sure you are running IIS as the 
same user as the encryption or you will not be able to decrypt your password

This pulls data form external api's according to the below specification:

        //Lab Visits
        //NOTE: The API need to be accessed with SSN as the identifier, here is one example:
        //https://testapi.mindware.us/patient-lab-visits?SSN=111111111

        //Lab Results
        //NOTE: Lab results need to be retrieved based on the visit id
        //e.g. https://testapi.mindware.us/Patient-lab-results/?lab_visit_id=1

        //Medication
        //e.g. https://testapi.mindware.us/patient-medications?SSN=111111111

        //Vaccination
        //e.g. https://testapi.mindware.us/patient-vaccinations?SSN=111111111