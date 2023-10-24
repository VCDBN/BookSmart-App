using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BookSmart.Pages
{
    /// <summary>
    /// Interaction logic for IdentifyingAreasPage.xaml
    /// </summary>
    public partial class IdentifyingAreasPage : Page
    {
        TimeSpan remainingTime;
        DispatcherTimer timer;
        int currentStreak = 0;
        int maxStreak = 0;
        int gameLength = 40;

        private Brush redBrush = new SolidColorBrush(Colors.Red);
        private Brush orangeBrush = new SolidColorBrush(Colors.Orange);
        private Brush yellowGreenBrush = new SolidColorBrush(Colors.Yellow);
        private Brush greenBrush = new SolidColorBrush(Colors.LimeGreen);
        public ObservableCollection<string> Table1 { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Table2 { get; set; } = new ObservableCollection<string>();
        string[] numbersArray = { "1", "2", "3", "4" };
        string[] lettersArray = { "A", "B", "C", "D", "E", "F", "G" };
        List<string> selectedKeys = new();
        List<string> selectedDescriptions = new();
        bool selectingDescriptions; //type of question, user selects descriptions = true, user selects call numbers = false

        Dictionary<string, string> deweyCategories = new Dictionary<string, string>
        {
            { "000", "General Knowlege" },
            { "100", "Philosophy & Psychology" },
            { "200", "Religion" },
            { "300", "Social Sciences" },
            { "400", "Languages" },
            { "500", "Science" },
            { "600", "Technology" },
            { "700", "Arts & Recreation" },
            { "800", "Literature" },
            { "900", "History & Geography" }         
        };
        public IdentifyingAreasPage()
        {
            DataContext = this;
            InitializeComponent();
            lblInfo.Text = ("Match the call numbers with the correct top-level category description.\n\n" +
                "You have 40 seconds per round to submit your answers.\n\n" +
                "The game will end when you run out of time.\n\n" +
                "Click 'New Game' to begin!");
            cmb1.ItemsSource = lettersArray;
            cmb2.ItemsSource = lettersArray;
            cmb3.ItemsSource = lettersArray;
            cmb4.ItemsSource = lettersArray;
        }

        private void NewRound()
        {
            PopulateTables();
            if (timer != null)
            {
                timer.Stop(); //Timer is stopped
            }
            pbRemainingTime.Foreground = greenBrush;
            pbRemainingTime.Value = gameLength;
            NewTimer();
            cmb1.SelectedIndex = -1;
            cmb2.SelectedIndex = -1;
            cmb3.SelectedIndex = -1;
            cmb4.SelectedIndex = -1;
        }

        private void PopulateTables()
        {
            int numberCounter = 0;
            int letterCounter = 0;

            Table1.Clear();
            Table2.Clear();
            Random random = new Random();

            // Get the keys and descriptions from the original dictionary
            List<string> keys = deweyCategories.Keys.ToList();
            List<string> descriptions = deweyCategories.Values.ToList();

            // Shuffle the keys randomly
            int n = keys.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);

                string key = keys[k];
                keys[k] = keys[n];
                keys[n] = key;

                string description = descriptions[k];
                descriptions[k] = descriptions[n];
                descriptions[n] = description;
            }

            // Create a new shuffled dictionary
            Dictionary<string, string> shuffledDeweyCategories = new Dictionary<string, string>();

            for (int i = 0; i < keys.Count; i++)
            {
                shuffledDeweyCategories[keys[i]] = descriptions[i];
            }

            bool fiftyFifty = random.Next(2) == 0;

            if (fiftyFifty) //matching 4 numbers to 7 descriptions
            {
                selectingDescriptions = true;
                selectedKeys = shuffledDeweyCategories.Keys.Take(4).ToList();
                selectedDescriptions = shuffledDeweyCategories.Values.Take(7).ToList();

                ShuffleList(selectedKeys);
                ShuffleList(selectedDescriptions);

                foreach(string key in selectedKeys)
                {
                    Table1.Add($"{numbersArray[numberCounter]}  {key}");
                    numberCounter++;
                }

                foreach (string description in selectedDescriptions)
                {
                    Table2.Add($"{lettersArray[letterCounter]}  {description}");
                    letterCounter++;
                }


            }
            else //matching 4 descriptions to 7 numbers
            {
                selectingDescriptions = false;
                selectedKeys = shuffledDeweyCategories.Keys.Take(7).ToList();
                selectedDescriptions = shuffledDeweyCategories.Values.Take(4).ToList();

                ShuffleList(selectedKeys);
                ShuffleList(selectedDescriptions);

                foreach (string key in selectedKeys)
                {
                    Table1.Add($"{lettersArray[letterCounter]}  {key}");
                    letterCounter++;
                }

                foreach (string description in selectedDescriptions)
                {
                    Table2.Add($"{numbersArray[numberCounter]}  {description}");
                    numberCounter++;
                }
            }
        }

        static void ShuffleList<T>(List<T> list) //Fisher-Yates shuffle algorithm
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                // Swap list[i] and list[j]
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(
            ValidateSelection(cmb1, 0)&&
            ValidateSelection(cmb2, 1)&&
            ValidateSelection(cmb3, 2)&&
            ValidateSelection(cmb4, 3))
            {//correct answers
                currentStreak++;
                if (currentStreak > maxStreak)
                {
                    maxStreak = currentStreak;
                }
                NewRound();
                DisplaySuccess();
            }
            else
            {
                currentStreak = 0;
                DisplayFailure();
            }
            lblStreak.Content = maxStreak.ToString();
            lblCurrentStreak.Content = currentStreak.ToString();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;
            Frame mainFrame = (Frame)mainWindow.FindName("mainFrame");

            if (timer != null)
            {
                timer.Stop(); //Timer is stopped
            }

            if (mainFrame != null)
            {
                mainFrame.Navigate(new MenuPage()); //Navigation to menu page
            }
        }

        private bool ValidateSelection(ComboBox cmb, int questionIndex)
        {
            int answerIndex;

            if (cmb.SelectedIndex > -1)
            {
                answerIndex = cmb.SelectedIndex;
            }
            else { 
                return false; 
            }

            
            if (selectingDescriptions)
            {
                string question = selectedKeys[questionIndex];
                string answer = selectedDescriptions[answerIndex];
                if (deweyCategories[question].Equals(answer))
                {
                    return true;
                }
            }
            else
            {
                string question = selectedDescriptions[questionIndex];
                string answer = selectedKeys[answerIndex];
                if (deweyCategories[answer].Equals(question))
                {
                    return true;
                }
            }
                        
            return false;
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewRound();
            btnSubmit.IsEnabled = true;
            btnNewGame.IsEnabled = false;
        }

        private void NewTimer()
        {
            timer = new DispatcherTimer();
            timer.Stop();
            remainingTime = TimeSpan.FromSeconds(gameLength);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //Defines when the game timer gets updated.
        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateGameTimer();
        }

        private void UpdateGameTimer()
        {
            pbRemainingTime.Value = remainingTime.TotalSeconds;
            switch (remainingTime.TotalSeconds) //Colour of the progress bar changed based on remaining time
            {
                case <= 0:
                    EndGame(); //The game is ended when the time remaining reaches 0
                    break;
                case <= 10:
                    pbRemainingTime.Foreground = redBrush;
                    break;
                case <= 20:
                    pbRemainingTime.Foreground = orangeBrush;
                    break;
                case <= 30:
                    pbRemainingTime.Foreground = yellowGreenBrush;
                    break;
                case <= 40:
                    pbRemainingTime.Foreground = greenBrush;
                    break;
            }
        }

        private void EndGame()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            MessageBox.Show($"Your highest streak: {maxStreak}", "Time's Up", MessageBoxButton.OK, MessageBoxImage.Information);

            ResetGame();
        }

        private void ResetGame()
        {
            lblStreak.Content = "0";
            lblCurrentStreak.Content = "0";
            currentStreak = 0;
            maxStreak = 0;
            btnSubmit.IsEnabled = false;
            btnNewGame.IsEnabled = true;
        }

        private async void DisplaySuccess()
        {
            SolidColorBrush greenBrush = new SolidColorBrush(Colors.Lime);
            lblResult.Foreground = greenBrush;
            lblResult.Content = "Correct";
            await Task.Delay(TimeSpan.FromSeconds(2));
            lblResult.Content = " ";
        }

        private async void DisplayFailure()
        {
            SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
            lblResult.Foreground = redBrush;
            lblResult.Content = "Incorrect";
            await Task.Delay(TimeSpan.FromSeconds(2));
            lblResult.Content = " ";
        }

    }
    }
