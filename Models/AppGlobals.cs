
namespace Timetablez.Models
{
    public class AppGlobals
    {

        // Login and credentials
        public static string dbLocal = string.Empty;
        public static string currentUser = string.Empty;

        public static bool tempSchedule = false;
        public static bool tempMade = false;


        // Timetable behaviour
        public static int TotalPeriods { get; set; } = 4;
        public static int TotalWeekDays { get; set; } = 5;

        public static string lessonView = "LessonView";

        public static string lessonTable = "WeeklySchedule";

        // Data Grid Themes
        public static Dictionary<string, GridTheme> themes = new Dictionary<string, GridTheme>(){
            {
                "Data",
                new GridTheme
                {
                    HeaderBackColor = Color.LightSlateGray,
                    HeaderForeColor = Color.WhiteSmoke,
                    HeaderSelectionBackColor = Color.LightSlateGray,
                    HeaderSelectionForeColor = Color.WhiteSmoke,

                    RowBackColor = Color.WhiteSmoke,
                    AlternatingRowBackColor = Color.Lavender,
                    SelectionBackColor = Color.LightSteelBlue,
                }
            },
            {
                "Validations",
                new GridTheme
                {
                    HeaderBackColor = Color.FromArgb(214, 150, 111),
                    HeaderForeColor = Color.White,
                    HeaderSelectionBackColor = Color.FromArgb(214, 150, 111),
                    HeaderSelectionForeColor = Color.White,

                    RowBackColor = Color.LightGray,
                    AlternatingRowBackColor = Color.FromArgb(194, 187, 178),
                    SelectionBackColor = Color.FromArgb(191, 155, 109),
                }
            },
            {
                "Details",
                new GridTheme
                {
                    HeaderBackColor = Color.FromArgb(121, 145, 126),
                    HeaderForeColor = Color.WhiteSmoke,
                    HeaderSelectionBackColor = Color.FromArgb(121, 145, 126),
                    HeaderSelectionForeColor = Color.WhiteSmoke,

                    RowBackColor = Color.WhiteSmoke,
                    AlternatingRowBackColor = Color.FromArgb(199, 207, 196),
                    SelectionBackColor = Color.FromArgb(170, 176, 2)
                }
            }
        };

    }

}
