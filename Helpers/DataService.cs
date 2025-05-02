using static ToDoList.Helpers.FactoryDb;
using System.Data;

namespace ToDoList.Helpers
{
    public class DataService
    {
        private T MapDataRowToModel<T>(DataRow row) where T : new()
        {
            var model = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name) && !row.IsNull(property.Name))
                    property.SetValue(model, Convert.ChangeType(row[property.Name], property.PropertyType));

            }

            return model;
        }

        private List<T> MapDataList<T>(DataTable table) where T : new()
        {
            var modelList = new List<T>();
            var properties = typeof(T).GetProperties();

            foreach (DataRow row in table.Rows)
            {
                var model = new T();
                foreach (var property in properties)
                {

                    if (row.Table.Columns.Contains(property.Name) && !row.IsNull(property.Name))
                        property.SetValue(model, Convert.ChangeType(row[property.Name], property.PropertyType));
                }

                modelList.Add(model);
            }

            return modelList;
        }

        public T GetData<T>(string conString, string storedName, Dictionary<string, object> parameters) where T : new()
        {
            var dbFactory = DatabaseManagerFactory.CreateDatabaseManager(conString);
            var result = dbFactory.ExecuteStoredProcedure(storedName, parameters);

            if (result.Rows.Count > 0)
                return MapDataRowToModel<T>(result.Rows[0]);

            return default;
        }

        public List<T> GetDataList<T>(string conString, string storedName, Dictionary<string, object> parameters) where T : new()
        {
            var dbFactory = DatabaseManagerFactory.CreateDatabaseManager(conString);
            var result = dbFactory.ExecuteStoredProcedure(storedName, parameters);

            if (result.Rows.Count > 0)
                return MapDataList<T>(result);

            return default;
        }
    }
}
