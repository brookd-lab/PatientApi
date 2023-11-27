using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PatientApiDAL.Models
{
    [Table("Patient_Lab_Visit")]
    public class PatientLabVisit
    {
        [Key]
        public int id { get; set; }
        public int? patient_id { get; set; }
        public string? lab_name { get; set; }
        public string? lab_test_request { get; set; }
        public string? result_date { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
