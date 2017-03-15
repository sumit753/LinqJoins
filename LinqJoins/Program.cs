using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqJoins
{
    class Program
    {
        static void Main(string[] args)
        {

            //group join example
            var employeeByDepartment = Department.GetAllDepartments().GroupJoin(Employee.GetAllEmployees()
                                            , d => d.ID, e => e.DepartmentID, (department, employee) => new
                                            {
                                                Department = department,
                                                Employee = employee.OrderBy(e => e.Name)

                                           });
            // as sql

            var employeeByDept = from d in Department.GetAllDepartments()
                                 join e in Employee.GetAllEmployees()
                                 on d.ID equals e.DepartmentID into egroups
                                 select new
                                 {
                                     Department = d,
                                     Employee = egroups
                                 };


            foreach (var obj in employeeByDept)
            {

                Console.WriteLine(obj.Department.Name);

                foreach (var employee in obj.Employee)
                {
                    Console.WriteLine(" "+employee.Name);
                }
            }

            //Inner joins

            var innerjoinResult = Department.GetAllDepartments().Join(Employee.GetAllEmployees(),d=>d.ID ,e=>e.DepartmentID,
                                  (department, employee)=> new {
                                                                  Deparment = department,
                                                                  Employee = employee
                                                                       
                                                                }).GroupBy(d=>d.Deparment.Name).Select(obj => new { department = obj.Key,Employee = obj});

            
            foreach (var obj in innerjoinResult)
            {

                Console.WriteLine(obj.department);
                foreach (var e in obj.Employee)
                {
                    Console.WriteLine(" "+e.Employee.Name);
                }
               
            }


        }
    }
}
