using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogitudeCardsCustomFields.Services
{
    public class CardsCustomFieldsGeneratedScriptExecuter
    {
        public void Execute()
        {
            string mainConnectionString = ConfigurationManager.ConnectionStrings["Mainstr"]?.ToString();
            if (string.IsNullOrEmpty(mainConnectionString))
            {
                MessageBox.Show("Connection is Empty");
                return;
            }

            using (SqlConnection connection = new SqlConnection(BuildConnectionString(mainConnectionString)))
            {
                connection.Open();
                string sqlString = File.ReadAllText("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql");
                SqlCommand command = GetNewSqlCommand(sqlString, connection);
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    connection.Close();
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private SqlCommand GetNewSqlCommand(string sqlString, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(sqlString, connection)
            {
                CommandTimeout = 1000000000
            };
            return command;
        }

        private string BuildConnectionString(string dbSourceConnection)
        {
            string[] sourceConnectionArray = dbSourceConnection.Split(',');
            string server = sourceConnectionArray[3];
            string password = sourceConnectionArray[2];
            string userName = sourceConnectionArray[1];
            string catalog = sourceConnectionArray[0];

            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
    }
}
