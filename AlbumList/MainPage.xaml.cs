namespace AlbumList
{
    public partial class MainPage : ContentPage
    {
        private AlbumViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new AlbumViewModel("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/library1.json", 1);
            BindingContext = viewModel;
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.CurrentSelection.FirstOrDefault() is Album selectedAlbum) {
                var parameters = new Dictionary<string, object> { 
                    { "Album", selectedAlbum }
                };

                await Shell.Current.GoToAsync(nameof(AlbumDetailPage), parameters);
                ((CollectionView)sender).SelectedItem = null;
            }
        }

    }
}
