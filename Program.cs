using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace servecoin
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.Main());
            new JsonFileManager();
        }
    }

    public class JsonFileManager
    {
        private readonly string _filePath;

        public JsonFileManager()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");

            if (!File.Exists(_filePath))
            {

                var initialContent = new JObject();
                File.WriteAllText(_filePath, initialContent.ToString(Newtonsoft.Json.Formatting.Indented));
                this.SetNestedValue("piggy.goals", new JArray { });
                this.SetNestedValue("language", "en");
            }
        }

        public void AddToArrayByPath(string arrayPath, JToken newValue)
        {
            var json = ReadJson();
            var tokens = arrayPath.Split('.');
            JToken current = json;

            foreach (var token in tokens)
            {
                if (current[token] != null)
                {
                    current = current[token];
                }
                else
                {
                    Console.WriteLine($"Масив за шляхом {arrayPath} не знайдено.");
                    return;
                }
            }

            if (current is JArray array)
            {
                array.Add(newValue);
                WriteJson(json);
            }
            else
            {
                Console.WriteLine($"Шлях {arrayPath} не є масивом.");
            }
        }


        public JObject ReadJson()
        {
            try
            {
                var jsonContent = File.ReadAllText(_filePath);
                return JObject.Parse(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка читання JSON-файлу: {ex.Message}");
                return new JObject();
            }
        }

        public void WriteJson(JObject content)
        {
            try
            {
                File.WriteAllText(_filePath, content.ToString(Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка запису в JSON-файл: {ex.Message}");
            }
        }

        public JToken GetNestedValue(string path)
        {
            var json = ReadJson();
            var tokens = path.Split('.');
            JToken current = json;

            foreach (var token in tokens)
            {
                if (current[token] != null)
                {
                    current = current[token];
                }
                else
                {
                    return null;
                }
            }

            return current;
        }

        public void SetNestedValue(string path, JToken value)
        {
            var json = ReadJson();
            var tokens = path.Split('.');
            JToken current = json;

            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];

                if (i == tokens.Length - 1) // Останній елемент
                {
                    if (current is JObject obj)
                    {
                        obj[token] = value;
                    }
                    else if (current is JArray array) // Якщо поточний елемент масив
                    {
                        int index = int.Parse(token); // Отримуємо індекс елемента масиву
                        if (array.Count > index)
                        {
                            array[index] = value;
                        }
                        else
                        {
                            // Якщо індекс більше, ніж існуючі елементи, додаємо новий елемент
                            array.Add(value);
                        }
                    }
                }
                else
                {
                    if (current[token] == null)
                    {
                        if (current is JObject obj)
                        {
                            obj[token] = new JObject();
                        }
                        else if (current is JArray array)
                        {
                            array.Add(new JObject()); // Якщо це масив, додаємо об'єкт
                        }
                    }

                    current = current[token];
                }
            }

            WriteJson(json);
        }

        // Додаткові методи для роботи з масивами
        public void AddToArray(string arrayPath, JToken newValue)
        {
            var json = ReadJson();
            var tokens = arrayPath.Split('.');
            JToken current = json;

            // Проходимо до потрібного масиву
            foreach (var token in tokens)
            {
                if (current[token] != null)
                {
                    current = current[token];
                }
                else
                {
                    Console.WriteLine($"Масив за шляхом {arrayPath} не знайдено.");
                    return;
                }
            }

            if (current is JArray array)
            {
                array.Add(newValue); // Додаємо новий елемент до масиву
            }
            else
            {
                Console.WriteLine($"Шлях {arrayPath} не є масивом.");
            }

            WriteJson(json);
        }
    }
}