using System;


namespace ConsoleClient.Commands
{
    public class TimeCommand : Command
    {
        private DateTime _startDate;
        private int _currentTime;


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
            ChildCommand.Add(new Command("cont", StartTimeEvent));
            ChildCommand.Add(new Command("pause", PauseTimeEvent));
            ChildCommand.Add(new Command("end", EndTimeEvent));
        }


        private void StartTimeEvent()
        {
            _currentTime = 0;

            UpdateStartTime();

            Console.WriteLine($"start time: {DateTime.Now}");
        }


        private void PauseTimeEvent()
        {
            UpdateCurrentTime();
            Console.WriteLine($"pause time: {DateTime.Now}");
        }


        private void EndTimeEvent()
        {
            UpdateCurrentTime();
            Console.WriteLine($"end time: {DateTime.Now}");
            Console.WriteLine($"All time in decimal hour: {Math.Round(_currentTime / 60.0, 2)}");
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
