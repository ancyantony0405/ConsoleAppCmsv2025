using ConsoleAppCmsv2025.Repository;
using ConsoleAppCmsv2025.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Service
{
    public class DoctorServiceImpl : IDoctorService
    {
        // Declare private variable for repository
        private readonly IDoctorRepository _doctorRepo;

        // DI - constructor injection
        public DoctorServiceImpl(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        public async Task<List<AppointmentViewModel>> GetTodayAppointmentsAsync(int doctorId)
        {
            return await _doctorRepo.ViewTodayAppointmentsAsync(doctorId);
        }

        public async Task<int> StartConsultationAsync(int appointmentId, string diagnosis)
        {
            return await _doctorRepo.StartConsultationAsync(appointmentId, diagnosis);
        }
        public async Task AddMedicineAsync(int consultationId, int patientId, int medicineId, string dosage, string duration, int quantity)
        {
            await _doctorRepo.AddMedicineAsync(consultationId, patientId, medicineId, dosage, duration, quantity);

        }

        public async Task AddTestAsync(int consultationId, int patientId, int testId)
        {
            await _doctorRepo.AddTestAsync(consultationId, patientId, testId);
        }

        public async Task<BillViewModel> GenerateBillAsync(int consultationId, int patientId, int doctorId)
        {
            return await _doctorRepo.GenerateBillAsync(consultationId, patientId, doctorId);
        }

        public async Task<int> GetPatientIdByMMRAsync(string mmrNumber)
        {
            return await _doctorRepo.GetPatientIdByMMRAsync(mmrNumber);
        }

        public async Task<int> GetMedicineIdByNameAsync(string medicineName)
        {
            return await _doctorRepo.GetMedicineIdByNameAsync(medicineName);
        }

        public async Task<int> GetTestIdByNameAsync(string testName)
        {
            return await _doctorRepo.GetTestIdByNameAsync(testName);
        }

    }
}
