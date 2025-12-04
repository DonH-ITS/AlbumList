using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AlbumList
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private Album _selectedAlbum;
        private string _libraryName = "My Music Library";
        private string _url;
        private int _libraryNo;
        private bool _showActivity = true;
        private int _sortMode = 0;

        public ShellContent TabText { get; set; }
        private ObservableCollection<Album> _albums;
        public ObservableCollection<Album> Albums
        {
            get => _albums;
            set
            {
                _albums = value;
                OnPropertyChanged();
            }
        }

        public int SortMode
        {
            get
            {
                return _sortMode;
            }
            set{
                if (_sortMode != value) {
                    _sortMode = value;
                    Preferences.Default.Set($"SortMode{_libraryNo}", _sortMode);
                }
            }
        }

        public bool IsLoaded { get; private set; } = false;
        
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
                    TabText.Title = _libraryName;
                }
            }
        }
        
        public bool ShowActivity
        {
            get => _showActivity;
            set
            {
                if (_showActivity != value) {
                    _showActivity = value;
                    OnPropertyChanged();
                }
            }
        }

        public AlbumViewModel(string url, int numb) {
            _url = url;
            _libraryNo = numb;
            _libraryName = Preferences.Default.Get($"library{numb}", $"My Library {numb}");
            _sortMode = Preferences.Default.Get($"SortMode{_libraryNo}", 0);
        }


        public async Task DownloadAlbums() {
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, $"library{_libraryNo}.json");
            if (File.Exists(filename)) {
                // Use the downloaded copy
                try {
                    using FileStream inputStream = File.OpenRead(filename);
                    using StreamReader reader = new StreamReader(inputStream);
                    string contents = await reader.ReadToEndAsync();
                    Albums = JsonSerializer.Deserialize<ObservableCollection<Album>>(contents);
                }
                catch (Exception) {

                }
            }
            // Download the starting library
            else {
                try {
                    var response = await App.HttpClient.GetAsync(_url);
                    if (response.IsSuccessStatusCode) {
                        string contents = await response.Content.ReadAsStringAsync();
                        using FileStream outputStream = File.Create(filename);
                        using StreamWriter writer = new StreamWriter(outputStream);
                        await writer.WriteAsync(contents);
                        Albums = JsonSerializer.Deserialize<ObservableCollection<Album>>(contents);
                    }   
                }
                catch (Exception ex) {

                }
            }
            IsLoaded = true;
            ShowActivity = false;
        }

        public async Task SaveLibrary() {
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, $"library{_libraryNo}.json");
            string jsonContents = JsonSerializer.Serialize(Albums);
            using FileStream outputStream = File.Create(filename);
            using StreamWriter writer = new StreamWriter(outputStream);
            await writer.WriteAsync(jsonContents);
        }

        public void SortAlbums(string sortBy) {
            // If someone clicks the title button and it is already ordered, it swaps the order around
            switch (sortBy) {
                case "Title":
                    if (SortMode == 1) {
                        Albums = new ObservableCollection<Album>(Albums.OrderByDescending(a => a.Title));
                        SortMode = 2;
                    }
                    else {
                        Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Title));
                        SortMode = 1;
                    }
                    break;
                case "Artist":
                    if (SortMode == 3) {
                        Albums = new ObservableCollection<Album>(Albums.OrderByDescending(a => a.Artist));
                        SortMode = 4;
                    }
                    else {
                        Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Artist));
                        SortMode = 3;
                    }
                    break;
                case "Year":
                    if (SortMode == 5) {
                        Albums = new ObservableCollection<Album>(Albums.OrderByDescending(a => a.Year));
                        SortMode = 6;
                    }
                    else {
                        Albums = new ObservableCollection<Album>(Albums.OrderBy(a => a.Year));
                        SortMode = 5;
                    }
                    break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
