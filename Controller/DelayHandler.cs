using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace EksamenOpgave.Controller
{
    public class DelayHandler
    {
        private Timer timer;
        private bool _running = false;
        public event EventHandler<ElapsedEventArgs> OnTimerElapsed;
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("I was called!");
            OnTimerElapsed.Invoke(source, e);
            _running = false;
        }
        public void InvokeDelay(int delay)
        {
            if (delay <= 0) { return; }
            timer = new(delay);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
            timer.AutoReset = false;
            _running = true;
        }
        public void CancelTimer()
        {
            timer.Stop();
            timer.Enabled = false;
            _running = false;
        }
        public bool IsRunning { get => _running; }
        public bool Wait { get {
                while (IsRunning)
                {
                    Console.ReadKey();
                    CancelTimer();
                }
                return IsRunning;
            } }
        
    }
}
