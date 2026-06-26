# PROG6221 Part 3: Cybersecurity Awareness Chatbot

## Student Details
**Name:** Thabiso Khumalo  
**Student Number:** ST10477675  
**Course:** PROG6221 - Programming 3B  

## Project Links
**Video Demonstration:** https://youtu.be/aedu3a2RuYA

## Project Overview
A WPF Desktop Application built in C# (.NET 8.0) that acts as a Cybersecurity Awareness Chatbot with database integration, task management, and interactive quiz features.

## Features Implemented
- ✅ SQLite database integration with Entity Framework Core
- ✅ Task management system (Create, Read, Update, Delete)
- ✅ Interactive 10-question cybersecurity quiz with real-time scoring
- ✅ Activity logging with timestamps
- ✅ Natural language processing (NLP) for intent detection
- ✅ Keyword recognition for cybersecurity topics
- ✅ Sentiment analysis
- ✅ Voice greeting on startup
- ✅ Personalized user responses

## Technology Stack
- **Language:** C#
- **Framework:** .NET 8.0 (WPF)
- **Database:** SQLite
- **ORM:** Entity Framework Core 8.0

## How to Run
1. Clone this repository
2. Open in Visual Studio 2022
3. Install NuGet packages:
4. Ensure `Greeting.wav` file is in the project root
5. Build and run (F5)

## Project Structure
- **Models/** - Database models (UserTask, ActivityLog)
- **Data/** - Entity Framework DbContext
- **Helpers/** - Business logic (TaskManager, ActivityLogger, QuizManager)
- **MainWindow.xaml** - User interface
- **MainWindow.xaml.cs** - Application logic

## Releases
- **v1.0** - Basic chatbot functionality
- **v2.0** - Keyword recognition and sentiment analysis
- **v3.0** - Complete project with all features
