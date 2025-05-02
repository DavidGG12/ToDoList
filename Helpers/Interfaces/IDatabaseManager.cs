using System.Data;

namespace ToDoList.Helpers.Interfaces
{
    public interface IDatabaseManager
    {
        DataTable ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters);
    }
}
