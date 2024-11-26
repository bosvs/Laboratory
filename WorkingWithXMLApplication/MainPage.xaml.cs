using WorkingWithXMLApplication.ParsingStrategy;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

namespace WorkingWithXMLApplication
{
    public partial class MainPage : ContentPage
    {
        private string? _selectedFilePath;
        private string _selectedParsingMethod = "LINQ";

        public MainPage()
        {
            InitializeComponent();
        }

        public string SelectedParsingMethod => _selectedParsingMethod;
        public string? SelectedFilePath => _selectedFilePath;

        public static List<string?> GetUniqueValues(string filePath, string node, string attribute)
        {
            var xmlDocument = XDocument.Load(filePath);
            return xmlDocument.Descendants(node)
                              .Select(element => (string?)element.Attribute(attribute))
                              .Where(value => !string.IsNullOrEmpty(value))
                              .Distinct()
                              .ToList();
        }

        public async void OnInfoButtonClicked(object sender, EventArgs e)
        {
            string Inf = "Розробив програму - Остапович Всеволод, студент групи К - 26\n" +
                                 "Програма дозволяє обробляти XML файл за допомогою технологій:" +
                                 " LINQ, SAX та DOM.\nЗа бажанням (в мене бажання це реалізовувати не було, але прийшлось) " +
                                 "можна перетворити файл в HTML" +
                                 ".\nОбраний варіант - \"Студентський парламент\" ";
                                

            bool result = await DisplayAlert("Загальна інформація", Inf, "Ну такоє", "Це супер проєкт");
        }

        private async Task HandleFileSelectionAsync(string fileType, string pickerTitle, Action<string> onSuccess)
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { fileType } }
            });

            var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = pickerTitle, FileTypes = customFileType });

            if (result?.FullPath is string filePath)
                onSuccess(filePath);
        }

        public async void OnOpenFileButtonClicked(object sender, EventArgs e)
        {
            await HandleFileSelectionAsync(".xml", "Choose XML file", filePath =>
            {
                _selectedFilePath = filePath;
                PathToFile.Text = $"Opened {filePath}";
                ToggleUIElementsVisibility(true);

                PopulatePicker(DayPicker, GetUniqueValues(filePath, "Event", "Day"));
                PopulatePicker(RoomPicker, GetUniqueValues(filePath, "Event", "Room"));

#if WINDOWS
                Application.Current.Windows[0].Width = 750;
                Application.Current.Windows[0].Height = 770;
#endif
            });
        }

        private void ToggleUIElementsVisibility(bool isVisible)
        {
            FiltersMenuFirstBlock.IsVisible = isVisible;
            FiltersMenuSecondBlock.IsVisible = isVisible;
            OpenScheduleButton.IsVisible = isVisible;
            HTMLTransorm.IsVisible = isVisible;
            // new update
            ClearFilters.IsVisible = isVisible;
        }

        private static void PopulatePicker(Picker picker, IEnumerable<string?> items)
        {
            picker.Items.Clear();
            picker.Items.Add(" ");
            foreach (var item in items.Where(i => !string.IsNullOrWhiteSpace(i)))
                picker.Items.Add(item);
        }

        public void OnClearFiltersButtonClicked(object sender, EventArgs e)
        {
            EventNameFilter.Text = string.Empty;
            StudentNameFilter.Text = string.Empty;
            TimeFilter.Time = TimeSpan.Zero;
            DayPicker.SelectedIndex = -1;
            RoomPicker.SelectedIndex = -1;
        }

        

        private async void OnOpenScheduleButtonClicked(object sender, EventArgs e)
        {
            if (_selectedFilePath == null)
            {
                await DisplayAlert("Error", "File not chosen", "ОК");
                return;
            }
            _selectedParsingMethod = await DisplayActionSheet("Select Parsing Technology", "Cancel", null, "LINQ", "SAX", "DOM");

            if (_selectedParsingMethod == "Cancel")
                return;

            IParsingStrategy selectedParsingStrategy = _selectedParsingMethod switch
            {
                "LINQ" => new LINQParsingStrategy(),
                "SAX" => new SAXParsingStrategy(),
                "DOM" => new DOMParsingStrategy(),
                _ => throw new InvalidOperationException("Unkwnown parsing strategy")
            };

            var newResult = new InfoSheet(
                _selectedFilePath,
                selectedParsingStrategy,
                StudentNameFilter.Text,
                TimeFilter.Time == TimeSpan.Zero ? null : TimeFilter.Time.ToString(),
                EventNameFilter.Text,
                RoomPicker.SelectedItem as string,
                DayPicker.SelectedItem as string);

            await Navigation.PushAsync(newResult);
        }

        private async void OnHTMLTransformButtonClicked(object sender, EventArgs e)
        {
            await HandleFileSelectionAsync(".xsl", "Select XSL файл", async xslFilePath =>
            {
                try
                {
                    if (_selectedFilePath == null)
                        throw new InvalidOperationException("File not chosen");

                    using var stream = new MemoryStream();
                    var xsl = new XslCompiledTransform();
                    xsl.Load(xslFilePath);

                    using (var reader = XmlReader.Create(_selectedFilePath))
                    using (var writer = XmlWriter.Create(stream, xsl.OutputSettings))
                    {
                        xsl.Transform(reader, writer);
                    }

                    stream.Position = 0;
                    var saveResult = await FileSaver.Default.SaveAsync("TransformedResult.html", stream);
                    saveResult.EnsureSuccess();

                    await Toast.Make("HTML file was saved!").Show();
                }
                catch (Exception ex)
                {
                    await Toast.Make($"File wasn't saved!\n{ex.Message}").Show();
                }
            });
        }
    }
}
