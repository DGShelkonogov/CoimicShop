using Avalonia.Controls;

namespace ComicShop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DBConnection.getConnection();
        }
    }
}
