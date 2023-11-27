using Microsoft.Azure.Cosmos.Table;

namespace TableStorageCrudConsoleApp.Model
{
    public class Employee : TableEntity
    {
        public Employee(string departmentId, string employeeId)
        {
            PartitionKey = departmentId;
            RowKey = employeeId;
        }

        public Employee() { }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
    }
}
