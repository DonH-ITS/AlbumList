using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlbumList
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
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
                    Preferences.Default.Set("libraryname", _libraryName);
                }
            }
        }

        public AlbumViewModel() {
            _libraryName = Preferences.Default.Get("libraryname", "My Music Library");
            Albums = new ObservableCollection<Album>
            {
                new Album { Title = "Nevermind", Artist = "Nirvana", Year = 1991, Genre = "Grunge", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/b/b7/NirvanaNevermindalbumcover.jpg" },
                new Album { Title = "Dirt", Artist = "Alice in Chains", Year = 1992, Genre = "Grunge", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/f/f9/Dirt_%28Alice_in_Chains_album_-_cover_art%29.jpg" },
                new Album { Title = "Mellon Collie and the Infinite Sadness", Artist = "Smashing Pumpkins", Year = 1995, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/7/76/The_Smashing_Pumpkins_-_Mellon_Collie_And_The_infinite_Sadness.jpg" },
                new Album { Title = "The Colour and the Shape", Artist = "Foo Fighters", Year = 1997, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/0d/FooFighters-TheColourAndTheShape.jpg" },
                new Album { Title = "Songs for the Deaf", Artist = "Queens of the Stone Age", Year = 2002, Genre = "Stoner Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/01/Queens_of_the_Stone_Age_-_Songs_for_the_Deaf.png" },
                new Album { Title = "Daydream Nation", Artist = "Sonic Youth", Year = 1988, Genre = "Noise Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/a/ad/Daydreamnation.png" },
                new Album { Title = "Doolittle", Artist = "Pixies", Year = 1989, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/6b/Pixies-Doolittle.jpg" },
                new Album { Title = "Toxicity", Artist = "System of a Down", Year = 2001, Genre = "Alternative Metal", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/64/SystemofaDownToxicityalbumcover.jpg" },
                new Album { Title = "The Downward Spiral", Artist = "Nine Inch Nails", Year = 1994, Genre = "Industrial Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3b/Official_Album_Cover_of_%22The_Downward_Spiral%22_by_Nine_Inch_Nails.png" },
                new Album { Title = "Ten", Artist = "Pearl Jam", Year = 1991, Genre = "Grunge", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/6f/Pearl_Jam_Ten_Alt_Cover.jpg" },
                new Album { Title = "Green Mind", Artist = "Dinosaur Jr", Year = 1991, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/1/14/DinosaurJrGreenMind.jpg" },
                new Album { Title = "Married", Artist = "Kills Birds", Year = 2021, Genre = "Alternative Rock", CoverUrl = "https://f4.bcbits.com/img/a4221726546_10.jpg" },
                new Album { Title = "Live Through This", Artist = "Hole", Year = 1994, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/c/c5/Hole_-_Live_Through_This.png" },
                new Album { Title = "Let Go", Artist = "Nada Surf", Year = 1996, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/96/Let_go_by_nada_surf.jpg" },
                new Album { Title = "Stories from the City, Stories from the Sea", Artist = "PJ Harvey", Year = 2000, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/a/a3/Stories_From_The_City%2C_Stories_From_The_Sea.jpg" },
                new Album { Title = "Sweet Oblivion", Artist = "Screaming Trees", Year = 1992, Genre = "Grunge", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/a/aa/Screaming_Trees_Sweet_Oblivion.jpg" },
                new Album { Title = "Superunknown", Artist = "Soundgarden", Year = 1994, Genre = "Grunge", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3a/Superunknown.jpg" },
                new Album { Title = "Core", Artist = "Stone Temple Pilots", Year = 1992, Genre = "Grunge / Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/1/19/Stonetemplepilotscore.jpeg" },
                new Album { Title = "Gentlemen", Artist = "The Afghan Whigs", Year = 1993, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/33/Gentlemen.jpg" },
                new Album { Title = "Blue Album", Artist = "Weezer", Year = 1994, Genre = "Alternative Rock", CoverUrl = "https://upload.wikimedia.org/wikipedia/en/7/70/Weezer_-_Blue_Album.png" }
            };

        }

        public event PropertyChangedEventHandler? PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
