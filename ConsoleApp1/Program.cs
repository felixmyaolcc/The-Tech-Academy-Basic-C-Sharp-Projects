using System;
using System.Linq;
using System.Data.Entity;  // Add this namespace for EF 6

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StudentContext())
            {
                // Initialize the database - ensure it's created (if not using migrations)
                context.Database.Initialize(force: false);  // EF 6: Initialize the database (force: false means it won't reinitialize if it already exists)

                // Add a student
                var student = new Student { Name = "James Cameron", Age = 30 };
                context.Students.Add(student);
                context.SaveChanges();

                // Display all students
                var students = context.Students.ToList();
                Console.WriteLine("Students in Database");
                foreach (var s in students)
                {
                    Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Age: {s.Age}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
