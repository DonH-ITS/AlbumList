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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel != null && !viewModel.IsLoaded)
            {
                await Task.Delay(50);
                await viewModel.DownloadAlbums();
            }
        }

        private void SortByClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            viewModel.SortAlbums(btn.Text);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            SaveButton.IsEnabled = false;
            await viewModel.SaveLibrary();
            SaveButton.Text = "Library Saved";
            await Task.Delay(1500);
            SaveButton.Text = "Save Library";
            SaveButton.IsEnabled = true;
        }

    }
}
