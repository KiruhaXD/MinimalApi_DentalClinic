using Dental_clinic_WebApi.Module;

namespace Dental_clinic_WebApi.Data
{
    public class BaseDbContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* try
             {*/
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-HIOKBS7;Initial Catalog=Dental_clinic;Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True");
            /* }
             catch (Exception ex) { 
                 Console.WriteLine(ex.ToString());
             }*/
        }
    }
    public class PatientDb : BaseDbContext
    {
        public DbSet<Patients> Patients => Set<Patients>();
    }

    public class DoctorsDb : BaseDbContext 
    {
        public DbSet<Doctors> Doctors => Set<Doctors>();
    }

    public class SpecializationDoctorDb : BaseDbContext
    {
        public DbSet<Specialization_doctor> SpecializationDoctor => Set<Specialization_doctor>();
    }

    public class RecordOnReceivingDb : BaseDbContext
    {
        public DbSet<Records_on_receiving> Record => Set<Records_on_receiving>();
    }

    public class DestinationDb : BaseDbContext
    {
        public DbSet<Destination> Destination => Set<Destination>();
    }

    public class ServicesDb : BaseDbContext
    {
        public DbSet<Services> Services => Set<Services>();
    }

    public class ListOfDiseasesDb : BaseDbContext
    {
        public DbSet<List_of_diseases> Listdiseases => Set<List_of_diseases>();
    }

    public class PatientCardDb : BaseDbContext
    {
        public DbSet<Patient_card> PatientCard => Set<Patient_card>();
    }

    public class ReceivingTypeDb : BaseDbContext
    {
        public DbSet<Receiving_type> DReceiving => Set<Receiving_type>();
    }
}
