using System.Collections.ObjectModel;

namespace AlbumList;

public partial class AddAlbumPage : ContentPage
{
    // Temporary track list
    public ObservableCollection<string> Tracks { get; private set; } 

    private bool _editing;
    private readonly AlbumViewModel _albumViewModel;
    private readonly Album? _album;

    public AddAlbumPage(AlbumViewModel albumViewModel) {
        InitializeComponent();
        _albumViewModel = albumViewModel;
        Tracks = new ObservableCollection<string>();
        TracksCollectionView.BindingContext = this;
        _editing = false;
        _album = null;
        
    }

    public AddAlbumPage(AlbumViewModel albumViewModel, Album album) {
        InitializeComponent();
        AddAlbumBtn.Text = "Save Changes";
        _albumViewModel = albumViewModel;
        TitleEntry.Text = album.Title;
        ArtistEntry.Text = album.Artist;
        YearEntry.Text = album.Year.ToString();
        GenreEntry.Text = album.Genre;
        CoverUrlEntry.Text = album.CoverUrl;
        Tracks = new ObservableCollection<string>(album.Tracks);
        TracksCollectionView.BindingContext = this;
        _album = album;
        _editing = true;
    }

    // Add a track
    private void AddTrackClicked(object sender, EventArgs e) {
        if (!string.IsNullOrWhiteSpace(NewTrackEntry.Text)) {
            Tracks.Add(NewTrackEntry.Text.Trim());
            NewTrackEntry.Text = string.Empty;
        }
    }

    // Remove a track
    private void RemoveTrackClicked(object sender, EventArgs e) {
        if (sender is Button btn && btn.BindingContext is string track) {
            Tracks.Remove(track);
        }
    }

    // Add album to collection
    private async void AddAlbumClicked(object sender, EventArgs e) {
        // Simple validation
        if (string.IsNullOrWhiteSpace(TitleEntry.Text)) {
            await DisplayAlert("Error", "Album title is required.", "OK");
            return;
        }
        int year;
        if(!int.TryParse(YearEntry.Text.Trim(), out year)) {
            await DisplayAlert("Error", "Invalid Year Given.", "OK");
            return;
        }
        if (_editing) {
            _album!.Title = TitleEntry.Text.Trim();
            _album.Artist = ArtistEntry.Text.Trim();
            _album.Year = year;
            _album.Genre = GenreEntry.Text.Trim();
            _album.CoverUrl = CoverUrlEntry.Text.Trim();
            _album.Tracks = Tracks.ToList();
            var idx = _albumViewModel.Albums.IndexOf(_album);
            if (idx >= 0) {
                // Replace item to force UI refresh (cheap hack)
                _albumViewModel.Albums[idx] = _album;
            }
        }
        else {
            var newAlbum = new Album
            {
                Title = TitleEntry.Text.Trim(),
                Artist = ArtistEntry.Text.Trim(),
                Year = year,
                Genre = GenreEntry.Text.Trim(),
                CoverUrl = CoverUrlEntry.Text.Trim(),
                Tracks = Tracks.ToList()
            };

            // Add to AlbumViewModel's collection
            _albumViewModel?.Albums.Add(newAlbum);
        }

        // Close page
        await Navigation.PopAsync();
    }
}