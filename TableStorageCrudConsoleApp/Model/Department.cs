using Microsoft.Azure.Cosmos.Table;

namespace TableStorageCrudConsoleApp.Model
{
    public class Department : TableEntity
    {
        public Department(string departmentId)
        {
            PartitionKey = departmentId;
            RowKey = departmentId;
        }
        public Department() { }

        public string DepartmentName { get; set; }
    }
}
