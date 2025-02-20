# IsSuspiciousAssembly

IsSuspiciousAssembly — это небольшое консольное приложение на C#, использующее библиотеку [Mono.Cecil](https://www.nuget.org/packages/Mono.Cecil/), которое позволяет:

- Проверить, является ли указанный файл действительной сборкой .NET.
- Проанализировать метаданные сборки без её загрузки в исполняемую память (без риска выполнения потенциально вредоносного кода).
- Определить, содержит ли сборка определение класса, расширяющего `System.AppDomainManager`.

## Особенности

- **Безопасный анализ сборки:** Программа читает метаданные сборки напрямую с диска, не выполняя её код.
- **Проверка типа сборки:** Если файл не является валидной .NET сборкой, анализ прерывается с соответствующим сообщением.
- **Рекурсивный поиск наследования:** Приложение сканирует все типы в сборке и проверяет, наследует ли какой-либо из них `System.AppDomainManager` (прямо или косвенно).

## Требования

- [.NET 6 SDK (или выше)](https://dotnet.microsoft.com/download)
- [Mono.Cecil](https://www.nuget.org/packages/Mono.Cecil/) — библиотека для анализа сборок .NET  
  (Её можно добавить через NuGet: `dotnet add package Mono.Cecil`)

## Сборка и использование

1. Склонируйте репозиторий:

   ```bash
   git clone [https://github.com/yourusername/assembly-inspector.git](https://github.com/gam4er/IsSuspiciousAssembly)
   cd IsSuspiciousAssembly

2. Сборка
   ```bash
   dotnet restore
   dotnet build -c Release

3. Использование
   ```bash
   dotnet run --project IsSuspiciousAssembly.csproj -- "C:\Path\To\YourAssembly.dll"
   # Или собранный exe файл
   .\IsSuspiciousAssembly\bin\Release\IsSuspiciousAssembly.exe "C:\Path\To\YourAssembly.dll"

## Лицензия
Этот проект распространяется под лицензией GNU. См. файл LICENSE для подробностей.    
   

