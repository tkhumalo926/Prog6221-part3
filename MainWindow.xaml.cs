using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Prog_6221_Part3_PoE.Helpers;
using Prog_6221_Part3_PoE.Models;

namespace Prog_6221_Part3_PoE
{
    public partial class MainWindow : Window
    {
        private string userName = "";
        private string currentTopic = "";
        private bool waitingForName = true;

        private TaskManager _taskManager = new TaskManager();
        private ActivityLogger _activityLogger = new ActivityLogger();
        private QuizManager _quizManager = new QuizManager();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            try
            {
                string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string soundPath = System.IO.Path.Combine(basePath, "Greeting.wav");
                
                if (System.IO.File.Exists(soundPath))
                {
                    SoundPlayer sound = new SoundPlayer(soundPath);
                    sound.PlaySync();
                }
            }
            catch { }

            AddBotMessage("Hello! I am your Cybersecurity Awareness Bot.");
            AddBotMessage("Please enter your name before we continue.");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
        }

        private string ProcessInput(string userInput)
        {
            string input = userInput.ToLower();

            if (input.Contains("add task") || input.Contains("create task") || input.Contains("i need to"))
            {
                string taskName = ExtractTaskName(input);
                if (!string.IsNullOrEmpty(taskName))
                {
                    try {
                        _taskManager.AddTask(taskName, "Added via chat", "none");
                        _activityLogger.Log("Task created: " + taskName);
                        RefreshTaskList(); 
                        return "Great! I have added '" + taskName + "' to your task list. Would you like to set a reminder?";
                    } catch (Exception ex) {
                        return "Sorry, I encountered an error: " + ex.Message;
                    }
                }
            }

            if (input.Contains("remind me") || input.Contains("reminder"))
            {
                _activityLogger.Log("Reminder set: " + userInput);
                return "Perfect! I will remind you: '" + userInput + "'";
            }

            if (input.Contains("start quiz") || input.Contains("quiz me"))
            {
                _activityLogger.Log("Quiz started");
                _quizManager.ResetQuiz();
                LoadNextQuestion();
                if (MainTabControl != null) MainTabControl.SelectedIndex = 2;
                return "Quiz mode activated! Good luck!";
            }

            if (input.Contains("show activity log") || input.Contains("what have you done"))
            {
                string log = _activityLogger.GetRecentLog(10);
                if (_activityLogger.GetCount() > 10) log += "\n\nType 'show more' for complete history";
                return log;
            }

            if (input.Contains("show more")) return _activityLogger.GetFullLog();

            return null;
        }

