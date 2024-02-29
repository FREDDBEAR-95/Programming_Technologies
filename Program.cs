using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using static LAB_1.Program;

namespace LAB_1
{
    internal class Program
    {
        /* ----- Классы для описание объектов таблиц ----- */

        /// <summary>
        /// Завод
        /// </summary>
        public class Factory
        {
            public uint Id { get; set; }                     // Идентификатор завода (Primary key)
            public string Name { get; set; }        // Название завода
            public string Description { get; set; } // Описание завода

            // Конструктор с параметрами
            public Factory(uint id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print()
            {
                Console.WriteLine($"└ Завод #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  └ Описание: {Description}\n");
            }
        }

        /// <summary>
        /// Установка
        /// </summary>
        public class Unit
        {
            public uint Id { get; set; }                     // Идентификатор установки (Primary key)
            public string Name { get; set; }        // Название установки
            public string Description { get; set; } // Описание установки
            public uint FactotyId { get; set; }              // Идентификатор завода (Foreign key)

            // Конструктор с параметрами
            public Unit(uint id, string name, string description, uint factotyId)
            {
                Id = id;
                Name = name;
                Description = description;
                FactotyId = factotyId;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print()
            {
                Console.WriteLine($"└ Установка #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  ├ Описание: {Description}");
                Console.WriteLine($"  └ Завод #{FactotyId}\n");
            }
        }

        /// <summary>
        /// Резервуар
        /// </summary>
        public class Tank
        {
            public uint Id { get; set; }            // Идентификатор резервуара (Primary key)
            public string Name { get; set; }        // Название резервуара
            public string Description { get; set; } // Описание резервуара
            public double Volume { get; set; }      // Объём резервуара
            public double MaxVolume { get; set; }   // Максимальный объём резервуара
            public uint UnitId { get; set; }        // Идентификатор установки (Foreign key)

            // Конструктор с параметрами
            public Tank(uint id, string name, string description, uint volume, uint maxVolume, uint unitId)
            {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxVolume;
                UnitId = unitId;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print()
            {
                Console.WriteLine($"└ Резервуар #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  ├ Описание: {Description}");
                Console.WriteLine($"  ├ Объём: {Volume}");
                Console.WriteLine($"  ├ Максимальный объём: {MaxVolume}");
                Console.WriteLine($"  └ Установка #{UnitId}\n");
            }
        }

        /* ----------------------------------------------- */


        /* ----- Функции для загрузки объектов таблиц из файла ----- */

