using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Finals_JCDomasian.DataAccess
{
    public class DbHelper
    {
        private string connectionString = "Server=localhost;Database=milomarathondb;Uid=root;Pwd=;";

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public DataTable GetAllParticipants()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    string query = @"
                        SELECT 
                            p.ParticipantID,
                            p.FullName,
                            p.BirthDate,
                            p.Phone,
                            p.Address,
                            p.BibNumber,
                            m.MarathonType,
                            m.Price,
                            p.PaymentStatus
                        FROM participants p
                        INNER JOIN marathontypes m ON p.TypeID = m.TypeID
                        ORDER BY p.ParticipantID DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading participants: " + ex.Message);
            }
            return dt;
        }

        public DataTable GetMarathonTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    string query = "SELECT TypeID, MarathonType, Price FROM marathontypes ORDER BY TypeID";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading marathon types: " + ex.Message);
            }
            return dt;
        }

        public DataTable SearchParticipants(string nameFilter, string marathonType)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    string query = @"
                        SELECT 
                            p.ParticipantID,
                            p.FullName,
                            p.BirthDate,
                            p.Phone,
                            p.Address,
                            p.BibNumber,
                            m.MarathonType,
                            m.Price,
                            p.PaymentStatus
                        FROM participants p
                        INNER JOIN marathontypes m ON p.TypeID = m.TypeID
                        WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(nameFilter))
                    {
                        query += " AND p.FullName LIKE @nameFilter";
                    }

                    if (!string.IsNullOrWhiteSpace(marathonType) && marathonType != "All")
                    {
                        query += " AND m.MarathonType = @marathonType";
                    }

                    query += " ORDER BY p.ParticipantID DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(nameFilter))
                        {
                            cmd.Parameters.AddWithValue("@nameFilter", "%" + nameFilter + "%");
                        }
                        if (!string.IsNullOrWhiteSpace(marathonType) && marathonType != "All")
                        {
                            cmd.Parameters.AddWithValue("@marathonType", marathonType);
                        }

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching participants: " + ex.Message);
            }
            return dt;
        }

        public bool BibNumberExists(string bibNumber)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM participants WHERE BibNumber = @bibNumber";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bibNumber", bibNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking bib number: " + ex.Message);
            }
        }

        public bool InsertParticipant(string fullName, DateTime birthDate, string phone,
                                     string address, string bibNumber, int typeID, string paymentStatus)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO participants 
                        (FullName, BirthDate, Phone, Address, BibNumber, TypeID, PaymentStatus)
                        VALUES 
                        (@fullName, @birthDate, @phone, @address, @bibNumber, @typeID, @paymentStatus)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@birthDate", birthDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@bibNumber", bibNumber);
                        cmd.Parameters.AddWithValue("@typeID", typeID);
                        cmd.Parameters.AddWithValue("@paymentStatus", paymentStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    throw new Exception("Bib number already exists. Please use a different bib number.");
                }
                throw new Exception("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting participant: " + ex.Message);
            }
        }

        public int GetTypeIDByName(string marathonType)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT TypeID FROM marathontypes WHERE MarathonType = @marathonType";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@marathonType", marathonType);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting TypeID: " + ex.Message);
            }
        }
    }
}