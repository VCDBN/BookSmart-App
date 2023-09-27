using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BookSmart.Pages
{
    public partial class ReplacingBooksPage : Page
    {
        //ObservableCollection used instead of basic list for drag drop UI binding (will be referred to as lists from here on).
        public ObservableCollection<string> CallNumbers { get; set; } = new ObservableCollection<string>(); //Used to contain 10 random call numbers
        public ObservableCollection<string> SortedCallNumbers { get; set; } = new ObservableCollection<string>(); //Contains a sorted list of current call numbers for comparison purposes

        //Global variables (used across methods)
        private DispatcherTimer? timer;
        private TimeSpan remainingTime;
        private readonly static Random random = new();
        private static double score = 0;
        private static double totalScore = 0;
        private static int solved = 0;
        private Brush redBrush = new SolidColorBrush(Colors.Red);
        private Brush orangeBrush = new SolidColorBrush(Colors.Orange);
        private Brush yellowGreenBrush = new SolidColorBrush(Colors.Yellow);
        private Brush greenBrush = new SolidColorBrush(Colors.LimeGreen);

        public ReplacingBooksPage()
        {
            InitializeComponent();
            DataContext = this;
            lblInfo.Text = ("Reorder the list of call numbers by dragging them in the list, " +
                "when the numbers are in the correct order click 'Submit' to gain points.\n\n" +
                "You have 1 minute to order as many lists as possible, points will be awarded for speed and the number of lists you manage to solve\n\n" +
                "Submitting an incorrectly ordered list resets the list and no points are awarded.\n\n" +
                "Click 'New Game' to Begin!\n\n" +
                "Good Luck!");
        }


        //-----------------------Call Number Generation-----------------------


        //Clears the call number and sorted call number lists and adds newly generated call numbers.
        private void NewCallNumbers()
        {
            //lists are cleared
            CallNumbers.Clear();
            SortedCallNumbers.Clear();

            //10 newly generated call numbers added to call number list
            for (int i = 0; i < 10; i++)
            {
                CallNumbers.Add(GenerateCallNumber());
            }
            SortedCallNumbers = SortCallNumbers(CallNumbers); //Sorted list of call numbers written to the sorted call numbers list
        }

        //Assembles and returns a single call number.
        public static string GenerateCallNumber()
        {
            int classificationNumber = random.Next(0, 1000); //0-999
            int cutterNumber = random.Next(100); //0-99
            string surnameLetters = GenerateSurname(); //Surname letters generated

            return $"{classificationNumber:D3},{cutterNumber:D2} {surnameLetters}"; //call number assembled, formatted and returned
        }

        //Generates and returns a random 3 letter string for the surname part of the call number.
        public static string GenerateSurname()
        {

            const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //Sandard letters that can make up surname part
            StringBuilder surname = new();
            int surnameLength = 3; //3 letters for the surname part

            for (int i = 0; i < surnameLength; i++) //appends random letters to create surname part
            {
                int index = random.Next(LETTERS.Length);
                surname.Append(LETTERS[index]);
            }
            return surname.ToString();
        }

        //Takes a list of call numbers and returns a sorted list.
        static ObservableCollection<string> SortCallNumbers(ObservableCollection<string> callNumbers)
        {
            //OrderBy used to create an ordered list based on the comparer results
            var sortedCallNumbers = callNumbers.OrderBy(x => x, new CallNumberComparer()).ToList();
            ObservableCollection<string> sortedCallNumbersObs = new ObservableCollection<string>(sortedCallNumbers);
            return sortedCallNumbersObs;
        }

        //IComparer class that uses custom comparison logic for sorting the list in SortCallNumbers().
        class CallNumberComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                var partsX = x.Split(' ');
                var partsY = y.Split(' ');
                //Numeric parts of the call numbers are taken
                double numericPartX = double.Parse(partsX[0]);
                double numericPartY = double.Parse(partsY[0]);

                //Numeric parts are compared
                int numericComparison = numericPartX.CompareTo(numericPartY);

                //If the numbers are not equal, the numeric comparison is returned
                if (numericComparison != 0)
                {
                    return numericComparison;
                }
                //If the numbers are equal, then the string (surname) part is compared, and string comparison returned
                else
                {
                    return string.Compare(partsX[1], partsY[1], StringComparison.Ordinal);
                }
                //This ensures that the list is ordered numerically and then alphabetically where any numbers are identical
            }
        }


        //-----------------------------Event Logic----------------------------


        //User clicks submit button to submit their reordered list.
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //The sorted call numbers list contains the correct order
            //The reordered list is compared to the sorted list
            if (CallNumbers.SequenceEqual(SortedCallNumbers)) //Lists are identical = correct
            {
                NewCallNumbers(); //New call numbers made for next round
                UpdateScoreSuccess(); //Score is updated
                AnimateSuccess(); //Success animation played
            }
            else //incorrect
            {
                NewCallNumbers(); //New call numbers are made for next round
                AnimateFailure(); //Failure animation played
            }
        }

        //User clicks start button to start a new game.
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartGame();//Start game method called
        }

        //User clicks exit button to exit game.
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            KillGame(); //game is stopped  
        }


        //-----------------------------Game Logic-----------------------------


        //Used to start a game.
        private void StartGame()
        {
            ResetGame(); //game is reset
            NewCallNumbers(); //new call numbers generated
            NewTimer(); //new timer created
        }

        //Used to end the game.
        private void EndGame()
        {
            if (timer != null)
            { //if there is an active timer, it is stopped
                timer.Stop();
            }
            MessageBox.Show($"Your final score: {totalScore}", "Time's Up", MessageBoxButton.OK, MessageBoxImage.Information); //User is informed that the game has ended

            btnSubmit.IsEnabled = false; //submit button is disabled to prevent submission when game is over
            btnStart.IsEnabled = true; //start button is enabled to allow the user to start a new game.
        }

        //Prepares variables and UI components for the start of a new game.
        private void ResetGame()
        {
            //Reset Score and Solved Counter and UI components
            solved = 0;
            totalScore = 0;
            tbNumSolved.Text = "Solved: 0";
            tbScore.Text = "Score: 0";

            //Reset Timer TextBlock
            Brush limeGreenBrush = new SolidColorBrush(Colors.LimeGreen);
            tbTimer.Foreground = limeGreenBrush;
            pbRemainingTime.Foreground = limeGreenBrush;
            pbRemainingTime.Value = 60;
            tbTimer.Text = "01:00";

            //Reset Buttons
            btnStart.IsEnabled = false;
            btnSubmit.IsEnabled = true;
        }

        //Stops game timer and navigates back to menu page. 
        private void KillGame()
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

        //Creates new game timer.
        private void NewTimer()
        {
            timer = new DispatcherTimer();
            timer.Stop();
            remainingTime = TimeSpan.FromSeconds(60);
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

        //Calculates and updates score for correct answer.
        private void UpdateScoreSuccess()
        {
            solved++; //number of correct solutions incremented
            score = solved * remainingTime.TotalSeconds; //score calulation: number of solved solutions multiplied by the remaining time in seconds
            totalScore += score; //round score is added to total score
            tbNumSolved.Text = $"Solved: {solved}"; //number of solutions displayed
            tbScore.Text = $"Score: {totalScore}";  //score displayed
        }

        //Updates the game timer visually and defines timer logic.
        private void UpdateGameTimer()
        {
            tbTimer.Text = $"{remainingTime.Minutes:00}:{remainingTime.Seconds:00}"; //Timer UI updated
            pbRemainingTime.Value = remainingTime.TotalSeconds; //Progress bar value updated

            switch (remainingTime.TotalSeconds) //Colour of the timer and progress bar changed based on remaining time
            {
                case <= 0:
                    EndGame(); //The game is ended when the time remaining reaches 0
                    break;
                case <= 10:
                    tbTimer.Foreground = redBrush;
                    pbRemainingTime.Foreground = redBrush;
                    break;
                case <= 20:
                    tbTimer.Foreground = orangeBrush;
                    pbRemainingTime.Foreground = orangeBrush;
                    break;
                case <= 40:
                    tbTimer.Foreground = yellowGreenBrush;
                    pbRemainingTime.Foreground = yellowGreenBrush;
                    break;
                case <= 60:
                    tbTimer.Foreground = greenBrush;
                    pbRemainingTime.Foreground = greenBrush;
                    break;
            }
        }

        //Animation when answer is correct.
        private void AnimateSuccess()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush limeGreenBrush = new SolidColorBrush(Colors.LimeGreen);

            ColorAnimation toLimeGreenAnimation = new ColorAnimation
            {
                To = limeGreenBrush.Color,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation toGrayAnimation = new ColorAnimation
            {
                To = grayBrush.Color,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(toLimeGreenAnimation);
            storyboard.Children.Add(toGrayAnimation);

            Storyboard.SetTarget(toLimeGreenAnimation, lbCallNumbers);
            Storyboard.SetTargetProperty(toLimeGreenAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(toGrayAnimation, lbCallNumbers);
            Storyboard.SetTargetProperty(toGrayAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            storyboard.Begin(this); //The colour of the listbox's border pulses limeGreen.
        }

        //Animation when anser is incorrect.
        private void AnimateFailure()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);

            ColorAnimation toRedAnimation = new ColorAnimation
            {
                To = redBrush.Color,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            ColorAnimation toGrayAnimation = new ColorAnimation
            {
                To = grayBrush.Color,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(toRedAnimation);
            storyboard.Children.Add(toGrayAnimation);

            Storyboard.SetTarget(toRedAnimation, lbCallNumbers);
            Storyboard.SetTargetProperty(toRedAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(toGrayAnimation, lbCallNumbers);
            Storyboard.SetTargetProperty(toGrayAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            storyboard.Begin(this); //The colour of the listbox's border pulses red.
        }
    }
}