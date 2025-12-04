namespace AlbumList
{
    public partial class MainPage : ContentPage {
        private AlbumViewModel viewModel;

        public MainPage(string url, int no) {
            InitializeComponent();
            viewModel = new AlbumViewModel(url, no);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            await Task.Delay(50);
            try {
                if (!viewModel.IsLoaded) {
                    await viewModel.DownloadAlbums();
                    viewModel.TabText = Shell.Current?.CurrentItem?.CurrentItem?.CurrentItem;
                }
            }
            catch (Exception ex) {
                await DisplayAlert("exception", $"Exception is {ex.Message}", "ok");
            }
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

        private void SortByClicked(object sender, EventArgs e) {
            Button btn = (Button)sender;
            viewModel.SortAlbums(btn.Text);
        }

        private async void AddAlbumButton_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new AddAlbumPage(viewModel));
        }

        private async void SaveButton_Clicked(object sender, EventArgs e) {
            SaveButton.IsEnabled = false;
            await viewModel.SaveLibrary();
            SaveButton.Text = "Library Saved";
            await Task.Delay(1500);
            SaveButton.Text = "Save Library";
            SaveButton.IsEnabled = true;

        }

        private async void EditButton_Clicked(object sender, EventArgs e) {
            var album = (sender as Button)?.CommandParameter as Album;
            if (album != null) {
                await Navigation.PushAsync(new AddAlbumPage(viewModel, album));
            }
        }
    }
}
