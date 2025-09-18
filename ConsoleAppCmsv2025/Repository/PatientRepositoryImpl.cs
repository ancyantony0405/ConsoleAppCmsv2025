using ClassLibraryDatabaseConnection;
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
    public class PatientRepositoryImpl : IPatientRepository
    {
        // ConnectionString from App.config
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        # region Search Patient By MMR
        public async Task<PatientViewModel> SearchPatientByMMRAsync(string mmrNumber)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_SearchPatientByMMR", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@MMRNumber", mmrNumber);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PatientViewModel
                            {
                                PatientId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Gender = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Address = reader.GetString(5),
                                MMRNumber = reader.GetString(6),
                                BillId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                ConsultationFee = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                                LabFee = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                MedicineFee = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                                TotalAmount = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                                AmountPaid = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                                PaymentStatus = reader.IsDBNull(13) ? "Pending" : reader.GetString(13),
                                MembershipName = reader.IsDBNull(14) ? "No Membership" : reader.GetString(14)
                            };
                        }
                    }
                }

                return null; // If no patient found
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

        #region Search Patient By Phone
        public async Task<PatientViewModel> SearchPatientByPhoneAsync(string phone)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_SearchPatientByPhone", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Phone", phone);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new PatientViewModel
                            {
                                PatientId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Gender = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Address = reader.GetString(5),
                                MMRNumber = reader.GetString(6),
                                BillId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                ConsultationFee = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                                LabFee = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                MedicineFee = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                                TotalAmount = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                                AmountPaid = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                                PaymentStatus = reader.IsDBNull(13) ? "Pending" : reader.GetString(13),  
                                MembershipName = reader.IsDBNull(14) ? "No Membership" : reader.GetString(14) 
                            };
                        }
                    }
                }

                return null;  // If no patient found
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

        #region Add Patient
        public async Task<(int PatientId, string MMRNumber)> AddPatientAsync(PatientInputModel patient)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_AddPatient", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@DOB", patient.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                    cmd.Parameters.AddWithValue("@Address", patient.Address);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (Convert.ToInt32(reader["PatientId"]), reader["MMRNumber"].ToString()!);
                        }
                    }
                }
                return (0, "");
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

        #region Get all memberships
        public async Task<List<MembershipViewModel>> GetMembershipsAsync()
        {
            try
            {
                var memberships = new List<MembershipViewModel>();
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetMemberships", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            memberships.Add(new MembershipViewModel
                            {
                                MembershipId = reader.GetInt32(0),
                                MembershipName = reader.GetString(1),
                                DiscountPercent = reader.GetDecimal(2),
                                DurationMonths = reader.GetInt32(3),
                                Fee = reader.GetDecimal(4)
                            });
                        }
                    }
                }
                return memberships;
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

        #region Assign Membership
        public async Task AssignMembershipAsync(int patientId, int membershipId)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_AssignMembership", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@MembershipId", membershipId);
                    await cmd.ExecuteNonQueryAsync();
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

        #region GetDoctorSchedule
        public async Task<List<AppointmentViewModel>> GetDoctorScheduleAsync(int doctorId, DateTime appointmentDate)
        {
            var appointments = new List<AppointmentViewModel>();

            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetDoctorSchedule", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(new AppointmentViewModel
                            {
                                AppointmentId = reader.GetInt32(0),
                                PatientId = reader.GetInt32(1),
                                PatientName = reader.GetString(2),
                                AppointmentDate = reader.GetDateTime(3),
                                Status = reader.GetString(4)
                            });
                        }
                    }
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

        #region BookAppointment
        public async Task<(int AppointmentId, int TokenNo)> BookAppointmentAsync(int patientId, int doctorId, DateTime appointmentDate)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand cmd = new SqlCommand("sp_BookAppointment", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int appointmentId = Convert.ToInt32(reader["AppointmentId"]);
                            int tokenNo = Convert.ToInt32(reader["TokenNo"]);

                            if (appointmentId == 0)
                            {
                                Console.WriteLine("Appointment failed! Doctor is busy at this time. Please choose a different time slot.");
                                return (0, 0);
                            }
                            else if (appointmentId == -1)
                            {
                                Console.WriteLine("Appointment failed! You already have an overlapping appointment at this time.");
                                return (-1, 0);
                            }
                            else
                            {
                                Console.WriteLine($"Appointment booked successfully. Appointment ID: {appointmentId}, Token No: {tokenNo}");
                                return (appointmentId, tokenNo);
                            }
                        }
                    }
                    return (0, 0);
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
