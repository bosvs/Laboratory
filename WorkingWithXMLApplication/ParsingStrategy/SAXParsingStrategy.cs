using System.Xml;

namespace WorkingWithXMLApplication.ParsingStrategy
{
    public class SAXParsingStrategy : IParsingStrategy
    {
        public Schedule Parse(string selectedFilePath)
        {
            var schedule = new Schedule { Events = new List<Event>() };

            Event? currentEvent = null;
            Student? currentStudent = null;
            //Student? currentStudent = null;

            using var reader = XmlReader.Create(selectedFilePath);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        HandleStartElement(reader, ref currentEvent, ref currentStudent);
                        break;

                    case XmlNodeType.EndElement:
                        HandleEndElement(reader, ref schedule, ref currentEvent, ref currentStudent);
                        break;
                }
            }

            return schedule;
        }

        private static void HandleStartElement(XmlReader reader, ref Event? currentEvent, ref Student? currentStudent)
        {
            switch (reader.Name)
            {
                case "Event":
                    currentEvent = new Event
                    {
                        //Students = new List<Student>(),
                        Day = reader.GetAttribute("Day"),
                        Title = reader.GetAttribute("Title"),
                        Room = reader.GetAttribute("Room"),
                        ScheduleTime = reader.GetAttribute("ScheduleTime")
                    };
                    break;

                case "Student":
                    currentStudent = new Student
                    {
                        Faculty = reader.GetAttribute("Faculty"),
                        Department = reader.GetAttribute("Department")
                    };
                    break;

                case "FullName":
                    reader.Read(); // Перехід до текстового вузла
                    var fullName = reader.Value.Trim();
                    if (currentStudent != null)
                        currentStudent.FullName = fullName;
                    else if (currentStudent != null)
                        currentStudent.FullName = fullName;
                    break;

   
            }
        }

        private static void HandleEndElement(XmlReader reader, ref Schedule schedule, ref Event? currentEvent, ref Student? currentStudent)
        {
            switch (reader.Name)
            {
                case "Student":
                    if (currentEvent != null && currentStudent != null)
                        currentEvent.Student = currentStudent;
                    currentStudent = null;
                    break;

                case "Event":
                    if (currentEvent != null)
                        schedule.Events.Add(currentEvent);
                    currentEvent = null;
                    break;
            }
        }
    }
}
