using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace task
{
    internal class Program
    {
        /// <summary>
        /// 1. Написать приложение, которое ищет в указанном каталоге файлы, 
        /// удовлетворяющие заданной маске, у которых дата последней 
        /// модификации находится в указанном диапазоне. Поиск производится как 
        /// в указанном каталоге, так и в его подкаталогах. Результаты поиска 
        /// сбрасываются в файл отчета.
        /// </summary>
        static void Main(string[] args)
        {
            // Ввод пути к каталогу.
            Console.Write("Введите путь к каталогу: ");

            string path = Console.ReadLine(); // вкл./выкл.
                                              //string path = "d:t";

            // Проверка пути к каталогу.
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Каталог не найден.");
                Console.ReadKey();
                return;
            }

            // Ввод маски для поиска файлов.
            Console.Write("Введите маску для поиска файлов: ");

            string mask = Console.ReadLine(); // вкл./выкл.
                                              //string mask = "*.cs";

            // Проверка маски для поиска файлов.
            if (!Regex.IsMatch(mask, @"^[\w\.\*\?]+$"))
            {
                Console.WriteLine("Неверный формат маски.");
                Console.ReadKey();
                return;
            }

            // Ввод даты ( DateTime ) начала диапазона для поиска в указанном каталоге файлов.
            Console.Write("Введите дату начала диапазона для поиска файлов в формате (дд.мм.гггг): ");
            DateTime dateStart;
            try
            {
                dateStart = DateTime.Parse(Console.ReadLine()); // вкл./выкл.
                //dateStart = Convert.ToDateTime("04.11.2000");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // Ввод даты ( DateTime ) конца диапазона для поиска в указанном каталоге файлов. Конечная дата должна быть больше начальной.
            Console.Write("Введите дату конца диапазона для поиска файлов в формате (дд.мм.гггг): ");
            DateTime dateEnd;
            try
            {
                dateEnd = DateTime.Parse(Console.ReadLine()); // вкл./выкл.
                //dateEnd = Convert.ToDateTime("04.11.2022");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Проверка даты ( DateTime ) конца диапазона для поиска в указанном каталоге файлов. Конечная дата должна быть больше начальной.
            if (dateEnd < dateStart)
            {
                Console.WriteLine("Дата конца диапазона должна быть больше даты начала диапазона.");
                Console.ReadKey();
                return;
            }

            // Поиск как в указанном каталоге, так и в его подкаталогах файлов с указанной маской,
            // у которых дата последней модификации находится в указанном диапазоне.
            string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories)
                .Where(f => File.GetLastWriteTime(f) >= dateStart && File.GetLastWriteTime(f) <= dateEnd)
                .ToArray();

            string reportFile = Path.Combine(path, "report.txt");
            StreamWriter sw = new StreamWriter(reportFile, false, Encoding.UTF8);

            // Вывод результатов поиска в консоль, файл. Если ничего не найдено вывести сообщение.
            bool firstMatсh = true;
            if (files.Length == 0)
            {
                Console.WriteLine("Файлы не найдены.");
            }
            else
            {
                Console.WriteLine("Найденные файлы:");
                foreach (string file in files)
                {
                    // Вывод в консоль.
                    Console.WriteLine(file);

                    // Запись результатов поиска в файл.
                    if (firstMatсh)
                    {
                        sw.WriteLine($"Поиск файлов в каталоге: {path}");
                        sw.WriteLine($"Маска поиска файлов: {mask}");
                        sw.WriteLine($"Дата начала диапазона для поиска файлов: {dateStart}");
                        sw.WriteLine($"Дата конца диапазона для поиска файлов: {dateEnd}");
                        sw.WriteLine();
                        sw.WriteLine("Найденные файлы:");
                    }
                    sw.WriteLine(file);
                    firstMatсh = false;
                }
            }

            // Поиск текста в файлах каталога.
            Console.Write("\nВведите текст для поиска в файлах: ");
            string text;
            try
            {
                text = Console.ReadLine(); // вкл./выкл.
                //text = "StreamReader";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                text = "";
            }
            // Проверка текста для поиска в файлах.
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Текст для поиска не может быть пустым.");
                Console.ReadKey();
                return;
            }
            // Поиск текста в файлах по заданному адресу каталога.
            firstMatсh = true;
            foreach (string file in files)
            {
                // Открыть файл file для чтения.
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    // Поиск текста в файле.
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(text))
                        {
                            // Вывод в консоль.
                            if (firstMatсh)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Найденные файлы с текстом \"{text}\":");
                            }
                            Console.WriteLine(file);

                            // Запись результатов поиска в файл.
                            if (firstMatсh)
                            {
                                sw.WriteLine();
                                sw.WriteLine($"Найденные файлы с текстом \"{text}\":");
                                firstMatсh = false;
                            }
                            sw.WriteLine(file);
                            break;
                        }
                    }
                }
            }
            sw.Close();
        }
    }
}