        /// <summary>
        /// Функция для получения списка заводов из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список заводов</returns>
        public static List<Factory> GetFactories(string path, string delimiter = ";")
        {
            var factories = new List<Factory>(); // Список заводов
            uint id;                             // Переменная для проверки id записи
            int lineCount = 0;                   // Номер считываемой строки

            // Проверка существования файла
            if (File.Exists(path))
            {
                // Получение всех строк из файла
                var Lines = File.ReadAllLines(path);

                // Проход по строкам
                foreach (var Line in Lines)
                {
                    lineCount++;
                    // Проверка того, что строка не пуста и не равна null
                    if (!string.IsNullOrEmpty(Line))
                    {
                        // Разделение строки
                        var Data = Line.Split(delimiter);

                        // Проверка кол-ва элементов
                        if (Data.Length == 3)
                        {
                            // Преобразование части данных из string в unit
                            if (!uint.TryParse(Data[0], out id)) id = 0;

                            // Проверка преобразование части данных
                            if (id != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                // Создание объекта класса Tank и добавление в список
                                Factory Object = new Factory(id, Data[1], Data[2]);
                                factories.Add(Object);
                            }
                            else
                            {
                                Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                            }

                        }
                        else
                        {
                            Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return factories;
        }

        /// <summary>
        /// Функция для получения списка резервуаров из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список установок</returns>
        public static List<Unit> GetUnits(string path, string delimiter = ";")
        {
            var units = new List<Unit>(); // Список установок
            uint id, factoryId;           // Переменные для проверки части данных
            int lineCount = 0;            // Номер считываемой строки

            // Проверка существования файла
            if (File.Exists(path))
            {
                // Получение всех строк из файла
                var Lines = File.ReadAllLines(path);

                // Проход по строкам
                foreach (var Line in Lines)
                {
                    lineCount++;
                    // Проверка того, что строка не пуста и не равна null
                    if (!string.IsNullOrEmpty(Line))
                    {
                        // Разделение строки
                        var Data = Line.Split(delimiter);

                        // Проверка кол-ва элементов
                        if (Data.Length == 4)
                        {
                            // Преобразование части данных из string в unit
                            if (!uint.TryParse(Data[0], out id)) id = 0;
                            if (!uint.TryParse(Data[3], out factoryId)) factoryId = 0;

                            // Проверка данных
                            if (id != 0 && factoryId != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                // Создание объекта класса Tank и добавление в список
                                Unit Object = new Unit(id, Data[1], Data[2], factoryId);
                                units.Add(Object);
                            }
                            else
                            {
                                Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                            }

                        }
                        else
                        {
                            Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return units;
        }

        /// <summary>
        /// Функция для получения списка резервуаров из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список объектов класса Tank</returns>
        public static List<Tank> GetTanks(string path, string delimiter = ";")
        {
            var tanks = new List<Tank>(); // Список резервуаров
            uint id, volume, maxVolume, unitId; // Переменные для проверки части данных
            int lineCount = 0; // Номер считываемой строки

            // Проверка существования файла
            if (File.Exists(path))
            {
                // Получение всех строк из файла
                var Lines = File.ReadAllLines(path);

                // Проход по строкам
                foreach (var Line in Lines)
                {
                    lineCount++;
                    // Проверка того, что строка не пуста и не равна null
                    if (!string.IsNullOrEmpty(Line))
                    {
                        // Разделение строки
                        var Data = Line.Split(delimiter);

                        // Проверка кол-ва элементов
                        if (Data.Length == 6)
                        {
                            // Преобразование части данных из string в unit
                            if (!uint.TryParse(Data[0], out id)) id = 0;
                            if (!uint.TryParse(Data[3], out volume)) volume = 0;
                            if (!uint.TryParse(Data[4], out maxVolume)) maxVolume = 0;
                            if (!uint.TryParse(Data[5], out unitId)) unitId = 0;

                            // Проверка преобразование части данных
                            if (id != 0 && unitId != 0 && !string.IsNullOrEmpty(Data[1]) && !string.IsNullOrEmpty(Data[2]))
                            {
                                // Проверка объёма резервуара
                                if (volume >= 0 || maxVolume >= 0)
                                {
                                    if (volume <= maxVolume)
                                    {
                                        // Создание объекта класса Tank и добавление в список
                                        Tank Object = new Tank(id, Data[1], Data[2], volume, maxVolume, unitId);
                                        tanks.Add(Object);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"[!] Объём резервуара не может быть больше максимального объёма! Строка #{lineCount}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"[!] Объём резервуара не может быть отрицательным! Строка #{lineCount}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                            }

                        }
                        else
                        {
                            Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
            }

            return tanks;
        }

        /* --------------------------------------------------------- */


        /* ----- Функции для поиска объекта из списка по имени ----- */

        /// <summary>
        /// Функция для поиска завода по имени
        /// </summary>
        /// <param name="factories">Список заводов</param>
        /// <param name="name">Имя завода для поиска</param>
        /// <returns>Объект завода или null</returns>
        public static Factory? SearchFactoryByName(List<Factory> factories, string name)
        {
            // Проходимся по списку заводов
            foreach (var item in factories)
            {
                // Если завод по имени найден, возвращаем объект завода
                if (item.Name == name) 
                    return item;
            }

            // Если завод по имени не найден, то вернуть null
            return null;
        }

        /// <summary>
        /// Функция для поиска установки по имени
        /// </summary>
        /// <param name="units">Список установок</param>
        /// <param name="name">Имя установки для поиска</param>
        /// <returns>Объект установки или null</returns>
        public static Unit? SearchUnitByName(List<Unit> units, string name)
        {
            // Проходимся по списку установок
            foreach (var item in units)
            {
                // Если установка по имени найден, возвращаем объект установки
                if (item.Name == name)
                    return item;
            }

            // Если установка по имени не найден, то вернуть null
            return null;
        }

        /// <summary>
        /// Функция для поиска резервуара по имени
        /// </summary>
        /// <param name="tanks">Список резервуаров</param>
        /// <param name="name">Имя резервуара для поиска</param>
        /// <returns>Объект резервуара или null</returns>
        public static Tank? SearchTankByName(List<Tank> tanks, string name)
        {
            // Проходимся по списку резервуаров
            foreach (var item in tanks)
            {
                // Если резервуар по имени найден, возвращаем объект резервуара
                if (item.Name == name)
                    return item;
            }

            // Если резервуар по имени не найден, то вернуть null
            return null;
        }

        /* --------------------------------------------------------- */


        /// <summary>
        /// Функция для поиска установки, которой принадлежит резервуар
        /// </summary>
        /// <param name="units">Список установок</param>
        /// <param name="tanks">Список резервуаров</param>
        /// <param name="tankName">Название резервуара для поиска</param>
        /// <returns>Объект установки или null</returns>
        public static Unit? FindUnit(List<Unit> units, List<Tank> tanks, string tankName)
        {
            // Проходимся по списку резервуаров
            foreach (var tank in tanks)
            {
                // Если резервуар с нужным именем найден, ищем связанную с ним установку
                if (tank.Name == tankName)
                {
                    // Проходимся по списку установок
                    foreach (var unit in units)
                    {
                        // Возвращаем объект найденной установки
                        if (tank.UnitId == unit.Id)
                            return unit;
                    }
                }
            }

            // Вернуть null, если резервуар или установка не найдены
            return null;
        }

        /// <summary>
        /// Функция для поиска заводу, которому принадлежит установка
        /// </summary>
        /// <param name="factories">Список заводов</param>
        /// <param name="unit">Объект установок</param>
        /// <returns>Объект завода или null</returns>
        public static Factory? FindFactory(List<Factory> factories, Unit unit)
        {
            // Проходимся по списку заводов
            foreach (var factory in factories)
            {
                // Если завод, в котором находится установка найдена, вернуть объект завода
                if (unit.Id == factory.Id)
                    return factory;
            }

            // Вернуть null, если завод не найден
            return null;
        }

        /// <summary>
        /// Функция для вычисления суммарного объёма все резервуаров
        /// </summary>
        /// <param name="tanks">Список резервуаров</param>
        /// <returns>Суммарный объём всех резервуаров</returns>
        public static double GetTotalVolume(List<Tank> tanks)
        {
            return tanks.Sum(item => item.Volume);
        }

        /// <summary>
        /// Функция для вывода 
        /// </summary>
        /// <param name="factories"></param>
        /// <param name="units"></param>
        /// <param name="tanks"></param>
        public static void PrintAllTanks(List<Factory> factories, List<Unit> units, List<Tank> tanks)
        {
            foreach (var tank in tanks)
            {
                foreach(var item in units)
                {
                    if (tank.UnitId == item.Id)
                    {
                        foreach (var factory in factories)
                        {
                            if (item.FactotyId == factory.Id)
                            {
                                Console.WriteLine($"└ Резервуар #{tank.Id}");
                                Console.WriteLine($"  ├ Название: {tank.Name}");
                                Console.WriteLine($"  ├ Описание: {tank.Description}");
                                Console.WriteLine($"  ├ Объём: {tank.Volume}");
                                Console.WriteLine($"  ├ Максимальный объём: {tank.MaxVolume}");
                                Console.WriteLine($"  ├ Название завода: {factory.Description}");
                                Console.WriteLine($"  └ Имя цеха: {factory.Name}\n");
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            bool flag = true, flag_1, flag_2;
            int choice;

            // Список заводов
            var factories = GetFactories(@"Путь к файлу");
            
            // Список установок
            var units = GetUnits(@"Путь к файлу");

            // Список резервуаров
            var tanks = GetTanks(@"Путь к файлу");

            while (flag)
            {
                Console.WriteLine("[*] Меню программы:");
                Console.WriteLine("\t[1] Вывод все объекты таблицы");
                Console.WriteLine("\t[2] Вывод всех резервуаров с именами цеха и завода, где они числятся");
                Console.WriteLine("\t[3] Вывод общей сумму загрузки всех резервуаров");
                Console.WriteLine("\t[4] Найти объект в таблице по имени");
                Console.WriteLine("\t[0] Выход");

                while (true)
                {
                    Console.Write("\n\n[*] Ваш выбор > ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 0:
                                flag = false; 
                                break;

                            case 1:
                                flag_1 = true;
                                Console.WriteLine("\n[*] Выберите таблицу:");
                                Console.WriteLine("\t[1] Заводы");
                                Console.WriteLine("\t[2] Установки");
                                Console.WriteLine("\t[3] Резервуары");
                                Console.WriteLine("\t[0] Выход");

                                while (flag_1)
                                {
                                    Console.Write("\n\n[*] Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_1 = false;
                                                break;

                                            case 1:
                                                if (factories.Count != 0)
                                                {
                                                    factories.ForEach(factory => factory.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }
                                                    

                                                else
                                                    Console.WriteLine("\nСписок заводов пуст!");
                                                
                                                break;

                                            case 2:
                                                if (units.Count != 0)
                                                {
                                                    units.ForEach(item => item.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }
                                                    

                                                else
                                                    Console.WriteLine("\nСписок установок пуст!");
                                                
                                                break; 

                                            case 3:
                                                if (tanks.Count != 0)
                                                {
                                                    tanks.ForEach(tank => tank.Print());
                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_1 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок резервуаров пуст!");
                                                
                                                break;

                                            default:
                                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                    }


                                }

                                break;

                            case 2:
                                if (factories.Count != 0)
                                {
                                    if (units.Count != 0)
                                    {
                                        if (tanks.Count != 0)
                                        {
                                            Console.WriteLine("\n");
                                            PrintAllTanks(factories, units, tanks);
                                        }

                                        else
                                            Console.WriteLine("\nСписок резервуаров пуст!");
                                    }

                                    else
                                        Console.WriteLine("\nСписок установок пуст!");
                                }

                                else
                                    Console.WriteLine("\nСписок заводов пуст!");
                                
                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break;

                            case 3:
                                if (tanks.Count != 0)
                                    Console.WriteLine($"\n\nОбщая сумма загрузки всех резервуаров: {GetTotalVolume(tanks)}");

                                else
                                    Console.WriteLine("Список резервуаров пуст!");

                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break; 
                            
                            case 4:
                                flag_2 = true;
                                while (flag_2)
                                {
                                    Console.WriteLine("\n[*] Выберите таблицу:");
                                    Console.WriteLine("\t[1] Заводы");
                                    Console.WriteLine("\t[2] Установки");
                                    Console.WriteLine("\t[3] Резервуары");
                                    Console.WriteLine("\t[0] Выход");

                                    Console.Write("\n\n[*] Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 0:
                                                flag_2 = false;
                                                break;

                                            case 1:
                                                if (factories.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя завода для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchFactoryByName(factories, name);
                                                        if (result != null)
                                                        {
                                                            result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок заводов пуст!");

                                                break;

                                            case 2:
                                                if (units.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя установки для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchUnitByName(units, name);
                                                        if (result != null)
                                                        {
                                                            result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }

                                                else
                                                    Console.WriteLine("\nСписок установок пуст!");

                                                break;

                                            case 3:
                                                if (tanks.Count != 0)
                                                {
                                                    Console.Write("\n[*] Введите имя резервуара для поиска >");

                                                    string? name = Console.ReadLine();
                                                    if (name != null)
                                                    {
                                                        var result = SearchTankByName(tanks, name);
                                                        if (result != null)
                                                        {
                                                           result.Print();
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }
                                                    }

                                                    else
                                                    {
                                                        Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                    }

                                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                    flag_2 = false;
                                                }


                                                else
                                                    Console.WriteLine("\nСписок резервуаров пуст!");

                                                break;

                                            default:
                                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                    }
                                }

                                break;

                            default:
                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку\n");
                                break;
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                    }
                }
            }
        }
    }
}
