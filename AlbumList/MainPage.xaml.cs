namespace AlbumList
{
    public partial class MainPage : ContentPage
    {
        private AlbumViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new AlbumViewModel();
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

        protected override void OnDisappearing() {
            base.OnDisappearing();
            // Preferences saving could go here
            /* if(Preferences.Default.Get("libraryname", "My Music Library") != viewModel.LibraryName) {
                Preferences.Default.Set("libraryname", viewModel.LibraryName);
            }*/
        }


    }
}
