using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApiDAL.Models
{
    [Table("Patient_Medication")]
    public class PatientMedication
    {
        [Key]
        public int id { get; set; }
        public int patient_id { get; set; }

        public int? visit_id { get; set; }

        public string? medicine_name { get; set; }

        public string? dosage { get; set; }

        public string? frequency { get; set; }

        public string? prescribed_by { get; set; }

        [DataType(DataType.Date)]
        public DateTime? prescription_date { get; set; }

        public string? prescription_period { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
