using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApplication1
{
    using System.Linq;

    public class Person
    {
        public string Name;

        public int Age;

        public string Gender;

        // Used to find individuals in the DB
        public string DadName;

        public string MomName;
    }

    // Interface for DI
    public interface IO
    {
        void WriteLine(string arg);

        string ReadLine();
    }

    // Service implementation
    public struct ConsoleIO : IO
    {
        public void WriteLine(string arg)
        {
            Console.WriteLine(arg);
        }
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }

    // TEST FOR CONSOLE
    public sealed class TestIO : IO
    {
        public void WriteLine(string arg)
        {
            Debug.WriteLine(arg);
        }

        public string ReadLine()
        {
            return string.Empty;
        }
    }

    //A LIST TO HOLD USER VALUES
    public static class Db
    {
        public static List<Person> people = new List<Person>();
    }

    public class Solution
    {



        // WHAT THIS IS ACTUALLY DOING:
        public static void Main(string[] args)
        {
            var io = new ConsoleIO();
            var p = new Solution();

            var addUser = "Y";
            while (addUser == "Y")
            {
                Console.WriteLine("Would you like to add a user ( Y or N )?");
                addUser = Console.ReadLine().ToUpper();
                if (addUser == "Y")
                {
                    Db.people.Add(p.ReadPerson(io));
                }
            }

            var goSearch = "Y";
            while (goSearch == "Y")
            {
                Console.WriteLine("Would you like to find a particular registered user (Y or N)?");
                goSearch = Console.ReadLine().ToUpper();
                if (goSearch == "Y")
                {
                    Console.WriteLine("Enter the name of the user you would like to find.");
                    var search = Console.ReadLine().ToUpper();
                    foreach (var user in Db.people)
                    {
                        if (user.Name.Contains(search))
                        {
                            Console.WriteLine("Result:");
                            p.WritePerson(io, user);
                        }
                    }

                }
            }

            var seeUsers = "Y";
            while (seeUsers == "Y")
            {
                Console.WriteLine("Would you like to see registered users (Y or N)?");
                seeUsers = Console.ReadLine().ToUpper();
                if (seeUsers == "Y")
                {
                    for (int i = 0; i < Db.people.Count; i++)
                    {
                        Console.WriteLine("Person " + (1 + i) + ":");
                        p.WritePerson(io, Db.people[i]);
                    }
                }
            }
            Console.Read();
        }


        public Person ReadPerson(ConsoleIO io)
        {
            Person p = new Person();
            io.WriteLine("What is the user's first name ?");
            string fName = Console.ReadLine().ToUpper();

            io.WriteLine("What is the user's last name ?");
            string lName = Console.ReadLine().ToUpper();


            io.WriteLine("What is the user's age (example: 45) ?");
            var age = Console.ReadLine().ToUpper();
            int ageInt;
            bool isNumeric = int.TryParse(age, out ageInt);
            while (!isNumeric | ageInt < 0 | ageInt > 150)
            {
                io.WriteLine("Please enter a valid value");
                io.WriteLine("What is the user's age ?");
                age = Console.ReadLine().ToUpper();
                int.TryParse(age, out ageInt);
            }

            // M or F
            io.WriteLine("What is the user's gender (M or F)?");
            var gender = Console.ReadLine().ToUpper();
            while(gender != "M" & gender != "F")
            {
                io.WriteLine("Please enter a valid value");
                io.WriteLine("What is the user's gender (M or F)?");
                gender = Console.ReadLine().ToUpper();
            }

            io.WriteLine("What is the user's dad's first name ?");
            var dadFName = Console.ReadLine().ToUpper();

            io.WriteLine("What is the user's dad's last name ?");
            var dadLName = Console.ReadLine().ToUpper();

            io.WriteLine("What is the user's mom's first name ?");
            var momFName = Console.ReadLine().ToUpper();

            io.WriteLine("What is the user's mom's last name ?");
            var momLName = Console.ReadLine().ToUpper();


            p = new Person();
            p.Name = fName + " " + lName;
            p.Age = ageInt;
            p.Gender = gender;
            p.DadName = dadFName + " " + dadLName;
            p.MomName = momFName + " " + momLName;
            return p;
        }

        public void WritePerson(ConsoleIO io, Person p)
        {
            var names = p.Name.Split(' ');
            Console.WriteLine("first name: {0}", names[0]);
            Console.WriteLine("last name : {0}", names[1]);
            Console.WriteLine("Age       : {0}", p.Age);
            Console.WriteLine("Gender    : {0}", p.Gender == "M" ? "Man" : "Woman");

            var dad = Db.people.Where(o => o.Name == p.DadName);
            if (dad.Count() >= 1)
            {
                if (dad.First().Age > p.Age)
                {
                    Console.WriteLine("Dad's Age : {0}", dad.First().Age);
                }
            }

            var mom = from x in Db.people where x.Name == p.MomName select x;

            if (mom.Count() >= 1)
            {
                if (mom.First().Age > p.Age)
                {
                    Console.WriteLine("Mom's Age : {0}", mom.First().Age);
                }
            }
        }
    }
}