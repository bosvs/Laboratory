using System.Text.RegularExpressions;

namespace WorkingWithXMLApplication.ParsingStrategy
{
    public static class ScheduleFilter
    {
        /// <summary>
        /// Розбиває вхідний рядок на слова та розділові знаки.
        /// </summary>
        private static string[] SplitIntoWords(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Array.Empty<string>();

            var regex = new Regex(@"[\w'-]+|[^\w\s]+");
            return regex.Matches(input).Select(m => m.Value).ToArray();
        }

        /// <summary>
        /// Перевіряє, чи є часткові збіги між значенням атрибуту та фільтром.
        /// </summary>
        private static bool IsSimilarParts(string? userFilter, string? attributesValue)
        {
            var filterParts = SplitIntoWords(userFilter?.ToLower());
            var attributeParts = SplitIntoWords(attributesValue?.ToLower());

            return filterParts.Any(filter =>
                attributeParts.Any(attribute => attribute.StartsWith(filter)));
        }

        /// <summary>
        /// Перевіряє, чи час відповідає вказаному інтервалу.
        /// </summary>
        private static bool MatchesTime(string? time, TimeOnly[] timeInterval)
        {
            if (string.IsNullOrWhiteSpace(time))
                return true;

            if (!TimeOnly.TryParse(time.Trim(), out var parsedTime))
                return false;

            return timeInterval[0] <= parsedTime && timeInterval[1] >= parsedTime;
        }

        /// <summary>
        /// Відфільтровує розклад за заданими критеріями.
        /// </summary>
        public static Schedule FilterSchedule(Schedule schedule,
            string? StudentName = null,
            string? time = null,
            string? EventTitle = null,
            string? room = null,
            string? day = null)
        {
            // Якщо критерії не задані, повертаємо початковий розклад
            if (new[] { StudentName, time, EventTitle, room, day }.All(string.IsNullOrWhiteSpace))
            {
                return schedule;
            }

            var filteredEvents = schedule.Events.Where(Event =>
                (string.IsNullOrWhiteSpace(StudentName) || IsSimilarParts(StudentName, Event.Student.FullName)) &&
                MatchesTime(time, Event.TimeInterval) &&
                (string.IsNullOrWhiteSpace(EventTitle) || IsSimilarParts(EventTitle, Event.Title)) &&
                (string.IsNullOrWhiteSpace(room) || IsSimilarParts(room, Event.Room)) &&
                (string.IsNullOrWhiteSpace(day) || IsSimilarParts(day, Event.Day))
            ).ToList();

            return new Schedule { Events = filteredEvents };
        }
    }
}
