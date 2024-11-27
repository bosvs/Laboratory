using System.Xml.Linq;

namespace WorkingWithXMLApplication.ParsingStrategy
{
    public class LINQParsingStrategy : IParsingStrategy
    {
        public Schedule Parse(string selectedFilePath)
        {
            XDocument xml = XDocument.Load(selectedFilePath);
            var schedule = new Schedule { Events = new List<Event>() };

            foreach (var EventElement in xml.Descendants("Event"))
            {
                var Event = new Event
                {
                    Day = (string)EventElement.Attribute("Day"),
                    Title = (string)EventElement.Attribute("Title"),
                    Room = (string)EventElement.Attribute("Room"),
                    ScheduleTime = (string)EventElement.Attribute("ScheduleTime"),
                    Student = new Student
                    {
                        FullName = (string)EventElement.Element("Student")?.Element("FullName"),
                        Faculty = (string)EventElement.Element("Student")?.Attribute("Faculty"),
                        Department = (string)EventElement.Element("Student")?.Attribute("Department")
                    },
                    
                };
                schedule.Events.Add(Event);
            }

            return schedule;
        }
    }
}
