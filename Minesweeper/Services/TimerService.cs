using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Minesweeper.Services
{
    public class TimerService
    {
        public string Time { get; set; }
        private TimeSpan timeSpan;
        public DispatcherTimer DispatcherTimer { get; set; }
        public TimerService()
        {
            DispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
        }

        public string Tick()
        {
            timeSpan += TimeSpan.FromSeconds(1);
            if (timeSpan < TimeSpan.FromHours(1))
            {
                Time = timeSpan.ToString(@"mm\:ss");
            }
            else
            {
                Time = timeSpan.ToString(@"hh\:mm\:ss");
            }
            return Time;
        }

        public void ResetTimer()
        {
            Time = "00:00";
            timeSpan = TimeSpan.FromSeconds(0);
            StopTimer();
        }

        public void StartTimer()
        {
            DispatcherTimer.Start();
        }

        public void StopTimer()
        {
            DispatcherTimer.Stop();
        }
    }
}
