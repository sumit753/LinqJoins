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

            //sql format 
            var innerjoinSqlResult = from d in Department.GetAllDepartments()
                                 join e in Employee.GetAllEmployees()
                                 on d.ID equals e.DepartmentID 
                                 select new
                                 {
                                     Department = d.Name,
                                     Employee = e.Name
                                 };
            Console.WriteLine("=========================================");

            Console.WriteLine("Inner Joins");

            Console.WriteLine("=========================================");
            foreach(var obj in innerjoinSqlResult)
            {

                Console.WriteLine( obj.Employee + "\t"+ obj.Department);
                
            }


            // left outer join
            Console.WriteLine("=========================================");

            Console.WriteLine("Left Outer Joins");

            Console.WriteLine("=========================================");

            //var leftOuterJoinResult = from e in Employee.GetAllEmployees()
            //                          join d in Department.GetAllDepartments()
            //                          on e.DepartmentID equals d.ID into eGrp
            //                          from d in eGrp.DefaultIfEmpty()
            //                          select new
            //                          {
            //                              DepartmentName = d == null ? "No Department": d.Name,
            //                              Employee = e.Name
            //                          };

            //as extension method

            var leftOuterJoinResult = Employee.GetAllEmployees().GroupJoin(Department.GetAllDepartments()
                                                                , emp => emp.DepartmentID
                                                                , dep => dep.ID
                                                                , (emp, dept) => new { emp, dept })
                                                                .SelectMany(z => z.dept.DefaultIfEmpty(), (a, b) => new {

                                                                    DepartmentName = b == null ? "No Departmet" : b.Name,
                                                                    EmployeeName = a.emp.Name
                                                                });
                                                                    
            foreach (var obj in leftOuterJoinResult)
            {

                Console.WriteLine(obj.EmployeeName + "\t"+  obj.DepartmentName);
               

            }

        }
    }
}
