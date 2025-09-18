using ConsoleAppCmsv2025.Repository;
using ConsoleAppCmsv2025.ViewModel;

namespace ConsoleAppCmsv2025.Service
{
    public class PatientServiceImpl : IPatientService
    {
        // Declare private variable for repository
        private readonly IPatientRepository _patientRepository;
        private object _appointmentRepository;

        // DI - constructor injection
        public PatientServiceImpl(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<PatientViewModel> SearchPatientByMMRAsync(string mmrNumber)
        {
            return await _patientRepository.SearchPatientByMMRAsync(mmrNumber);
        }

        public async Task<PatientViewModel> SearchPatientByPhoneAsync(string phone)
        {
            return await _patientRepository.SearchPatientByPhoneAsync(phone);
        }

        public async Task<(int PatientId, string MMRNumber)> AddPatientAsync(PatientInputModel patient)
        {
            return await _patientRepository.AddPatientAsync(patient);
        }

        public async Task<List<MembershipViewModel>> GetMembershipsAsync()
        {
            return await _patientRepository.GetMembershipsAsync();
        }

        public async Task AssignMembershipAsync(int patientId, int membershipId)
        {
            await _patientRepository.AssignMembershipAsync(patientId, membershipId);
        }

        public async Task<List<AppointmentViewModel>> GetDoctorScheduleAsync(int doctorId, DateTime appointmentDate)
        {
            return await _patientRepository.GetDoctorScheduleAsync(doctorId, appointmentDate);
        }

        public async Task<(int AppointmentId, int TokenNo)> BookAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate)
        {
            return await _patientRepository.BookAppointmentAsync(patientId, doctorId, appointmentDate);
        }

    }
}