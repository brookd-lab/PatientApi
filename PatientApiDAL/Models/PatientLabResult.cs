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
    [Table("Patient_Lab_Result")]
    public class PatientLabResult
    {
        [Key]
        public int id { get; set; }

        public string? lab_visit_id { get; set; }

        public string? test_name { get; set; }

        public string? test_result { get; set; }

        public string? test_observation { get; set; }

        [NotMapped]
        public string[]? attachments { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
       
    }
}
