using System.Text.RegularExpressions;

namespace task_2
{
    internal class Program
    {
        /// <summary>
        /// 2. Написать приложение для поиска по всему диску файлов и 
        /// каталогов, удовлетворяющих заданной маске. Необходимо вывести 
        /// найденную информацию на экран в компактном виде (с нумерацией 
        /// объектов) и запросить у пользователя о дальнейших действиях. 
        /// Варианты действий: удалить все найденное, удалить указанный файл 
        /// (каталог), удалить диапазон файлов (каталогов).
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

            // Поиска по всему диску файлов или каталогов, удовлетворяющих заданной маске.

            string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);

            // Вывод найденной информации на экран с нумерацией объектов.
            if (files.Length == 0)
            {
                Console.WriteLine("Файлы не найдены.");
            }
            else
            {
                Console.WriteLine("Найденные файлы:");
                for (var index = 0; index < files.Length; index++)
                {
                    // Вывод в консоль.
                    Console.WriteLine($"{index}\t{files[index]}");
                }
            }

            if (files.Length == 0)
            {
                Console.WriteLine("Поиск не дал результатов, нечего удалять.");
            }
            else
            {
                // Запрос у пользователя о дальнейших действиях с найденными файлами.
                Console.WriteLine("\nВведите номер файла для удаления (-1 - удалить все найденные файлы): ");
                string input = Console.ReadLine();

                // Проверка ввода.
                if (!Regex.IsMatch(input, @"^-?\d+$"))
                {
                    Console.WriteLine("Неверный формат ввода.");
                    Console.ReadKey();
                    return;
                }

                // Удаление всех найденных файлов.
                if (input == "-1")
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    // Удаление указанного файла.
                    int index = int.Parse(input);
                    if (index >= 0 && index < files.Length)
                    {
                        File.Delete(files[index]);
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер файла.");
                        Console.ReadKey();
                        return;
                    }
                }
            }
        }
    }
}