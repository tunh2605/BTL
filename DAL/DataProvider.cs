using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class DataProvider
    {
        private static DataProvider instance;
        private string connectionString = "Data Source=DESKTOP-L4JFPFV;Initial Catalog=QLKS;Integrated Security=True";

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set { instance = value; }
        }

        private DataProvider() { }

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    if (parameter != null)
                    {
                        var matches = System.Text.RegularExpressions.Regex.Matches(query, @"@\w+");
                        int i = 0;
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            if (i < parameter.Length)
                            {
                                command.Parameters.AddWithValue(match.Value, parameter[i]);
                                i++;
                            }
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameter != null)
                    {
                        var matches = System.Text.RegularExpressions.Regex.Matches(query, @"@\w+");
                        int i = 0;
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            if (i < parameter.Length)
                            {
                                command.Parameters.AddWithValue(match.Value, parameter[i]);
                                i++;
                            }
                        }
                    }

                    data = command.ExecuteScalar();
                }
            }

            return data;
        }


        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(query, @"@\w+");
                    int i = 0;
                    foreach (System.Text.RegularExpressions.Match match in matches)
                    {
                        if (i < parameter.Length)
                        {
                            command.Parameters.AddWithValue(match.Value, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();
            }
            return data;
        }
    }
}