        private string ExtractTaskName(string input)
        {
            if (input.Contains("add task")) return input.Substring(input.IndexOf("add task") + 9).Trim();
            if (input.Contains("create task")) return input.Substring(input.IndexOf("create task") + 12).Trim();
            if (input.Contains("i need to")) return input.Substring(input.IndexOf("i need to") + 10).Trim();
            return input;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) { SendMessage(); }
        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) SendMessage(); }

        private void SendMessage()
        {
            string userInput = UserInputTextBox.Text.Trim();
            if (userInput == "") return;

            AddUserMessage(userInput);
            UserInputTextBox.Clear();

            if (waitingForName)
            {
                userName = userInput;
                waitingForName = false;
                _activityLogger.Log("User " + userName + " joined");
                AddBotMessage("Welcome aboard, " + userName + ". I am here to help you master cybersecurity. Ask me about passwords, phishing, malware, or privacy!");
                return;
            }

            string nlpResponse = ProcessInput(userInput);
            if (nlpResponse != null) { AddBotMessage(nlpResponse); return; }

            string response = GetBotResponse(userInput.ToLower());
            _activityLogger.Log("Query: " + currentTopic);
            AddBotMessage(response);
        }

        private string GetBotResponse(string input)
        {
            if (input.Contains("password")) { currentTopic = "password"; return "Password Security: Create strong passwords with 12+ characters, mix upper/lower case, numbers, and symbols. Never reuse passwords across accounts!"; }
            if (input.Contains("phishing")) { currentTopic = "phishing"; return "Phishing Alert: Always verify sender addresses, hover over links before clicking, and never share credentials via email."; }
            if (input.Contains("malware") || input.Contains("virus")) { currentTopic = "malware"; return "Malware Protection: Keep antivirus updated, avoid suspicious downloads, and never open attachments from unknown senders."; }
            if (input.Contains("privacy")) { currentTopic = "privacy"; return "Privacy First: Review app permissions, use privacy-focused browsers, and think twice before sharing personal info online."; }
            if (input.Contains("scam")) { currentTopic = "scam"; return "Scam Detection: Legitimate organizations never demand immediate payment via gift cards. Verify independently before acting."; }
            if (input.Contains("safe browsing")) { currentTopic = "safe browsing"; return "Safe Browsing: Look for HTTPS padlock, use ad blockers, keep browser updated, and avoid clicking pop-ups."; }
            if (input.Contains("two-factor") || input.Contains("2fa")) { currentTopic = "2FA"; return "2FA Power: Enable 2FA everywhere! Use authenticator apps instead of SMS when possible for maximum security."; }
            if (input.Contains("social engineering")) { currentTopic = "social engineering"; return "Social Engineering: Attackers exploit trust and urgency. Always verify identity through official channels."; }
            if (input.Contains("why") || input.Contains("explain") || input.Contains("more")) { return GetFollowUpResponse(); }
            if (input.Contains("hello") || input.Contains("hi")) { return "Hey " + userName + "! Ready to level up your security knowledge?"; }
            if (input.Contains("bye")) { return "Stay safe online, " + userName + "!"; }
            return "I am not sure about that yet. Try asking about passwords, phishing, malware, privacy, scams, safe browsing, 2FA, or social engineering!";
        }

        private string GetFollowUpResponse()
        {
            if (currentTopic == "password") return "Pro Tip: Use a password manager to generate and store unique passwords for every account!";
            if (currentTopic == "phishing") return "Pro Tip: Enable email filtering and report phishing attempts to help protect others!";
            if (currentTopic == "malware") return "Pro Tip: Run weekly full system scans and keep all software patched and updated!";
            if (currentTopic == "privacy") return "Pro Tip: Use privacy-focused search engines and consider a VPN for sensitive browsing!";
            if (currentTopic == "scam") return "Pro Tip: Register with Do Not Call lists and use call blocking apps to reduce scam calls!";
            if (currentTopic == "safe browsing") return "Pro Tip: Use browser extensions like HTTPS Everywhere for enhanced protection!";
            if (currentTopic == "2FA") return "Pro Tip: Keep backup codes in a secure location in case you lose access to your 2FA device!";
            if (currentTopic == "social engineering") return "Pro Tip: Train yourself to pause and verify before acting on any urgent request!";
            return "Would you like to know more about a specific security topic?";
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (string.IsNullOrWhiteSpace(TxtTaskTitle.Text) || TxtTaskTitle.Text == "Task Title") return;
                _taskManager.AddTask(TxtTaskTitle.Text, TxtTaskDesc.Text == "Description" ? "" : TxtTaskDesc.Text, 
                                    TxtTaskReminder.Text == "Reminder (optional)" ? "none" : TxtTaskReminder.Text);
                _activityLogger.Log("Task created via UI: " + TxtTaskTitle.Text);
                RefreshTaskList();
                TxtTaskTitle.Text = "Task Title"; TxtTaskDesc.Text = "Description"; TxtTaskReminder.Text = "Reminder (optional)";
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "Task Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (TaskListBox.SelectedIndex == -1) return;
                var tasks = _taskManager.GetAllTasks();
                int selectedId = tasks[TaskListBox.SelectedIndex].Id;
                _taskManager.MarkAsComplete(selectedId);
                _activityLogger.Log("Task completed: " + tasks[TaskListBox.SelectedIndex].Title);
                RefreshTaskList();
            } catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (TaskListBox.SelectedIndex == -1) return;
                var tasks = _taskManager.GetAllTasks();
                int selectedId = tasks[TaskListBox.SelectedIndex].Id;
                _taskManager.DeleteTask(selectedId);
                _activityLogger.Log("Task deleted: " + tasks[TaskListBox.SelectedIndex].Title);
                RefreshTaskList();
            } catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void RefreshTaskList()
        {
            try {
                TaskListBox.Items.Clear();
                var tasks = _taskManager.GetAllTasks();
                if (tasks == null) return;
                
                foreach (var task in tasks)
                {
                    string status = task.IsComplete ? "[COMPLETED]" : "[PENDING]";
                    TaskListBox.Items.Add(status + " | " + task.Title);
                }
            } catch (Exception ex) { MessageBox.Show("Error loading tasks: " + ex.Message); }
        }

        private void BtnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.ResetQuiz();
            _activityLogger.Log("Quiz started");
            UpdateScoreDisplay();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            var question = _quizManager.GetCurrentQuestion();
            if (question == null) return;
            TxtQuizQuestion.Text = "Question " + (_quizManager.QuestionsAnswered + 1) + "/10: " + question.Question;
            TxtQuizFeedback.Text = "";
            QuizOptionsPanel.Children.Clear();
            
            foreach (var option in question.Options)
            {
                RadioButton rb = new RadioButton();
                rb.Content = option;
                rb.Foreground = Brushes.White;
                rb.Margin = new Thickness(0, 10, 0, 10);
                rb.FontSize = 14;
                rb.Tag = option;
                rb.GroupName = "QuizOptions";
                QuizOptionsPanel.Children.Add(rb);
            }
            BtnSubmitAnswer.IsEnabled = true;
            BtnNextQuestion.IsEnabled = false;
            UpdateScoreDisplay();
        }

        private void BtnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            string selectedAnswer = null;
            foreach (RadioButton rb in QuizOptionsPanel.Children)
            {
                if (rb.IsChecked == true) { selectedAnswer = rb.Tag.ToString(); break; }
            }
            if (selectedAnswer == null) return;
            
            bool isCorrect = _quizManager.SubmitAnswer(selectedAnswer);
            
            if (isCorrect) {
                TxtQuizFeedback.Text = "Correct! Excellent work!";
                TxtQuizFeedback.Foreground = new SolidColorBrush(Colors.LightGreen);
            } else {
                TxtQuizFeedback.Text = "Not quite right. Keep learning!";
                TxtQuizFeedback.Foreground = new SolidColorBrush(Colors.Orange);
            }
            
            UpdateScoreDisplay();
            BtnSubmitAnswer.IsEnabled = false;
            
            if (_quizManager.IsFinished())
            {
                TxtQuizQuestion.Text = "Quiz Complete!";
                TxtQuizFeedback.Text = _quizManager.GetFinalScore() + "\n\n" + _quizManager.GetFinalMessage();
                _activityLogger.Log("Quiz finished: " + _quizManager.GetFinalScore());
                QuizOptionsPanel.Children.Clear();
            }
            else { BtnNextQuestion.IsEnabled = true; }
        }

        private void UpdateScoreDisplay()
        {
            TxtQuizScore.Text = "Score: " + _quizManager.CurrentScore + " / " + _quizManager.TotalQuestions;
        }

        private void BtnNextQuestion_Click(object sender, RoutedEventArgs e) { LoadNextQuestion(); }

        private void BtnShowMoreLog_Click(object sender, RoutedEventArgs e)
        {
            TxtActivityLog.Text = _activityLogger.GetFullLog();
            BtnShowMoreLog.Visibility = Visibility.Collapsed;
        }

        private void AddUserMessage(string message) { AddMessage(userName == "" ? "U" : userName.Substring(0, 1).ToUpper(), message, true); }
        private void AddBotMessage(string message) { AddMessage("AI", message, false); }

        private void AddMessage(string avatarText, string message, bool isUser)
        {
            StackPanel messageRow = new StackPanel();
            messageRow.Orientation = Orientation.Horizontal;
            messageRow.Margin = new Thickness(0, 10, 0, 10);

            Border avatar = new Border();
            avatar.Width = 40; avatar.Height = 40;
            avatar.CornerRadius = new CornerRadius(20);
            avatar.Background = isUser ? new SolidColorBrush(Color.FromRgb(255, 165, 0)) : new SolidColorBrush(Color.FromRgb(0, 217, 255));
            
            TextBlock avatarTextBlock = new TextBlock();
            avatarTextBlock.Text = avatarText;
            avatarTextBlock.Foreground = Brushes.Black;
            avatarTextBlock.FontWeight = FontWeights.Bold;
            avatarTextBlock.FontSize = 16;
            avatarTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            avatarTextBlock.VerticalAlignment = VerticalAlignment.Center;
            avatar.Child = avatarTextBlock;

            Border bubble = new Border();
            bubble.Background = isUser ? new SolidColorBrush(Color.FromRgb(30, 37, 71)) : new SolidColorBrush(Color.FromRgb(21, 27, 61));
            bubble.CornerRadius = new CornerRadius(15);
            bubble.Padding = new Thickness(15, 10, 15, 10);
            bubble.MaxWidth = 400;

            TextBlock messageText = new TextBlock();
            messageText.Text = message;
            messageText.Foreground = Brushes.White;
            messageText.FontSize = 14;
            messageText.TextWrapping = TextWrapping.Wrap;
            bubble.Child = messageText;

            if (isUser) { 
                messageRow.Children.Add(bubble);
                messageRow.Children.Add(avatar);
                messageRow.HorizontalAlignment = HorizontalAlignment.Right;
            } else { 
                messageRow.Children.Add(avatar);
                messageRow.Children.Add(bubble);
                messageRow.HorizontalAlignment = HorizontalAlignment.Left;
            }

            ChatPanel.Children.Add(messageRow);
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
