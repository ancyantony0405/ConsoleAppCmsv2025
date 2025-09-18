using ConsoleAppCmsv2025.Model;
using ConsoleAppCmsv2025.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Repository
{
    public interface IDoctorRepository
    {
        Task<List<AppointmentViewModel>> ViewTodayAppointmentsAsync(int doctorId);
        Task<int> StartConsultationAsync(int appointmentId, string diagnosis);
        Task AddMedicineAsync(int consultationId, int patientId, int medicineId, string dosage, string duration, int quantity);
        Task AddTestAsync(int consultationId, int patientId, int testId);
        Task<BillViewModel> GenerateBillAsync(int consultationId, int patientId, int doctorId);
        Task<int> GetPatientIdByMMRAsync(string mmrNumber);
        Task<int> GetMedicineIdByNameAsync(string medicineName);
        Task<int> GetTestIdByNameAsync(string testName);
    }
}
