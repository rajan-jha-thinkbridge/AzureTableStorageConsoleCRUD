using Microsoft.Azure.Cosmos.Table;
using TableStorageCrudConsoleApp.Model;

namespace TableStorageCrudConsoleApp.TableStorage
{
    public class TableStorageManager
    {
        private readonly CloudTable _employeeTable;
        private readonly CloudTable _departmentTable;

        public TableStorageManager(CloudTable employeeTable, CloudTable departmentTable)
        {
            _employeeTable = employeeTable;
            _departmentTable = departmentTable;
        }

        public async Task InsertEmployee(Employee employee)
        {
            var insertOperation = TableOperation.Insert(employee);
            await _employeeTable.ExecuteAsync(insertOperation);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var updateOperation = TableOperation.Replace(employee);
            await _employeeTable.ExecuteAsync(updateOperation);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            var deleteOperation = TableOperation.Delete(employee);
            await _employeeTable.ExecuteAsync(deleteOperation);
            Console.WriteLine("Employee deleted successfully.");
        }

        public async Task InsertDepartment(Department department)
        {
            var insertOperation = TableOperation.Insert(department);
            await _departmentTable.ExecuteAsync(insertOperation);
        }

        public async Task<Employee> RetrieveEmployee(string departmentId, string employeeId)
        {
            var retrieveOperation = TableOperation.Retrieve<Employee>(departmentId, employeeId);
            var retrievedResult = await _employeeTable.ExecuteAsync(retrieveOperation);
            return (Employee)retrievedResult.Result;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            TableQuery<Department> query = new TableQuery<Department>();
            var departments = new List<Department>();

            TableContinuationToken continuationToken = null;
            do
            {
                var queryResult = await _departmentTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                departments.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;
            } while (continuationToken != null);

            return departments;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            TableQuery<Employee> query = new TableQuery<Employee>();
            var employees = new List<Employee>();

            TableContinuationToken continuationToken = null;
            do
            {

            } while (true);
            {
                var queryResult = await _employeeTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                employees.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;
            } while (continuationToken != null);

            return employees;
        }
    }
}
