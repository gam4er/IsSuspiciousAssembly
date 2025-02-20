using Mono.Cecil;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsSuspiciousAssembly
{
    class IsSuspiciousAssembly
    {
        /// <summary>
        /// Проверяет, что файл по указанному пути является сборкой .NET.
        /// При этом сборка анализируется как файл, без её загрузки для выполнения.
        /// </summary>
        public static bool IsDotNetAssembly(string path)
        {
            try
            {
                // Задаём параметры для чтения сборки.
                // InMemory = false гарантирует, что данные не будут загружены в виде исполняемого объекта,
                // а будут доступны только для анализа метаданных.
                var readerParameters = new ReaderParameters
                {
                    ReadSymbols = false,
                    InMemory = false
                };

                var assembly = AssemblyDefinition.ReadAssembly(path, readerParameters);
                return assembly != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении сборки: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Анализирует сборку на наличие определения класса, который (прямо или косвенно)
        /// наследует System.AppDomainManager.
        /// </summary>
        public static bool ContainsAppDomainManagerSubclass(string path)
        {
            try
            {
                // Читаем сборку как файл (без загрузки для исполнения).
                var readerParameters = new ReaderParameters
                {
                    ReadSymbols = false,
                    InMemory = false                    
                };

                var assembly = AssemblyDefinition.ReadAssembly(path, readerParameters);
                foreach (var type in assembly.MainModule.Types)
                {
                    if (!type.IsClass)
                        continue;

                    if (InheritsFrom(type, "System.AppDomainManager"))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при анализе сборки: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Рекурсивно проверяет, наследует ли тип (прямо или через цепочку наследования)
        /// указанный базовый тип по полному имени.
        /// </summary>
        private static bool InheritsFrom(TypeDefinition type, string baseFullName)
        {
            if (type == null || type.BaseType == null)
                return false;

            if (type.BaseType.FullName == baseFullName)
                return true;

            try
            {
                var baseTypeDef = type.BaseType.Resolve();
                return InheritsFrom(baseTypeDef, baseFullName);
            }
            catch
            {
                // Если не удалось разрешить базовый тип, прекращаем поиск.
                return false;
            }
        }

        static void Main(string[] args)
        {
            string path = "";
            if (args.Length < 1)
            {
                Console.WriteLine("Использование: AssemblyInspector <путь к сборке>");
#if DEBUG
                Console.WriteLine("Будет сборка по пути \"d:\\Desktop\\Детектится\\0xFB3586FA7200C88C47C92D0F076E5EEA\"");
                path = "d:\\Desktop\\Детектится\\0xFB3586FA7200C88C47C92D0F076E5EEA";
#else
                return;                
#endif
            }
            else
                path = args [0];


            if (!IsDotNetAssembly(path))
            {
                Console.WriteLine("Файл не является корректной сборкой .NET.");
                return;
            }

            if (ContainsAppDomainManagerSubclass(path))
            {
                Console.WriteLine("Сборка содержит определение класса, наследующего System.AppDomainManager.");
            }
            else
            {
                Console.WriteLine("Сборка не содержит определения класса, наследующего System.AppDomainManager.");
            }
        }
    }
}
