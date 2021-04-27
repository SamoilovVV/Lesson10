using System;
using System.Linq;

namespace ConsoleAppForEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            // CRUD

            Console.WriteLine("Create:");
            Create();
            ShowDbContents();

            Console.WriteLine("Update:");
            Update();
            ShowDbContents();

            Console.WriteLine("Delete:");
            Delete();
            ShowDbContents();

            Console.Read();
        }

        private static void Create()
        {
            using AppContext db = new AppContext();

            // создаем объекты класса Person
            Person person1 = new Person("Вася", 29);
            Person person2 = new Person("Петя", 25);
            Person person3 = new Person("Лена", 25);

            // добавляем их в бд
            db.Persons.Add(person1);
            db.Persons.Add(person2);
            db.Persons.Add(person3);
            db.SaveChanges();
            Console.WriteLine("Объекты успешно сохранены");
        }

        private static void Update()
        {
            using AppContext db = new AppContext();

            // получаем первый объект
            Person user = db.Persons.Where(p => p.Age < 27).FirstOrDefault();
            if (user != null)
            {
                user.Age = 30;

                db.SaveChanges();
            }
        }

        private static void Delete()
        {
            using AppContext db = new AppContext();

            // получаем первый объект
            Person person = db.Persons.FirstOrDefault();
            if (person != null)
            {
                //удаляем объект
                db.Persons.Remove(person);
                db.SaveChanges();
            }
        }

        private static void ShowDbContents()
        {
            using AppContext db = new AppContext();

            // получаем объекты из бд и выводим на консоль
            var persons = db.Persons.ToList();
            Console.WriteLine("Список объектов в БД:");
            foreach (Person p in persons)
            {
                Console.WriteLine($"{p.Id}.{p.Name} - {p.Age}");
            }
        }
    }
}
