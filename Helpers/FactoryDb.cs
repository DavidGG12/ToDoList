using System.Data;
using System.Data.SqlClient;
using ToDoList.Helpers.Interfaces;

namespace ToDoList.Helpers
{
    public class FactoryDb : IDatabaseManager
    {
        private readonly string _cnnString;

        public FactoryDb(string cnnString)
        {
            _cnnString = cnnString;
        }

        public DataTable ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters)
        {
            DataTable resultTable = new DataTable();

            using (var con = new SqlConnection(_cnnString))
            {
                using (var cmd = new SqlCommand(storedProcedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                        foreach (var param in parameters)
                        {
                            if (param.Value is string)
                                cmd.Parameters.AddWithValue(param.Key, param.Value.ToString().Replace("'", "") ?? "");
                            else
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }

                    con.Open();

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(resultTable);
                        return resultTable;
                    }
                }
            }
        }

        public class DatabaseManagerFactory
        {
            public static IDatabaseManager CreateDatabaseManager(string cnnString)
            {
                return new FactoryDb(cnnString);
            }
        }
    }
}
