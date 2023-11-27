using PatientApiDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientApiDAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) { }

        public DbSet<PatientDetails> Patient_Details { get; set; }
        public DbSet<PatientLabResult> Patient_Lab_Result { get; set; }
        public DbSet<PatientLabVisit> Patient_Lab_Visit { get; set; }
        public DbSet<PatientMedication> Patient_Medication { get; set; }
        public DbSet<PatientVaccinationData> Patient_Vaccination_Data { get; set; }
        public DbSet<PatientVisitHistory> Patient_Visit_History { get; set; }
    }
}
