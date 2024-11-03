using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoWorker
{
    /// <summary>
    /// Делегат для обработки ошибок
    /// </summary>
    /// <param name="message">Сообщение</param>
    public delegate void HandleError(string message);
    public interface InfoWorker
    {
        /// <summary>
        /// Получить информацию
        /// </summary>
        /// <returns>Информация</returns>
        string getinfo();
        /// <summary>
        /// Задать информацию
        /// </summary>
        /// <param name="info">Информация</param>
        void setinfo(string info);
        /// <summary>
        /// Добавить информацию
        /// </summary>
        /// <param name="info">Информация</param>
        void addinfo(string info);
    }
}
