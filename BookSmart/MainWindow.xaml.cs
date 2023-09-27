using BookSmart.Pages;
using System.Windows;

namespace BookSmart
{
    public partial class MainWindow : Window
    {
        //On startup, the user is navigated to the menu page.
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new MenuPage());
        }
    }
}
