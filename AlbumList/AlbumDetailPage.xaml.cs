namespace AlbumList;

[QueryProperty(nameof(AlbumProperty), "Album")]
public partial class AlbumDetailPage : ContentPage
{
    private bool _isDownloading = false;

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
        
	}

    protected override async void OnAppearing() {
        base.OnAppearing();
        await Task.Delay(50);
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


    }

    private async void Button_Clicked(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("..");
    }
}