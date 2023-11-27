using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientApiDAL.Models
{
    [Table("Patient_Visit_History")]
    public class PatientVisitHistory
    {
        [Key]
        public int id { get; set; }
        public int? patient_id { get; set; }

        public DateTime? visit_date { get; set; }

        public string? doctor_name { get; set; }

        public string? nurse_name_1 { get; set; }

        public string? nurse_name_2 { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
    }
}
