using ClassLibraryDatabaseConnection;
using ConsoleAppCmsv2025.Model;
using ConsoleAppCmsv2025.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Repository
{
    public class DoctorRepositoryImpl : IDoctorRepository
    {
        // ConnectionString from App.config
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        #region Get Today's Appointments for Doctor
        public async Task<List<AppointmentViewModel>> ViewTodayAppointmentsAsync(int doctorId)
        {
            List<AppointmentViewModel> appointments = new();
            try
            {
                using SqlConnection conn = new(winConnString);
                using SqlCommand cmd = new("sp_Doctor_ViewAppointments", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    appointments.Add(new AppointmentViewModel
                    {
                        AppointmentId = reader.GetInt32(0),
                        PatientName = reader.GetString(1),
                        MMRNumber = reader.GetString(2),     
                        AppointmentDate = reader.GetDateTime(3),
                        Status = reader.GetString(4),
                        TokenNo = reader.GetInt32(5)
                    });
                }
                return appointments;
            }

            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion

        #region Start Consultation for Appointment
        public async Task<int> StartConsultationAsync(int appointmentId, string diagnosis)
        {
            try
            {
                using SqlConnection conn = new(winConnString);
                using SqlCommand cmd = new("sp_Doctor_StartConsultation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                cmd.Parameters.AddWithValue("@Diagnosis", diagnosis);

                SqlParameter outputId = new("@ConsultationId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return (int)outputId.Value;
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion

        #region Add Medicine to Consultation
        public async Task AddMedicineAsync(int consultationId, int patientId, int medicineId, string dosage, string duration, int quantity)
        {
            try
            {
                using SqlConnection conn = new(winConnString);
                using SqlCommand cmd = new("sp_Doctor_AddMedicine", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConsultationId", consultationId);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                cmd.Parameters.AddWithValue("@Dosage", dosage);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion

        #region Add Test to Consultation
        public async Task AddTestAsync(int consultationId, int patientId, int testId)
        {
            try
            {
                using SqlConnection conn = new(winConnString);
                using SqlCommand cmd = new("sp_Doctor_AddTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConsultationId", consultationId);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@TestId", testId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion

        #region Generate Bill for Consultation
        public async Task<BillViewModel> GenerateBillAsync(int consultationId, int patientId, int doctorId)
        {
            try
            {
                using SqlConnection conn = new(winConnString);
                using SqlCommand cmd = new("sp_Doctor_GenerateBill", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConsultationId", consultationId);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new BillViewModel
                    {
                        BillId = reader.GetInt32(0),
                        GrossAmount = reader.GetDecimal(1),
                        DiscountPercent = reader.GetDecimal(2),
                        NetPayable = reader.GetDecimal(3)
                    };
                }
                return null;
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion

        #region Get patient by MMR, Medicine by Name, Test by Name
        public async Task<int> GetPatientIdByMMRAsync(string mmrNumber)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(winConnString))
                {
                    string query = "SELECT PatientId FROM PatientTbl WHERE MMRNumber = @mmr";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@mmr", mmrNumber);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }

        public async Task<int> GetMedicineIdByNameAsync(string medicineName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(winConnString))
                {
                    string query = "SELECT MedicineId FROM MedicineTbl WHERE MedicineName = @name";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", medicineName);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }

        public async Task<int> GetTestIdByNameAsync(string testName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(winConnString))
                {
                    string query = "SELECT TestId FROM TestTbl WHERE TestName = @name";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", testName);

                    await con.OpenAsync();
                    object result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
