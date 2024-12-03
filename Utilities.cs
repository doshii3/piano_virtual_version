using System;
using System.IO;
using System.Windows.Forms;

namespace piano
{
    public static class Utilities
    {

        public static string GetNoteFromKey(Keys key) // метод для обозначения клавиш клавиатуры для клавиш пианино
        {
            return key switch
            {
                Keys.A => "DO", // a для ноты до
                Keys.S => "RE", // s для ноты ре
                Keys.D => "MI", // d для ноты ми
                Keys.F => "FA", // f для ноты фа
                Keys.G => "SOL", // g для ноты соль
                Keys.H => "LA", // h для ноты ля
                Keys.J => "SI", // j для ноты си
                _ => null // остальные не воспринимаются
            };
        }

        public static bool IsFileAccessible(string path) // проверка доступности файла для действий 
        {
            try
            {
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException ioEx)
            {
                Log.LogException(ioEx); // лог исключения
                MessageBox.Show("Произошла ошибка. Проверьте файл логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
