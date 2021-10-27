using System;


namespace ConsoleClient.Commands
{
    public class TimeCommand : Command
    {
        private DateTime _startDate;
        private int _currentTime = -1;

        private bool _isActive;        


        public TimeCommand()
        {
            Init();

            Title = "timer";
            CommandType = Primitives.Enums.CommandType.List;

            LoadChildCommands();
        }


        private void LoadChildCommands()
        {
            ChildCommand.Add(new Command("start", StartTimeEvent));
            ChildCommand.Add(new Command("cont", ContinueTimeEvent));
            ChildCommand.Add(new Command("pause", PauseTimeEvent));
            ChildCommand.Add(new Command("end", EndTimeEvent));
            ChildCommand.Add(new Command("last", LastTimeEvent));
        }


        private void LastTimeEvent()
        {
            if (_currentTime == -1)
            {
                Console.WriteLine($"Last error: already do firts start");
                return;
            }

            var time = _currentTime + (int)(DateTime.Now - _startDate).TotalMinutes;

            Console.WriteLine($"last time: {GetDecimalCurrentTime(time)}");
        }


        private void StartTimeEvent()
        {
            if (_isActive)
            {
                Console.WriteLine($"Start error: already start in: {_startDate}");
                return;
            }
            _currentTime = 0;
            _isActive = true;

            UpdateStartTime();

            Console.WriteLine($"start time: {DateTime.Now}");
        }


        private void ContinueTimeEvent()
        {
            if (_isActive)
            {
                Console.WriteLine($"Continue error: timer must be NOT active. Start in {_startDate}, current time {GetDecimalCurrentTime()}");
                return;
            }
            _currentTime = 0;
            _isActive = true;

            UpdateStartTime();

            Console.WriteLine($"start time: {DateTime.Now}");
        }


        private void PauseTimeEvent()
        {
            if (!_isActive)
            {
                Console.WriteLine($"Pause error: timer must be active");
                return;
            }

            UpdateCurrentTime();
            _isActive = false;
            Console.WriteLine($"pause time: {DateTime.Now}");
        }


        private void EndTimeEvent()
        {
            string time = GetDecimalCurrentTime();

            if (!_isActive)
            {
                Console.WriteLine($"End error: timer must be active");
                Console.WriteLine($"Last time: {time}");
            }

            UpdateCurrentTime();
            _isActive = false;

            Console.WriteLine($"end time: {DateTime.Now}");
            Console.WriteLine($"All time in decimal hour: {time}");
        }


        private string GetDecimalCurrentTime()
        {
            return GetDecimalCurrentTime(_currentTime);
        }


        private string GetDecimalCurrentTime(int minutes)
        {
            return Math.Round(minutes / 60.0, 2).ToString() + " min";
        }


        private void UpdateStartTime()
        {
            _startDate = DateTime.Now;
        }


        private void UpdateCurrentTime()
        {
            _currentTime += (int)(DateTime.Now - _startDate).TotalMinutes;
        }
    }
}
