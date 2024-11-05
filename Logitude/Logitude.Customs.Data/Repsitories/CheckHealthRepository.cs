using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.CheckHealthHelper
{
    public class CheckHealthRepository
    {

        public  void CheckHealth()
        {
            try
            {
                string connectionString = TenantServerConfigration.GetDbConnection(0);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert a new row
                    string insertQuery = "INSERT INTO CheckHealth (Status) VALUES ('Healthy')";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                    }

                    // Update the row
                    string updateQuery = "UPDATE CheckHealth SET Status = 'Updated' WHERE Status = 'Healthy'";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Get the row
                    string selectQuery = "SELECT Status FROM CheckHealth WHERE Status = 'Updated'";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader.GetString(0);
                              
                            }
                        }
                    }

                    // Delete the row
                    string deleteQuery = "DELETE FROM CheckHealth WHERE Status = 'Updated'";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }
                }

                //return true;
            }
            catch (Exception e)
            {
                throw new Exception("Health check failed" ,e);
            }
        }
    }
}