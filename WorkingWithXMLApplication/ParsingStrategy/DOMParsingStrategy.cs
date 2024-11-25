using System.Xml;

namespace WorkingWithXMLApplication.ParsingStrategy
{
    public class DOMParsingStrategy : IParsingStrategy
    {
        public Schedule Parse(string selectedFilePath)
        {
            var schedule = new Schedule { Events = new List<Event>() };
            
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(selectedFilePath);

            XmlNodeList EventNodes = xmlDoc.GetElementsByTagName("Event");
            foreach (XmlNode EventNode in EventNodes)
            {
                var Event = ParseEventNode(EventNode);
                if (Event != null)
                    schedule.Events.Add(Event);
            }

            return schedule;
        }

        private static Event? ParseEventNode(XmlNode EventNode)
        {
            if (EventNode.Attributes == null) return null;

            var Event = new Event
            {
                Difficulty = EventNode.Attributes["Difficulty"]?.Value.Trim(),
                Day = EventNode.Attributes["Day"]?.Value.Trim(),
                Title = EventNode.Attributes["Title"]?.Value.Trim(),
                Room = EventNode.Attributes["Room"]?.Value.Trim(),
                ScheduleTime = EventNode.Attributes["ScheduleTime"]?.Value.Trim()
            };

            foreach (XmlNode childNode in EventNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Student":
                        Event.Student = ParseStudentNode(childNode);
                        break;
                }
            }

            return Event;
        }

        private static Student? ParseStudentNode(XmlNode StudentNode)
        {
            if (StudentNode.Attributes == null) return null;

            var Student = new Student
            {
                Faculty = StudentNode.Attributes["Faculty"]?.Value.Trim(),
                Department = StudentNode.Attributes["Department"]?.Value.Trim()
            };

            foreach (XmlNode detailNode in StudentNode.ChildNodes)
            {
                if (detailNode.Name == "FullName")
                    Student.FullName = detailNode.InnerText.Trim();
            }

            return Student;
        }

        /*rivate static void ParseStudentsNode(XmlNode studentsNode, List<Student> students)
        {
            foreach (XmlNode studentNode in studentsNode.ChildNodes)
            {
                if (studentNode.Name != "Student" || studentNode.Attributes == null) continue;

                var student = new Student
                {
                    Group = studentNode.Attributes["Group"]?.Value.Trim()
                };

                foreach (XmlNode detailNode in studentNode.ChildNodes)
                {
                    if (detailNode.Name == "FullName")
                        student.FullName = detailNode.InnerText.Trim();
                }

                if (!string.IsNullOrWhiteSpace(student.FullName) && !string.IsNullOrWhiteSpace(student.Group))
                {
                    students.Add(student);
                }
            }
        }*/
    }
}
