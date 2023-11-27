using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApiDAL.Models
{
    [Table("Patient_Vaccination_Data")]
    public class PatientVaccinationData
    {
        [Key]
        public int id { get; set; }
        public int? patient_id { get; set; }

        public string? vaccine_name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? vaccine_date { get; set; }

        public string? vaccine_validity { get; set; }

        public string? administered_by { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
