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
            Application.Run(new Form1());
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
                this.SetNestedValue("piggy.targets", new JArray { });
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
                    Console.WriteLine($"����� �� ������ {arrayPath} �� ��������.");
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
                Console.WriteLine($"���� {arrayPath} �� � �������.");
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
                Console.WriteLine($"������� ������� JSON-�����: {ex.Message}");
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
                Console.WriteLine($"������� ������ � JSON-����: {ex.Message}");
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

                if (i == tokens.Length - 1) // ������� �������
                {
                    if (current is JObject obj)
                    {
                        obj[token] = value;
                    }
                    else if (current is JArray array) // ���� �������� ������� �����
                    {
                        int index = int.Parse(token); // �������� ������ �������� ������
                        if (array.Count > index)
                        {
                            array[index] = value;
                        }
                        else
                        {
                            // ���� ������ �����, �� ������� ��������, ������ ����� �������
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
                            array.Add(new JObject()); // ���� �� �����, ������ ��'���
                        }
                    }

                    current = current[token];
                }
            }

            WriteJson(json);
        }

        // �������� ������ ��� ������ � ��������
        public void AddToArray(string arrayPath, JToken newValue)
        {
            var json = ReadJson();
            var tokens = arrayPath.Split('.');
            JToken current = json;

            // ��������� �� ��������� ������
            foreach (var token in tokens)
            {
                if (current[token] != null)
                {
                    current = current[token];
                }
                else
                {
                    Console.WriteLine($"����� �� ������ {arrayPath} �� ��������.");
                    return;
                }
            }

            if (current is JArray array)
            {
                array.Add(newValue); // ������ ����� ������� �� ������
            }
            else
            {
                Console.WriteLine($"���� {arrayPath} �� � �������.");
            }

            WriteJson(json);
        }
    }

    public class LanguageManager
    {
        private readonly string _filePath = "languages.ini";
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new();

        public LanguageManager()
        {
            DownloadLanguageFileAsync();
            LoadTranslations();
        }

        private static async Task DownloadLanguageFileAsync()
        {
            string filePath = "languages.ini";
            string url = "https://raw.githubusercontent.com/dotinto/servecoin/main/languages.ini";
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            await File.WriteAllTextAsync(filePath, content);
        }

        private void LoadTranslations()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"���� {_filePath} �� ��������.");
            }

            string currentSection = null;

            foreach (var line in File.ReadAllLines(_filePath))
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                    continue;

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Trim('[', ']');
                    if (!_translations.ContainsKey(currentSection))
                    {
                        _translations[currentSection] = new Dictionary<string, string>();
                    }
                }
                else if (currentSection != null)
                {
                    var keyValue = trimmedLine.Split('=', 2);

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        _translations[currentSection][key] = value;
                    }
                }
            }
        }
        public string GetText(string languageCode, string key)
        {
            if (_translations.TryGetValue(languageCode, out var languageTexts))
            {
                if (languageTexts.TryGetValue(key, out var value))
                {
                    return value;
                }

                return $"[Key '{key}' not found]";
            }

            return $"[Language '{languageCode}' not supported]";
        }

        public IEnumerable<string> GetSupportedLanguages()
        {
            return _translations.Keys;
        }
    }
}