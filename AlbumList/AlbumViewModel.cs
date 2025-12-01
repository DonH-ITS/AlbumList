using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlbumList
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private string _url;
        private int _libraryNo;
        private Album _selectedAlbum;
        private string _libraryName = "My Music Library";

        public ObservableCollection<Album> Albums { get; set; }
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

        public event PropertyChangedEventHandler? PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
