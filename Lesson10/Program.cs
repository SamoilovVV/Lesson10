using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO.Compression;

namespace Lesson10
{
    class Program
    {
        static void Main(string[] args)
        {
            //WorkingWithDirectories();
            //WorkingWithDirectoryInfo();
            //WorkingWithFiles();
            //WorkingWithPaths();
            
            //WorkingWithFileStream();
            //WorkingWithStreamReaderAndWriters();

            //UsingAsyncStreams();
            //WorkingWithZipFiles();

            //UsingBinarySerialization();
            //UsingXmlSerialization();
            //UsingJsonSerialization();
            //UsingJsonSerialization2();

            //WorkingWithXml();
            //UsingLinqToXml();
            UsingLinqToXmlQueries();

            Console.ReadLine();
        }

        private static string _dirName = @"D:\Projects\C_sharp_courses";
        public static void WorkingWithDirectories()
        {
            if (Directory.Exists(_dirName))
            {
                Console.WriteLine("Содержимое папки:");

                Console.WriteLine("Дочерние папки");
                var dirs = Directory.GetDirectories(_dirName);
                foreach(var dir in dirs)
                {
                    Console.WriteLine(dir);
                }

                Console.WriteLine("Файлы");
                var files = Directory.GetFiles(_dirName);
                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }

            string subDir = "TestFolder";

            //string fullSubDirName = _dirName + @"\" + subDir;
            string fullSubDirName = Path.Combine(_dirName, subDir);

            Directory.CreateDirectory(fullSubDirName);
            if (Directory.Exists(fullSubDirName))
            {
                Console.WriteLine($"Directory {fullSubDirName} is successfully created");
            }

            //try
            //{
            //    //DirectoryInfo parentDirectory = Directory.GetParent(_dirName);
            //    //string parentDirectoryName = parentDirectory.FullName;
            //    //Directory.Move(fullSubDirName, Path.Combine(parentDirectoryName, subDir));

            //    Directory.Delete(fullSubDirName);
            //    Console.WriteLine($"Папка {fullSubDirName} удалена");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        public static void WorkingWithDirectoryInfo()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_dirName);

            Console.WriteLine($"{dirInfo.Name}");
            Console.WriteLine($"{dirInfo.FullName}");
            Console.WriteLine($"{dirInfo.Parent}");
            Console.WriteLine($"{dirInfo.CreationTime}");
            Console.WriteLine($"{dirInfo.Root}");

            string subDir = "TestFolder";
            string fullSubDirName = Path.Combine(_dirName, subDir);

            try
            {
                var subDirInfo = new DirectoryInfo(fullSubDirName);
                subDirInfo.Delete();
                
                Console.WriteLine($"Папка {fullSubDirName} удалена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void WorkingWithFiles()
        {
            try
            {
                string fileName = "testFile.txt";

                string filePath = Path.Combine(_dirName, fileName);
                using FileStream fs = File.Create(filePath);
                Console.WriteLine($"Файл создан!");
                fs.Close();

                var fileInf = new FileInfo(filePath);
                Console.WriteLine(fileInf.Name);
                Console.WriteLine(fileInf.FullName);
                Console.WriteLine(fileInf.CreationTime);

                string destinationPath = Path.Combine(_dirName, "anotherTestFile.txt");
                var destFileInf = fileInf.CopyTo(destinationPath, true);
                //File.Copy(filePath, destinationPath, true);
                Console.WriteLine($"Файл успешно скопирован в {destinationPath}");

                File.Delete(destinationPath);
                //destFileInf.Delete();

                Console.WriteLine($"Файл {destinationPath} удалён");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public static void WorkingWithPaths()
        {
            string fileName = "testFile.txt";

            string filePath = Path.Combine(_dirName, fileName);

            Console.WriteLine($"Directory Name: {Path.GetDirectoryName(filePath)}");
            Console.WriteLine($"File Name: {Path.GetFileName(filePath)}");
            Console.WriteLine($"File Name w/o extension: {Path.GetFileNameWithoutExtension(filePath)}");
            Console.WriteLine($"File extension: {Path.GetExtension(filePath)}");
            Console.WriteLine($"Root Path: {Path.GetPathRoot(filePath)}");
            Console.WriteLine($"File Name with another extension: {Path.ChangeExtension(filePath, "log")}");
        }

        public static void WorkingWithFileStream()
        {
            string fileName = "testFile.txt";

            string filePath = Path.Combine(_dirName, fileName);

            using (FileStream fs = File.OpenWrite(filePath))
            {
                string text = "А из нашего окна площадь Красная видна";

                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fs.Write(array, 0, array.Length);
                //await fs.WriteAsync(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");

            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open)) // File.OpenRead(filePath)
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                //await fs.ReadAsync(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine(textFromFile);
            }
        }

        public async static void WorkingWithStreamReaderAndWriters()
        {
            string fileName = "testFile.txt";

            string filePath = Path.Combine(_dirName, fileName);

            try
            {
                string text = "Мама мыла раму\n";
                using (StreamWriter sw = new StreamWriter(filePath, append: false))
                {
                    sw.Write(text);
                }

                string textAppend = "Раму мыла мама";
                using (StreamWriter sw = new StreamWriter(filePath, append: true, System.Text.Encoding.Default))
                {
                    await sw.WriteAsync(textAppend);
                }

                using (StreamReader sr = new StreamReader(filePath))
                {
                    //Console.WriteLine(sr.ReadToEnd());

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async static void UsingAsyncStreams()
        {
            string[] names = { "Вася", "Петя", "Маша", "Гена", "Юля", "Вадим", "Сергей", "Денис", "Коля", "Женя", "Света", "Таня", "Оля" };

            IAsyncEnumerable<string> data = GetDataAsync(names);

            await foreach(var name in data)
            {
                Console.WriteLine(name);
            }
        }

        private static async IAsyncEnumerable<string> GetDataAsync(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine($"Получаем {i + 1} элемент");
                await Task.Delay(500);
                yield return data[i];
            }
        }

        public static void WorkingWithZipFiles()
        {
            string folderName = "Lesson08";
            string sourcePath = Path.Combine(_dirName, folderName);
            string zipFileName = "testFile.zip";
            string zipPath = Path.Combine(_dirName, zipFileName);
            string destPath = Path.Combine(_dirName, "TestFolder");

            ZipFile.CreateFromDirectory(sourcePath, zipPath);
            Console.WriteLine($"Папка {folderName} архивирована в {zipPath}");

            ZipFile.ExtractToDirectory(zipPath, destPath);
            Console.WriteLine($"Папка {zipPath} распакована в {destPath}");
        }

        public static void UsingBinarySerialization()
        {
            Person person1 = new Person("Вася", 29);
            Person person2 = new Person("Петя", 25);
            Person person3 = new Person("Лена", 25);
            // массив для сериализации
            Person[] people = new Person[] { person1, person2, person3 };

            BinaryFormatter formatter = new BinaryFormatter();

            string datFileName = Path.Combine(_dirName, "people.dat");
            using (FileStream fs = new FileStream(datFileName, FileMode.OpenOrCreate))
            {
                // сериализуем весь массив people
                formatter.Serialize(fs, people);

                Console.WriteLine("Объект сериализован");
            }

            // десериализация
            using (FileStream fs = new FileStream(datFileName, FileMode.Open))
            {
                Person[] deserilizePeople = formatter.Deserialize(fs) as Person[];

                if (deserilizePeople != null)
                {
                    foreach (Person p in deserilizePeople)
                    {
                        Console.WriteLine($"Имя: {p.Name} --- Возраст: {p.Age}");
                    }
                }
            }
        }

        public static void UsingXmlSerialization()
        {
            Person person1 = new Person("Вася", 29);
            Person person2 = new Person("Петя", 25);
            Person person3 = new Person("Лена", 25);
            // массив для сериализации
            Person[] people = new Person[] { person1, person2, person3 };

            var serializer = new XmlSerializer(typeof(Person[]));

            string xmlFileName = Path.Combine(_dirName, "people.xml");
            //using (FileStream fs = new FileStream(xmlFileName, FileMode.OpenOrCreate))
            //{
            //    // сериализуем весь массив people
            //    serializer.Serialize(fs, people);

            //    Console.WriteLine("Объект сериализован");
            //}

            // десериализация
            using (FileStream fs = new FileStream(xmlFileName, FileMode.Open))
            {
                Person[] deserilizePeople = serializer.Deserialize(fs) as Person[];
                if (deserilizePeople != null)
                {
                    foreach (Person p in deserilizePeople)
                    {
                        Console.WriteLine($"Имя: {p.Name} --- Возраст: {p.Age}");
                    }
                }
            }
        }

        public static void WorkingWithXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Path.Combine(_dirName, "people.xml"));
            XmlElement xRoot = xDoc.DocumentElement;
            foreach(XmlNode node in xRoot)
            {
                foreach (XmlNode childNode in node)
                {
                    if (childNode.Name == "Name")
                    {
                        Console.WriteLine($"Имя: {childNode.InnerText}");
                    }

                    if (childNode.Name == "Age")
                    {
                        Console.WriteLine($"Возраст: {childNode.InnerText}");
                    }
                }
            }
        }

        public static void UsingLinqToXml()
        {
            XDocument xdoc = new XDocument();
            // создаем первый элемент
            XElement iphone6 = new XElement("phone");
            // создаем атрибут
            XAttribute iphoneNameAttr = new XAttribute("name", "iPhone X");
            XElement iphoneCompanyElem = new XElement("company", "Apple");
            XElement iphonePriceElem = new XElement("price", "40000");
            // добавляем атрибут и элементы в первый элемент
            iphone6.Add(iphoneNameAttr);
            iphone6.Add(iphoneCompanyElem);
            iphone6.Add(iphonePriceElem);

            // создаем второй элемент
            XElement galaxys7 = new XElement("phone");
            XAttribute galaxysNameAttr = new XAttribute("name", "Samsung Galaxy S7");
            XElement galaxysCompanyElem = new XElement("company", "Samsung");
            XElement galaxysPriceElem = new XElement("price", "33000");
            galaxys7.Add(galaxysNameAttr);
            galaxys7.Add(galaxysCompanyElem);
            galaxys7.Add(galaxysPriceElem);
            // создаем корневой элемент
            XElement phones = new XElement("phones");
            // добавляем в корневой элемент
            phones.Add(iphone6);
            phones.Add(galaxys7);
            // добавляем корневой элемент в документ
            xdoc.Add(phones);
            //сохраняем документ
            xdoc.Save(Path.Combine(_dirName,"phones.xml"));
        }

        class Phone
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }

        public static void UsingLinqToXmlQueries()
        {
            XDocument xdoc = XDocument.Load(Path.Combine(_dirName,"phones.xml"));

            var items = xdoc.Element("phones").Elements("phone")
                            .Where(xe => xe.Element("company").Value == "Samsung")
                            .Select(xe =>
                                    new Phone
                                    {
                                        Name = xe.Attribute("name").Value,
                                        Price = xe.Element("price").Value
                                    });

            foreach (var item in items)
                Console.WriteLine($"{item.Name} - {item.Price}");
        }

        public static void UsingJsonSerialization()
        {
            Person person1 = new Person("Вася", 29);
            Person person2 = new Person("Петя", 25);
            Person person3 = new Person("Лена", 25);
            // массив для сериализации
            Person[] people = new Person[] { person1, person2, person3 };

            string jsonPeople = JsonSerializer.Serialize(people);
            Console.WriteLine(jsonPeople);

            Person[] restoredPeople = JsonSerializer.Deserialize<Person[]>(jsonPeople);

            foreach(var p in restoredPeople)
            {
                Console.WriteLine(p.Name);
            }
        }

        public async static void UsingJsonSerialization2()
        {
            Person person1 = new Person("Вася", 29);
            Person person2 = new Person("Петя", 25);
            Person person3 = new Person("Лена", 25);
            // массив для сериализации
            Person[] people = new Person[] { person1, person2, person3 };

            string jsonFileName = Path.Combine(_dirName, "people.json");
            using (FileStream fs = new FileStream(jsonFileName, FileMode.OpenOrCreate))
            {
                // сериализуем весь массив people
                await JsonSerializer.SerializeAsync<Person[]>(fs, people);

                Console.WriteLine("Объект сериализован");
            }

            // десериализация
            using (FileStream fs = new FileStream(jsonFileName, FileMode.OpenOrCreate))
            {
                Person[] deserilizePeople = await JsonSerializer.DeserializeAsync<Person[]>(fs);

                foreach (Person p in deserilizePeople)
                {
                    Console.WriteLine($"Имя: {p.Name} --- Возраст: {p.Age}");
                }
            }
        }
    }
}
