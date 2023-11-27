This is the data access layer for the Patient API

There was a schema change due to the API returning string data for the lab_visit_id
in the patient_lab_result table.

alter table patient_lab_result
alter column lab_visit_id nvarchar(10)