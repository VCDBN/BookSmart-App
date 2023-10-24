using System.Windows;
using System.Windows.Controls;

namespace BookSmart.Pages
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }
        
        //When the 'replacing books' button is pressed, the user is navigated to the 'replacing books' game page.
        private void btnReplacingBooks_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            Frame mainFrame = (Frame)mainWindow.FindName("mainFrame");

            if (mainFrame != null)
            {
                mainFrame.Navigate(new ReplacingBooksPage());
            }
        }
        
        //Game not yet implemented.
        private void btnIdentifyingAreas_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            Frame mainFrame = (Frame)mainWindow.FindName("mainFrame");

            if (mainFrame != null)
            {
                mainFrame.Navigate(new IdentifyingAreasPage());
            }
        }

        //Game not yet implemented.
        private void btnFindingCallNumbers_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming Soon", "Finding Call Numbers", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
