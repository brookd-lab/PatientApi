using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace PatientApiDAL.Models
{
    [Table("Patient_Details")]
    public class PatientDetails
    {
        [Key]
        public int id { get; set; }
        public string? first_name { get; set; }
        public string? middle_name { get; set; }
        public string? last_name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dob { get; set; }

        public string? ssn { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? zip { get; set; }
        public string? state { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
