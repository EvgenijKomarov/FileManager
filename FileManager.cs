using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoWorker
{
    public class FileManager : InfoWorker
    {
        private string _route;//Путь до файла
        private readonly object _lockConnection = new object();//Защита соединения
        private HandleError errorHandle;//Обработка ошибок

        private static readonly Dictionary<string, FileManager> _instances = new();//Словарь с файловыми менеджерами. Нужен для предотвращения
                                                                                   //создания нескольких файловых менеджеров

        private FileManager(string route, HandleError errorHandler)
        {
            _route = route;
            errorHandle = errorHandler;

            try
            {
                lock (_lockConnection)
                {
                    if (!File.Exists(route))
                    {
                        using (FileStream fs = File.Create(route)) { }
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandle.Invoke($"Ошибка при создании файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Получить или создать файловый менеджер
        /// </summary>
        /// <param name="route">Путь до файла</param>
        /// <param name="errorHandler">Делегат обработки ошибок</param>
        /// <returns>Экземпляр FileManager</returns>
        public static FileManager GetOrCreate(string route, HandleError errorHandler)//паттерн Singletone 
        {
            if (_instances.TryGetValue(route, out var manager))
            {
                return manager;
            }

            manager = new FileManager(route, errorHandler);
            _instances.Add(route, manager);
            return manager;
        }
        /// <summary>
        /// Получить путь до файла
        /// </summary>
        /// <returns>Путь до файла</returns>
        public string GetRoute() => _route;
        /// <summary>
        /// Записать текст в файл
        /// </summary>
        /// <param name="info">Текст</param>
        public void setinfo(string info)
        {
            try
            {
                lock (_lockConnection)
                {
                    File.WriteAllText(_route, info);
                }
            }
            catch (Exception ex)
            {
                errorHandle.Invoke($"Ошибка при записи в файл: {ex.Message}");
            }
        }
        /// <summary>
        /// Получить текст из файла
        /// </summary>
        /// <returns>Текст файла</returns>
        public string getinfo()
        {
            try
            {
                lock (_lockConnection)
                {
                    return File.ReadAllText(_route);
                }
            }
            catch (Exception ex)
            {
                errorHandle.Invoke($"Ошибка при чтении из файла: {ex.Message}");
                return "";
            }
        }
        /// <summary>
        /// Дописать текст в файл
        /// </summary>
        /// <param name="info">Текст</param>
        public void addinfo(string info)
        {
            try
            {
                lock (_lockConnection)
                {
                    using (StreamWriter writer = new StreamWriter(_route, true))
                    {
                        writer.WriteLine(info);
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandle.Invoke($"Ошибка при добавлении информации в файл: {ex.Message}");
            }
        }
    }
}
