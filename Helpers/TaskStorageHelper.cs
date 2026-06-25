using System.Collections.Generic;
using System.Linq;
using Prog_6221_Part3_PoE.Data;
using Prog_6221_Part3_PoE.Models;

namespace Prog_6221_Part3_PoE.Helpers
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public List<UserTask> LoadTasks()
        {
            return db.Tasks.ToList();
        }

        public void AddTask(string title, string description, string reminder)
        {
            UserTask newTask = new UserTask
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            db.Tasks.Add(newTask);
            db.SaveChanges();
        }

        public void MarkAsComplete(int id)
        {
            var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                task.IsComplete = true;
                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }

        public void DeleteTask(int id)
        {
            var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
            }
        }
    }
}
