using System.Collections.Generic;
using Prog_6221_Part3_PoE.Models;

namespace Prog_6221_Part3_PoE.Helpers
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager() { _storage = new TaskStorageHelper(); _logger = new ActivityLogger(); }

        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);
            _logger.Log($"Task added: '{title}'");
            return $"Task added: '{title}'. Would you like to set a reminder?";
        }

        public List<UserTask> GetAllTasks() { return _storage.LoadTasks(); }

        public void MarkAsComplete(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null) { _storage.MarkAsComplete(id); _logger.Log($"Task marked complete: '{task.Title}'"); }
        }

        public void DeleteTask(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null) { _logger.Log($"Task deleted: '{task.Title}'"); _storage.DeleteTask(id); }
        }
    }
}
