using MySql.Data.MySqlClient;

namespace OnlineElectionControl.Classes
{
    public static class Database
    {
        private static MySqlConnection? connection;

        private static void ConnectDatabase()
        {
            if (connection == null)
            {
                connection = new MySqlConnection("server=localhost;"
                                               + "database=onlineelectioncontrol;"
                                               + "userid=root;"
                                               + "password=;");
            }
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            List<Dictionary<string, object>> rows = new();
            MySqlDataReader? result = null;

            try
            {
                ConnectDatabase();
                connection!.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    result = command.ExecuteReader();

                    var columnNames = new List<string>();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        columnNames.Add(result.GetName(i));
                    }

                    while (result.Read())
                    {
                        var row = new Dictionary<string, object>();

                        foreach (var columnName in columnNames)
                        {
                            row[columnName] = result[columnName];
                        }

                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception has occurred: " + ex.Message);
            }
            finally
            {
                result?.Close();
                connection!.Close();
            }

            return rows;
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            int affectedRows = 0;

            try
            {
                ConnectDatabase();
                connection!.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    affectedRows = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception has occurred: " + ex.Message);
            }
            finally
            {
                connection!.Close();
            }

            return affectedRows;
        }
    }
}
