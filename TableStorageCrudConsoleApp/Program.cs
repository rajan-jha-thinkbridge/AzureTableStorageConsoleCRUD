using Microsoft.Azure.Cosmos.Table;
using TableStorageCrudConsoleApp.Model;
using TableStorageCrudConsoleApp.TableStorage;

internal class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
        string employeeTableName = "Employee";
        string departmentTableName = "Department";

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

        CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

        CloudTable employeeTable = tableClient.GetTableReference(employeeTableName);
        await employeeTable.CreateIfNotExistsAsync();

        CloudTable departmentTable = tableClient.GetTableReference(departmentTableName);
        await departmentTable.CreateIfNotExistsAsync();

        TableStorageManager manager = new TableStorageManager(employeeTable, departmentTable);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Insert Department");
            Console.WriteLine("2. Insert Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await InsertDepartment(manager);
                    break;
                case "2":
                    await InsertEmployee(manager);
                    break;
                case "3":
                    await UpdateEmployee(manager);
                    break;
                case "4":
                    await DeleteEmployee(manager);
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
    }

    static async Task InsertDepartment(TableStorageManager manager)
    {
        Console.Write("Enter Department ID: ");
        string departmentId = Console.ReadLine();

        Console.Write("Enter Department Name: ");
        string departmentName = Console.ReadLine();

        // Create an instance of EmployeeEntity and set its properties
        Department department = new Department(departmentId);

        department.DepartmentName = departmentName;

        try
        {
            await manager.InsertDepartment(department);
            Console.WriteLine("Department inserted successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting department: {ex.Message}");
        }
    }

    static async Task InsertEmployee(TableStorageManager manager)
    {
        // Fetch and display all departments before adding an employee
        var departments = await manager.GetAllDepartments();

        Console.WriteLine("Available Departments:");
        foreach (var department in departments)
        {
            Console.WriteLine($"Department ID: {department.PartitionKey}, Department Name: {department.DepartmentName}");
        }

        Console.Write("Enter Department ID: ");
        string departmentId = Console.ReadLine();

        // Check if the entered department ID exists
        var selectedDepartment = departments.FirstOrDefault(d => d.PartitionKey == departmentId);
        if (selectedDepartment == null)
        {
            Console.WriteLine("Invalid Department ID. Employee not added.");
            return;
        }

        Console.Write("Enter Employee ID: ");
        string employeeId = Console.ReadLine();

        Console.Write("Enter Employee Name: ");
        string employeeName = Console.ReadLine();

        // Create an instance of EmployeeEntity and set its properties
        Employee employee = new Employee(departmentId, employeeId);
        employee.EmployeeName = employeeName;
        employee.EmployeeId = employeeId;

        try
        {
            await manager.InsertEmployee(employee);
            Console.WriteLine("Employee inserted successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting employee: {ex.Message}");
        }
    }

    static async Task UpdateEmployee(TableStorageManager manager)
    {
        Console.Write("Enter Department ID: ");
        string departmentId = Console.ReadLine();

        Console.Write("Enter Employee ID: ");
        string employeeId = Console.ReadLine();

        try
        {
            // Retrieve the employee record from storage using manager's retrieval method
            Employee employeeToUpdate = await manager.RetrieveEmployee(departmentId, employeeId);

            if (employeeToUpdate != null)
            {
                Console.Write("Enter updated Employee Name: ");
                string updatedName = Console.ReadLine();
                employeeToUpdate.EmployeeName = updatedName;

                // Call the manager to update the employee
                await manager.UpdateEmployee(employeeToUpdate);
                Console.WriteLine("Employee updated successfully!");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating employee: {ex.Message}");
        }
    }

    static async Task DeleteEmployee(TableStorageManager manager)
    {
        Console.Write("Enter Department ID: ");
        string departmentId = Console.ReadLine();

        Console.Write("Enter Employee ID: ");
        string employeeId = Console.ReadLine();

        try
        {
            // Retrieve the employee record from storage using manager's retrieval method
            Employee employeeToDelete = await manager.RetrieveEmployee(departmentId, employeeId);

            if (employeeToDelete != null)
            {
                await manager.DeleteEmployee(employeeToDelete);
                Console.WriteLine("Employee updated successfully!");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating employee: {ex.Message}");
        }
    }
}