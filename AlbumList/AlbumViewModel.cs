using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AlbumList
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private string _url;
        private int _libraryNo;
        private Album _selectedAlbum;
        private string _libraryName = "My Music Library";
        private bool _showActivity = false;

        private ObservableCollection<Album> _albums;
        public bool IsLoaded { get; private set; } = false;
        public ObservableCollection<Album> Albums
        {
            get { return _albums; }
            set
            {
                if (_albums != value)
                {
                    _albums = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ShowActivity
        {
            get => _showActivity;
            set
            {
                _showActivity = value;
                OnPropertyChanged();
            }
        }

        
        public Album SelectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                if (_selectedAlbum != value) {
                    _selectedAlbum = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LibraryName
        {
            get => _libraryName;
            set
            {
                if (_libraryName != value) {
                    _libraryName = value;
                    OnPropertyChanged();
                    // Preferences updating could go here, but may be excessive to write all the time
                    Preferences.Default.Set($"library{_libraryNo}", _libraryName);
                }
            }
        }

        public AlbumViewModel(string url, int numb) {
            _url = url;
            _libraryNo = numb;
            _libraryName = Preferences.Default.Get($"library{numb}", "My Music Library");
        }

        public async Task DownloadAlbums()
        {
            if (!IsLoaded)
            {
                ShowActivity = true;
                // Simulate a delay in downloading
                await Task.Delay(500);
                string filename = Path.Combine(FileSystem.Current.AppDataDirectory, $"library{_libraryNo}.json");
                if (File.Exists(filename))
                {
                    try
                    {
                        // Load the File from the storage
                        using FileStream inputStream = File.OpenRead(filename);
                        using StreamReader reader = new StreamReader(inputStream);
                        string contents = await reader.ReadToEndAsync();
                        Albums = JsonSerializer.Deserialize<ObservableCollection<Album>>(contents);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    // Load the File from the Web
                    try
                    {
                        var response = await App.HttpClient.GetAsync(_url);
                        if (response.IsSuccessStatusCode)
                        {
                            string contents = await response.Content.ReadAsStringAsync();
                            Albums = JsonSerializer.Deserialize<ObservableCollection<Album>>(contents);
                            using FileStream outputStream = File.Create(filename);
                            using StreamWriter writer = new StreamWriter(outputStream);
                            await writer.WriteAsync(contents);

                        }
                    }
                    catch
                    {

                    }
                }
                IsLoaded = true;
                ShowActivity = false;
            }
        }

        public void SortAlbums(string sortBy)
        {
            switch (sortBy)
            {
                case "Title":
                    Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Title));
                    break;
                case "Artist":
                    Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Artist));
                    break;
                case "Year":
                    Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Year));
                    break;
            }
        }

        public async Task SaveLibrary()
        {
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, $"library{_libraryNo}.json");
            string jsonContents = JsonSerializer.Serialize(Albums);
            using FileStream outputStream = File.Create(filename);
            using StreamWriter writer = new StreamWriter(outputStream);
            await writer.WriteAsync(jsonContents);
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
