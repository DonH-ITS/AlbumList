namespace AlbumList;

[QueryProperty(nameof(AlbumProperty), "Album")]
public partial class AlbumDetailPage : ContentPage
{
    private bool _isDownloading = false;
    private bool _hasImage = false;

    public bool IsDownloading
    {
        get => _isDownloading;
        set
        {
            if (_isDownloading != value) {
                _isDownloading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotDownloading));
            }
        }
    }
    // IsNotDownloading will always be the opposite of IsDownloading
    // This is a short hand way of doing
    /* public bool IsNotDownloading{
     *      get{
     *          return !IsDownloading;
     *      }
     * }
     */
    public bool IsNotDownloading => !IsDownloading;

	public Album AlbumProperty { get; set; } 
	public AlbumDetailPage()
	{
		InitializeComponent();
        // Do two columns on windows, one on Android
        if (DeviceInfo.Platform == DevicePlatform.Android) {
            for (int i = 0; i < 2; ++i) {
                MainGrid.AddRowDefinition(new RowDefinition());
            }

            Grid.SetColumn(ItemCollectionViewParent, 0);
            Grid.SetRow(ItemCollectionViewParent, 1);
        }
        else {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            Grid.SetColumn(ItemCollectionViewParent, 1);
            Grid.SetRow(ItemCollectionViewParent, 0);
        }
        if (DeviceInfo.Platform == DevicePlatform.WinUI) {
            DetailsStack.WidthRequest = 300;
        }
        else if (DeviceInfo.Platform == DevicePlatform.Android) {
            DetailsStack.SizeChanged += DetailsStack_SizeChanged;
        }
    }

    private void DetailsStack_SizeChanged(object? sender, EventArgs e) {
        // On android we need to set ItemCollectionView height differently
        if (DeviceInfo.Platform == DevicePlatform.Android) {
            ItemCollectionView.HeightRequest = this.Height - DetailsStack.Height - TrackText.Height - 40;
        }
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await Task.Delay(50);
        if (!_hasImage) {
            IsDownloading = true;

            BindingContext = AlbumProperty;

            try {
                byte[] data = await App.HttpClient.GetByteArrayAsync(AlbumProperty.CoverUrl);

                ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(data));
                ImageDisplay.Source = imgSrc;
            }
            catch (Exception ex) {
                //await DisplayAlert("Error Downloading Image", "Could not download Image", "OK");
                ImageDisplay.BackgroundColor = Colors.Black;
            }

            IsDownloading = false;
            _hasImage = true;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnSizeAllocated(double width, double height) {
        base.OnSizeAllocated(width, height);

        // Set track list height to fill available space (minus some padding/space for header)
        // This makes scrollview for only the tracklist section
        ItemCollectionView.HeightRequest = height - 100;
    }
}