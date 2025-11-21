namespace AlbumList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AlbumDetailPage), typeof(AlbumDetailPage));
        }
    }
}
