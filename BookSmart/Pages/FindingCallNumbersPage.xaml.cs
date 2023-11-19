using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace BookSmart.Pages
{
    /// <summary>
    /// Interaction logic for FindingCallNumbersPage.xaml
    /// </summary>
    public partial class FindingCallNumbersPage : Page
    {
        TimeSpan remainingTime;
        DispatcherTimer? timer;
        public static bool quiz1 = false;

        const int QUIZ_LENGTH = 30;    //length of game in seconds
        const int TIER1_REWARD = 5;    //points reward for correct tier 1 choice
        const int TIER2_REWARD = 10;   //points reward for correct tier 2 choice
        const int PENALTY = 5;         //point penalty for any incorrect choice
  
        private Brush redBrush = new SolidColorBrush(Colors.Red);
        private Brush orangeBrush = new SolidColorBrush(Colors.Orange);
        private Brush yellowGreenBrush = new SolidColorBrush(Colors.Yellow);
        private Brush greenBrush = new SolidColorBrush(Colors.LimeGreen);

        public Tree<string> DeweyTree = MakeDeweyTree();
        public static List<string> OrderedOptions1 = new List<string>();    //Tier 1 quiz options
        public static List<string> OrderedOptions2 = new List<string>();    //Tier 2 quiz options

        public static string answer1  = string.Empty;    //Tier 1 answer
        public static string answer2  = string.Empty;    //Tier 2 answer
        public static string question = string.Empty;    //Tier 3 Description

        public static int tier1Score = 0;
        public static int tier2Score = 0;
        public static int totalpenalty = 0;
        public static int totalScore = 0;

        public FindingCallNumbersPage()
        {
            InitializeComponent();
            pbRemainingTime.Maximum = QUIZ_LENGTH;
            pbRemainingTime.Value = pbRemainingTime.Maximum;
            lblInfo.Text =
                $"You have {QUIZ_LENGTH} seconds to choose the correct category for the given description.\n\n" +
                $"Choosing the correct answer will reset the timer.\n\n" +
                $"Answering a tier 1 question correctly will progress you to a tier 2 question with the same description.\n\n" +
                $"- Correct Tier 1 answer: +{TIER1_REWARD} pts.\n" +
                $"- Correct Tier 2 answer: +{TIER2_REWARD} pts.\n" +
                $"- Incorrect answer: -{PENALTY} pts";
        }

        public static void SetNewQuiz(Tree<string> DeweyTree) //New question and sets of options (level 1 and 2)
        {
            //List of child nodes for each subsequent level in the tree are shuffled and the
            //first node in the shuffled list is picked as the answer.
            //This ensures a random selection of nodes linked in the tree at each level,
            //independent of the number of nodes at each level.
            TreeNode<string> root = DeweyTree.Root;

            List<TreeNode<string>> Hundreds = root.Children;
            ShuffleList(Hundreds);

            List<TreeNode<string>> Tens = Hundreds.First().Children;
            ShuffleList(Tens);

            List<TreeNode<string>> Units = Tens.First().Children;
            ShuffleList(Units);

            //Answer text set
            answer1 = Hundreds.First().Value.ToString();
            answer2 = Tens.First().Value.ToString();

            //Split question node's value into call number and description.
            //Only use the description for the question.
            string[] questionValue = Units.First().Value.Split(" ", 2);
            question = questionValue[1];

            //Tier 1 and 2 options set
            List<string> OptionsT1 = new();
            List<string> OptionsT2 = new();

            for (int i = 0; i < 4; i++)
            {
                try //in case there is an issue with the tree data e.g., not enough nodes at tier 1 or 2.
                {
                    OptionsT1.Add(Hundreds[i].Value);
                    OptionsT2.Add(Tens[i].Value);
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error Setting Quiz!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            OrderedOptions1 = OptionsT1.OrderBy(GetNumberPart).ToList();
            OrderedOptions2 = OptionsT2.OrderBy(GetNumberPart).ToList();
        }

        public static Tree<string> MakeDeweyTree() //Returns a tree with dewey data loaded from textfile
        {
            Tree<string> tree = new("Dewey Decimal Tree 000-999");
           
            //Textfile is stored in the same folder as the executable
            //Gets the executable directory
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //Path of the textfile
            string relativePath = "DeweyData.txt";
            //Combine the two paths and get the full path to the textfile
            string fullPath = Path.Combine(baseDirectory, relativePath);

            if (File.Exists(fullPath))
            {
                using StreamReader reader = new(fullPath);
                List<string> lines = new();
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    lines.Add(line!);
                }

                TreeNode<string> tier1 = new("");
                TreeNode<string> tier2 = new("");
                TreeNode<string> tier3 = new("");

                for (int i = 0; i < 1000; i++) //0-999
                {
                    //Tier 1 - children of root node
                    //Hundreds e.g. 000, 100, 800, etc.
                    if (i % 100 == 0)
                    {
                        if (ValidLine(lines[i]))
                        {
                            tier1 = new(lines[i]);
                            tree.Root.AddChild(tier1);
                        }
                    }
                    //Tier 2 - children of tier 1 nodes
                    //Tens e.g. 000, 110, 200, 210, etc. (also includes hundreds)
                    if (i % 10 == 0)
                    {
                        if (ValidLine(lines[i]))
                        {
                            tier2 = new(lines[i]);
                            tier1.AddChild(tier2);
                        }
                    }
                    //Tier 3 - children of tier 2 nodes
                    //Units e.g. 001, 034, 106, 712, etc. (not perfectly divisible by 10)
                    else
                    {
                        if (ValidLine(lines[i]))
                        {
                            tier3 = new(lines[i]);
                            tier2.AddChild(tier3);
                        }
                    }
                }
            }
            else
            {
                //If for some reason the directory to the textfile could not be accessed or is incorrect
                MessageBox.Show("Could not load data! Please ensure that the textfile directory is correct.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return tree;
        }

        public static bool ValidLine(string line) //Returns false on unused dewey categories
        {
            string[] lineParts = line.Split(' ', 2);
            if (lineParts[1] == "[Unassigned]")
            {
                return false;
            }
            else
            {
                return true;
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

        static int GetNumberPart(string input)
        {
            //https://stackoverflow.com/questions/59220804/extract-number-from-string-value

            string numericPart = new string(input.TakeWhile(char.IsDigit).ToArray());
            return int.Parse(numericPart);
        }

        private void EvaluateAnswer(string selectedAns) //Checks if the selected quiz answer is correct
        {
            string correctAns;

            if(quiz1){
                correctAns = answer1;
            }
            else
            {
                correctAns = answer2;
            }

            if(selectedAns == correctAns)
            {
                timer!.Stop();
                NewTimer();
                DisplaySuccess();
                if (quiz1)
                {
                    tier1Score += TIER1_REWARD;
                    StartQuiz2();
                }
                else
                {
                    tier2Score += TIER2_REWARD;
                    SetNewQuiz(DeweyTree);
                    StartQuiz1();
                }
                totalScore = tier1Score + tier2Score - totalpenalty;
            }
            else
            {
                DisplayFailure();
                SetNewQuiz(DeweyTree);
                StartQuiz1();
                totalpenalty += PENALTY;
                totalScore = tier1Score + tier2Score - totalpenalty;
            }
            UpdateScoreboard();
        }

        private void UpdateScoreboard()
        {
            lblScore.Content = $"Score: {totalScore}";
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {

            tier1Score = 0;
            tier2Score = 0;
            totalpenalty = 0;
            totalScore = 0;

            btnNewGame.IsEnabled = false;
            btnOpt1.IsEnabled = true;
            btnOpt2.IsEnabled = true;
            btnOpt3.IsEnabled = true;
            btnOpt4.IsEnabled = true;

            SetNewQuiz(DeweyTree);
            StartQuiz1();
            NewTimer();
        }

        private void StartQuiz1()
        {
            //The first level quiz
            quiz1 = true;

            //Init question and options for quiz 1
            lblQuestion.Content = question;
            btnOpt1.Content = OrderedOptions1[0];
            btnOpt2.Content = OrderedOptions1[1];
            btnOpt3.Content = OrderedOptions1[2];
            btnOpt4.Content = OrderedOptions1[3];
        }

        private void StartQuiz2()
        {
            //not the first level quiz
            quiz1 = false;

            //Init question and options for quiz 2
            lblQuestion.Content = question; //question should remain the same
            btnOpt1.Content = OrderedOptions2[0];
            btnOpt2.Content = OrderedOptions2[1];
            btnOpt3.Content = OrderedOptions2[2];
            btnOpt4.Content = OrderedOptions2[3];
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

        private void btnOpt1_Click(object sender, RoutedEventArgs e)
        {
            EvaluateAnswer(btnOpt1.Content.ToString()!);
        }

        private void btnOpt2_Click(object sender, RoutedEventArgs e)
        {
            EvaluateAnswer(btnOpt2.Content.ToString()!);
        }

        private void btnOpt3_Click(object sender, RoutedEventArgs e)
        {
            EvaluateAnswer(btnOpt3.Content.ToString()!);
        }

        private void btnOpt4_Click(object sender, RoutedEventArgs e)
        {
            EvaluateAnswer(btnOpt4.Content.ToString()!);
        }

        private void NewTimer() //Creates a new game timer
        {
            timer = new DispatcherTimer();
            timer.Stop();
            lblTime.Content = $"Time: {QUIZ_LENGTH}";
            remainingTime = TimeSpan.FromSeconds(QUIZ_LENGTH);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) //Defines when the game timer gets updated
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateGameTimer();
        }

        private void UpdateGameTimer()
        {
            pbRemainingTime.Value = remainingTime.TotalSeconds;
            lblTime.Content = $"Time: {remainingTime.TotalSeconds}";
            const double totalTime = QUIZ_LENGTH;
            switch (remainingTime.TotalSeconds) //Colour of the progress bar changed based on remaining time
            {
                case <= 0:
                    EndGame(); //The game is ended when the time remaining reaches 0
                    break;
                case <= totalTime/6:
                    pbRemainingTime.Foreground = redBrush;
                    break;
                case <= totalTime/3:
                    pbRemainingTime.Foreground = orangeBrush;
                    break;
                case <= totalTime * 2/3:
                    pbRemainingTime.Foreground = yellowGreenBrush;
                    break;
                case <= totalTime:
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

            if(totalpenalty > 0)
            {
                MessageBox.Show(
                $"Tier 1 Score: {tier1Score}\n" +
                $"Tier 2 Score: {tier2Score}\n" +
                $"Penalty: -{totalpenalty}\n" +
                $"Total Score:  {totalScore}\n", 
                "Time's up!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                $"Tier 1 Score: {tier1Score}\n" +
                $"Tier 2 Score: {tier2Score}\n" +
                $"Total Score:  {totalScore}\n", 
                "Time's up!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            

            SetOpeningState();
        }

        private void SetOpeningState()
        {
            btnNewGame.IsEnabled = true;
            btnOpt1.IsEnabled = false;
            btnOpt2.IsEnabled = false;
            btnOpt3.IsEnabled = false;
            btnOpt4.IsEnabled = false;

            lblQuestion.Content = "Click 'New Game' to Begin!";
            lblScore.Content = "Score: 0";
            lblTime.Content = "Time: 0";
            pbRemainingTime.Value = QUIZ_LENGTH;
            pbRemainingTime.Foreground = greenBrush;

            btnOpt1.Content = string.Empty;
            btnOpt2.Content = string.Empty;
            btnOpt3.Content = string.Empty;
            btnOpt4.Content = string.Empty;
        }

        private async void DisplaySuccess()
        {
            SolidColorBrush greenBrush = new SolidColorBrush(Colors.Lime);
            lblResult.Foreground = greenBrush;
            if (quiz1)
            {
                lblResult.Content = $"Correct {TIER1_REWARD}";
            }
            else
            {
                lblResult.Content = $"Correct {TIER2_REWARD}";
            }
            
            await Task.Delay(TimeSpan.FromSeconds(2));
            lblResult.Content = " ";
        }

        private async void DisplayFailure()
        {
            SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
            lblResult.Foreground = redBrush;
            lblResult.Content = $"Incorrect -{PENALTY}";   
            await Task.Delay(TimeSpan.FromSeconds(2));
            lblResult.Content = " ";
        }
    }
}