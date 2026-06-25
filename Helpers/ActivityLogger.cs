using System;
using System.Collections.Generic;
using System.Linq;
using Prog_6221_Part3_PoE.Data;
using Prog_6221_Part3_PoE.Models;

namespace Prog_6221_Part3_PoE.Helpers
{
    public class ActivityLogger
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public void Log(string action)
        {
            ActivityLog log = new ActivityLog { Description = action, CreatedAt = DateTime.Now.ToString("[HH:mm]") };
            db.Logs.Add(log);
            db.SaveChanges();
        }

        public string GetRecentLog(int count = 10)
        {
            var logs = db.Logs.ToList();
            var recentLogs = logs.TakeLast(count).ToList();
            string result = "Recent actions:\n";
            for (int i = 0; i < recentLogs.Count; i++) { result += $"{i + 1}. {recentLogs[i].Description}\n"; }
            return result;
        }

        public string GetFullLog()
        {
            var logs = db.Logs.ToList();
            string result = "Full activity log:\n";
            for (int i = 0; i < logs.Count; i++) { result += $"{i + 1}. {logs[i].Description}\n"; }
            return result;
        }

        public int GetCount() { return db.Logs.Count(); }
    }
}
