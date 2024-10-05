using MySql.Data.MySqlClient;

namespace OnlineElectionControl.Classes
{
    public static class Database
    {
        private static MySqlConnection? _connection;

        private static MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection("server=localhost;"
                                                    + "database=onlineelectioncontrol;"
                                                    + "userid=root;"
                                                    + "password=;");
                }
                return _connection;
            }
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string pQuery, Dictionary<string, object>? pParameters = null)
        {
            List<Dictionary<string, object>> rows = new();
            MySqlDataReader? result = null;

            try
            {
                Connection.Open();

                using (var command = new MySqlCommand(pQuery, Connection))
                {
                    if (pParameters != null)
                    {
                        foreach (var param in pParameters)
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
                Connection.Close();
            }

            return rows;
        }

        public static int ExecuteNonQuery(string pQuery, Dictionary<string, object>? pParameters = null)
        {
            int affectedRows = 0;

            try
            {
                Connection.Open();

                using (var command = new MySqlCommand(pQuery, Connection))
                {
                    if (pParameters != null)
                    {
                        foreach (var param in pParameters)
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
                Connection.Close();
            }

            return affectedRows;
        }
    }
}
