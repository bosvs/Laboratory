using System.Data;

namespace WorkingWithXMLApplication{
    public class Schedule{
        public List<Event> Events { get; set; }
    }

    public class Event{

        private TimeOnly[] _timeInterval;
        private string _scheduleTime;
        public string Day { get; set; }
        public string Title { get; set; }
        public Student Student { get; set; }
        public string Room { get; set; }
        //public List<Student> Students { get; set; }
        public string ScheduleTime
        {
            get => _scheduleTime;
            set
            {
                _scheduleTime = value;
                _timeInterval = ParseTimeInterval(value);
            }
        }
        public TimeOnly[] TimeInterval => _timeInterval;
        private TimeOnly[] ParseTimeInterval(string timeInterval)
        {
            string[] times = timeInterval.Split(" - ");
            TimeOnly startTime = TimeOnly.Parse(times[0].Trim());
            TimeOnly endTime = TimeOnly.Parse(times[1].Trim());
            return new TimeOnly[] { startTime, endTime };
        }
        
    }

    public class Student
    {
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
    }
}
