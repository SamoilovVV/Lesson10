using System;

namespace Lesson10
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        [NonSerialized]
        public int accountNumber;

        public Person()
        {

        }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name}, {Age}";
        }
    }
}
