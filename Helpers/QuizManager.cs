using System.Collections.Generic;

namespace Prog_6221_Part3_PoE.Helpers
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        // Public properties so the UI can read them easily
        public int CurrentScore { get { return _score; } }
        public int QuestionsAnswered { get { return _currentIndex; } }
        public int TotalQuestions { get { return _questions.Count; } }

        public QuizManager() { _questions = new List<QuizQuestion>(); LoadQuestions(); }

        private void LoadQuestions()
        {
            _questions.Add(new QuizQuestion { Question = "What should you do if you receive an email asking for your password?", Options = new List<string> { "Reply", "Delete", "Report as phishing" }, CorrectAnswer = "Report as phishing", Explanation = "Correct!", IsTrueFalse = false });
            _questions.Add(new QuizQuestion { Question = "True or False: You should use the same password for all accounts.", Options = new List<string> { "True", "False" }, CorrectAnswer = "False", Explanation = "Correct! Use unique passwords.", IsTrueFalse = true });
            _questions.Add(new QuizQuestion { Question = "What does HTTPS indicate?", Options = new List<string> { "Government site", "Encrypted connection", "Free to use" }, CorrectAnswer = "Encrypted connection", Explanation = "Correct!", IsTrueFalse = false });
            _questions.Add(new QuizQuestion { Question = "True or False: Public Wi-Fi is always safe for banking.", Options = new List<string> { "True", "False" }, CorrectAnswer = "False", Explanation = "Correct! It is often unsecured.", IsTrueFalse = true });
            _questions.Add(new QuizQuestion { Question = "What is Two-Factor Authentication (2FA)?", Options = new List<string> { "Two passwords", "Password + second verification", "Logging in twice" }, CorrectAnswer = "Password + second verification", Explanation = "Correct!", IsTrueFalse = false });
            _questions.Add(new QuizQuestion { Question = "True or False: Ransomware locks your files until you pay.", Options = new List<string> { "True", "False" }, CorrectAnswer = "True", Explanation = "Correct!", IsTrueFalse = true });
            _questions.Add(new QuizQuestion { Question = "Which is an example of social engineering?", Options = new List<string> { "Virus", "Hacker pretending to be IT support", "Firewall" }, CorrectAnswer = "Hacker pretending to be IT support", Explanation = "Correct!", IsTrueFalse = false });
            _questions.Add(new QuizQuestion { Question = "True or False: You should regularly back up your data.", Options = new List<string> { "True", "False" }, CorrectAnswer = "True", Explanation = "Correct!", IsTrueFalse = true });
            _questions.Add(new QuizQuestion { Question = "Best way to create a strong password?", Options = new List<string> { "Name and birthdate", "Long mix of letters, numbers, symbols", "password123" }, CorrectAnswer = "Long mix of letters, numbers, symbols", Explanation = "Correct!", IsTrueFalse = false });
            _questions.Add(new QuizQuestion { Question = "True or False: Click links in unexpected texts.", Options = new List<string> { "True", "False" }, CorrectAnswer = "False", Explanation = "Correct! Never click unknown links.", IsTrueFalse = true });
        }

        public QuizQuestion GetCurrentQuestion() { return _currentIndex < _questions.Count ? _questions[_currentIndex] : null; }

        public bool SubmitAnswer(string answer)
        {
            bool isCorrect = answer == _questions[_currentIndex].CorrectAnswer;
            if (isCorrect) _score++;
            _currentIndex++;
            return isCorrect;
        }

        public bool IsFinished() { return _currentIndex >= _questions.Count; }
        public string GetFinalScore() { return $"You scored {_score} out of {_questions.Count}."; }
        public string GetFinalMessage()
        {
            int percentage = (_questions.Count > 0) ? (_score * 100 / _questions.Count) : 0;
            if (percentage >= 90) return "Excellent work! You're a cybersecurity expert!";
            if (percentage >= 70) return "Great job! You have good security knowledge!";
            if (percentage >= 50) return "Good effort! Keep learning to improve!";
            return "Keep studying! Cybersecurity is important!";
        }

        public void ResetQuiz() { _currentIndex = 0; _score = 0; }
    }
}
