using ConsoleAppCmsv2025.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Repository
{
    public interface IPatientRepository
    {
        Task<PatientViewModel> SearchPatientByMMRAsync(string mmrNumber);
        Task<PatientViewModel> SearchPatientByPhoneAsync(string phone);
        Task<(int PatientId, string MMRNumber)> AddPatientAsync(PatientInputModel patient);
        Task<List<MembershipViewModel>> GetMembershipsAsync();
        Task AssignMembershipAsync(int patientId, int membershipId);
        Task<List<AppointmentViewModel>> GetDoctorScheduleAsync(int doctorId, DateTime appointmentDate);
        Task<(int AppointmentId, int TokenNo)> BookAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate);
    }
}
