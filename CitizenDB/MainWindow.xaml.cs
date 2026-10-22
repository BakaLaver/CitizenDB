using CitizenDB.Entity;
using CitizenDB.ExelMaker;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CitizenDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Citizen> Citizens;
        List<CitizenExcel> CitizensToExcel;
        List<CityTiles> Buildings;
        ExcelCreater ExcelCreaterDoc;

        public MainWindow()
        {
            InitializeComponent();
            Citizens = new List<Citizen>();
            CitizensToExcel = new List<CitizenExcel>();
            Buildings = new List<CityTiles>();
            ExcelCreaterDoc = new ExcelCreater();

            ExcelPackage.License.SetNonCommercialPersonal("CitizenBD");
        }

        private async Task<JsonDocument> ParseGameFileAsync(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);

            if (extension.Equals(".citb", StringComparison.OrdinalIgnoreCase))
            {
                byte[] compressedData = await File.ReadAllBytesAsync(filePath);
                byte[] decompressedData = DecompressBrotli(compressedData);
                string jsonText = Encoding.UTF8.GetString(decompressedData);
                return JsonDocument.Parse(jsonText);
            }
            else
            {
                string text = await File.ReadAllTextAsync(filePath);
                // Replace invalid NaN with null (similar to the JavaScript version)
                string clean = text.Replace("NaN", "null");
                return JsonDocument.Parse(clean);
            }
        }

        public List<JsonElement> FindElementArray(JsonElement element, string elementName)
        {
            var result = new List<JsonElement>();

            if (element.ValueKind == JsonValueKind.Object)
            {
                // Проверяем свойства текущего объекта
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase) &&
                        property.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement item in property.Value.EnumerateArray())
                        {
                            result.Add(item);
                        }
                    }
                    else
                    {
                        // Рекурсивно ищем во вложенных объектах
                        result.AddRange(FindElementArray(property.Value, elementName));
                    }
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in element.EnumerateArray())
                {
                    result.AddRange(FindElementArray(item, elementName));
                }
            }

            return result;
        }

        private byte[] DecompressBrotli(byte[] compressedData)
        {
            using var inputStream = new MemoryStream(compressedData);
            using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();

            brotliStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }

        private async void CityFileCather_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Показываем индикатор загрузки
                await ShowLoadingIndicatorAsync("Загрузка файла...");

                try
                {
                    // Запускаем анимацию кружка
                    StartSpinnerAnimation();

                    // Парсим файл
                    UpdateLoadingText("Парсинг файла...");
                    var city = await ParseGameFileAsync(files[0]);
                    JsonElement root = city.RootElement;

                    // Находим citizens и buildings
                    UpdateLoadingText("Поиск данных...");
                    var citizens = FindElementArray(root, "citizens");
                    var buildings = FindElementArray(root, "cityTiles");

                    // Очищаем предыдущие данные
                    Buildings.Clear();
                    Citizens.Clear();
                    CitizensToExcel.Clear();

                    // Десериализация buildings
                    UpdateLoadingText($"Обработка зданий ({buildings.Count})...");
                    foreach (JsonElement item in buildings)
                    {
                        Buildings.Add(JsonConvert.DeserializeObject<CityTiles>(item.ToString()));
                    }

                    // Десериализация citizens
                    UpdateLoadingText($"Обработка граждан ({citizens.Count})...");
                    foreach (JsonElement item in citizens)
                    {
                        Citizens.Add(JsonConvert.DeserializeObject<Citizen>(item.ToString()));
                    }

                    // Маппинг в Excel
                    UpdateLoadingText("Подготовка данных для Excel...");
                    int processed = 0;
                    foreach (Citizen item in Citizens)
                    {
                        CitizensToExcel.Add(CitizenExcel.MappingToExcel(item, Buildings));
                        processed++;
                        if (processed % 100 == 0) // Обновляем текст каждые 100 граждан
                        {
                            UpdateLoadingText($"Подготовка данных для Excel ({processed}/{Citizens.Count})...");
                        }
                    }

                    // Создание Excel файла
                    UpdateLoadingText("Создание Excel файла...");
                    await Task.Run(() => ExcelCreaterDoc.CreateExcelFile(CitizensToExcel));

                    // Скрываем индикатор загрузки и показываем успех
                    await HideLoadingIndicatorAsync("Готово!");

                }
                catch (Exception ex)
                {
                    await HideLoadingIndicatorAsync();
                    MessageBox.Show($"Ошибка при обработке файла: {ex.Message}", "Ошибка",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task ShowLoadingIndicatorAsync(string message)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                LoadingOverlay.Visibility = Visibility.Visible;
                LoadingText.Text = message;
            });
        }

        private async Task HideLoadingIndicatorAsync(string successMessage = null)
        {
            if (!string.IsNullOrEmpty(successMessage))
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    LoadingText.Text = successMessage;
                });
                await Task.Delay(1000); // Показываем сообщение об успехе на секунду
            }

            await Dispatcher.InvokeAsync(() =>
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
                StopSpinnerAnimation();
            });
        }

        private void UpdateLoadingText(string message)
        {
            Dispatcher.Invoke(() => LoadingText.Text = message);
        }

        private void StartSpinnerAnimation()
        {
            Dispatcher.Invoke(() =>
            {
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    Duration = TimeSpan.FromSeconds(1),
                    RepeatBehavior = RepeatBehavior.Forever
                };

                SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, animation);
            });
        }

        private void StopSpinnerAnimation()
        {
            Dispatcher.Invoke(() =>
            {
                SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, null);
            });
        }
    }
}