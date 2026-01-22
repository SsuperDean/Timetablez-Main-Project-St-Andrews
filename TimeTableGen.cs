using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Timetablez.Classes;
using Timetablez.Models;

namespace Timetablez
{
    public partial class TimeTableGen : Form
    {
        public TimeTableGen()
        {
            string cStr = AppGlobals.dbLocal;

            GenGlobals.studentCourses = DataRead.GetDataAsList<StudentCourses>(cStr, "SELECT * FROM StudentCourses");
            GenGlobals.Courses = DataRead.GetDataAsList<Courses>(cStr, "SELECT * FROM Courses");
            GenGlobals.Rooms = DataRead.GetDataAsList<Rooms>(cStr, "SELECT * FROM Rooms");
            GenGlobals.Teachers = DataRead.GetDataAsList<TeacherCourses>(cStr, "SELECT * FROM TeacherCourses");
            GenGlobals.Periods = DataRead.GetDataAsList<WeekPeriods>(cStr, "SELECT * FROM WeekPeriods");
            GenGlobals.teachersFullInfo = DataRead.GetDataAsList<Teachers>(cStr, "SELECT * FROM Teachers");
            GenGlobals.teacherAvailability = DataRead.GetDataAsList<TeacherPeriods>(cStr, "SELECT * FROM TeacherPeriods");

            InitializeComponent();

            SQLtoTimetable();
            if (GenGlobals.timetableFromSQL.Count != 0)
            {
                InitialiseAlgoEdit(true);
                GenGlobals.editAllowed = true;
            }
            else
            {
                InitialiseAlgoEdit();
            }

        }

        private void InitialiseAlgoEdit(bool stat = false)
        {
            btnFixAll.Enabled = stat;
            btnFixRooms.Enabled = stat;
            btnFixStudents.Enabled = stat;
            btnFixTeachers.Enabled = stat;
            btnPrintConflicts.Enabled = stat;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length != 0)
            {
                try
                {
                    GenGlobals.maxConflicts = int.Parse(richTextBox1.Text);
                }
                catch
                {
                    MessageBox.Show("Non-integer input in MaxConflicts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    richTextBox1.Text = "0";

                }

            }

        }

        private void btnWriteToSQL_Click(object sender, EventArgs e)
        {
            WriteToSQL(GenGlobals.timetableStored, "WeeklySchedule");
            string cStr = AppGlobals.dbLocal;

            using (SqlConnection conn = new SqlConnection(cStr))
            {
                conn.Open();
                using (SqlCommand clearCmd = new SqlCommand($"DELETE FROM WeeklyScheduleTemp", conn))
                {
                    clearCmd.ExecuteNonQuery();
                }
            }

            AppGlobals.tempMade = false;

            btnWriteToSQL.Enabled = false;
            btnWriteToSQL.BackColor = SystemColors.AppWorkspace;
        }

        private void btnFixRooms_Click(object sender, EventArgs e)
        {
            GenGlobals.timetableFromSQL = FixConflict(GenGlobals.timetableFromSQL, CheckConflicts(GenGlobals.timetableFromSQL, "Rooms"), "Rooms");
            editorBox.AppendText("Attempted to fix rooms." + Environment.NewLine);
            WriteToSQL(GenGlobals.timetableFromSQL, "WeeklySchedule");
        }

        private void btnFixTeachers_Click(object sender, EventArgs e)
        {
            GenGlobals.timetableFromSQL = FixConflict(GenGlobals.timetableFromSQL, CheckConflicts(GenGlobals.timetableFromSQL, "Teachers"), "Teachers");
            editorBox.AppendText("Attempted to fix teachers." + Environment.NewLine);
            WriteToSQL(GenGlobals.timetableFromSQL, "WeeklySchedule");
        }

        private void btnFixStudents_Click(object sender, EventArgs e)
        {
            int conflictNumber = CheckConflicts(GenGlobals.timetableFromSQL, "Students").Count();
            int newNumberConflicts = conflictNumber;

            do
            {
                conflictNumber = newNumberConflicts;
                GenGlobals.timetableFromSQL = ResolveConflictByRelocation(GenGlobals.timetableFromSQL);
                GenGlobals.timetableFromSQL = ResolveConflictByStudentSwap(GenGlobals.timetableFromSQL);
                newNumberConflicts = CheckConflicts(GenGlobals.timetableFromSQL, "Students").Count();

            } while (newNumberConflicts < conflictNumber);
            editorBox.AppendText("Attempted to fix students." + Environment.NewLine);
            WriteToSQL(GenGlobals.timetableFromSQL, "WeeklySchedule");
        }

        private void btnFixAll_Click(object sender, EventArgs e)
        {
            GenGlobals.timetableFromSQL = FixConflict(GenGlobals.timetableFromSQL, CheckConflicts(GenGlobals.timetableFromSQL, "Rooms"), "Rooms");
            GenGlobals.timetableFromSQL = FixConflict(GenGlobals.timetableFromSQL, CheckConflicts(GenGlobals.timetableFromSQL, "Teachers"), "Teachers");

            int conflictNumber = CheckConflicts(GenGlobals.timetableFromSQL, "Students").Count();
            int newNumberConflicts = conflictNumber;

            do
            {
                conflictNumber = newNumberConflicts;
                GenGlobals.timetableFromSQL = ResolveConflictByRelocation(GenGlobals.timetableFromSQL);
                GenGlobals.timetableFromSQL = ResolveConflictByStudentSwap(GenGlobals.timetableFromSQL);
                newNumberConflicts = CheckConflicts(GenGlobals.timetableFromSQL, "Students").Count();

            } while (newNumberConflicts < conflictNumber);
            editorBox.AppendText("Attempted to fix All." + Environment.NewLine);
            WriteToSQL(GenGlobals.timetableFromSQL, "WeeklySchedule");
        }

        private void btnPrintConflicts_Click(object sender, EventArgs e)
        {
            editorBox.AppendText("Student Conflicts: " + CheckConflicts(GenGlobals.timetableFromSQL, "Students").Count() + Environment.NewLine);
            editorBox.AppendText("Teacher Conflicts: " + CheckConflicts(GenGlobals.timetableFromSQL, "Teachers").Count() + Environment.NewLine);
            editorBox.AppendText("Room Conflicts: " + CheckConflicts(GenGlobals.timetableFromSQL, "Rooms").Count() + Environment.NewLine);
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            GenerateTimetable();

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (GenGlobals.algoRun)
                GenGlobals.stopRequested = true;

        }


        public List<ClassInfo> CreateTimeTable()
        {

            List<ClassInfo> Timetable = new List<ClassInfo>();
            List<StudentGroup> randomGroups = CreateStudentGroups(GenGlobals.studentCourses, GenGlobals.Courses);

            foreach (StudentGroup Group in randomGroups)
            {
                ClassInfo Class = new ClassInfo();

                Class.courseID = Group.courseID;
                Class.groupPopulation = Group;

                // Create list of appropraite periods
                var availablePeriods = GenGlobals.Periods
                    .Where(p => p.Accessible)
                    .ToList();

                // Prevent no period eligible early
                if (availablePeriods.Count == 0)
                {
                    MessageBox.Show(
                            "No accessible periods available. \n The timetable cannot be generated.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return null;
                }

                List<string> usedDays = new List<string>();

                // Count how often each period has been used so far in the timeTable
                Dictionary<int, int> periodUsage = new Dictionary<int, int>();

                foreach (var g in Timetable)
                {
                    foreach (var pid in g.periodID)
                    {
                        if (!periodUsage.ContainsKey(pid))
                            periodUsage[pid] = 0;
                        periodUsage[pid]++;
                    }
                }


                for (int i = 0; i < Class.periodID.Length; i++)
                {

                    // Rank available periods based on usage and day uniqueness
                    List<PeriodRank> rankedPeriods = new List<PeriodRank>();

                    // Loop through periods
                    foreach (var p in availablePeriods)
                    {
                        // Dont want a class more than once on the same day
                        if (usedDays.Contains(p.DayOfWeek))
                            continue;


                        int usage = 0;
                        if (periodUsage.ContainsKey(p.PeriodID))
                        {
                            usage = periodUsage[p.PeriodID];
                        }

                        // Store for later
                        rankedPeriods.Add(new PeriodRank(p.PeriodID, p.DayOfWeek, usage));
                    }

                    if (rankedPeriods.Count == 0)
                    {
                        MessageBox.Show(
                            "Not enough unique days with accessible periods.",
                            "Room Assignment Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return null;
                    }

                    var bestPeriod = rankedPeriods
                        .OrderBy(rp => rp.Usage)
                        .ThenBy(rp => GenGlobals.rng.Next()) // random tie-breaker
                        .First();

                    // Assign Period to class and add it to the days used
                    Class.periodID[i] = bestPeriod.periodID;
                    usedDays.Add(bestPeriod.dayOfWeek);

                    // Remove from pool to avoid reuse
                    availablePeriods.RemoveAll(p => p.PeriodID == bestPeriod.periodID);
                }


                //Create list of appropraite rooms
                int groupSize = Group.studentIDs.Count;

                int requiredRoomType = GenGlobals.Courses
                    .Where(c => c.CourseID == Class.courseID)
                    .Select(c => c.RoomTypeID)
                    .Single();

                var eligibleRooms = GenGlobals.Rooms
                    .Where(r => r.RoomTypeID == requiredRoomType && r.NoDesks >= groupSize && r.Enabled)
                    .ToList();

                //Prevent no room eligible early
                if (eligibleRooms.Count == 0)
                {
                    MessageBox.Show(
                            $"No room found with enough capacity for group size {groupSize}",
                            "Room Assignment Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return null;
                }

                // Find the best room to assign
                var rankedRooms = new List<RoomRank>();
                foreach (var room in eligibleRooms)
                {
                    int conflictCount = 0;

                    // Find the number of conflicts picking this room would cause
                    foreach (var pid in Class.periodID)
                    {
                        bool conflict = Timetable.Any(
                            g => g.roomID == room.RoomID
                            && g.periodID.Contains(pid));

                        if (conflict)
                            conflictCount++;
                    }

                    // Find how many classes are already assigned to the room
                    int assignedCount = Timetable.Count(g => g.roomID == room.RoomID);

                    rankedRooms.Add(new RoomRank(room.RoomID, conflictCount, assignedCount));
                }

                if (rankedRooms.Count > 0)
                {
                    var bestRoom = rankedRooms
                        .OrderBy(r => r.conflictCount)
                        .ThenBy(r => r.assignedCount)
                        .ThenBy(r => GenGlobals.rng.Next()) // tie-breaker
                        .First();

                    // Assign the best room found
                    Class.roomID = bestRoom.roomID;
                }
                else
                {
                    MessageBox.Show(
                            $"No valid room assignment possible for Group {Class.groupPopulation.studentGroupID}",
                            "Room Assignment Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return null;
                }


                // Add class to timeTable
                Timetable.Add(Class);

            }

            return Timetable;
        }


        public List<StudentGroup> CreateStudentGroups(List<StudentCourses> students, List<Courses> courses)
        {
            // Create a mapping of each course and the number of students enrolled
            Dictionary<int, int> courseToStudents = new Dictionary<int, int>();

            // Search through the available courses
            foreach (Courses course in courses)
            {
                int EnrolledNO = 0;
                foreach (StudentCourses student in students)
                {
                    if (student.CourseID == course.CourseID)
                    {
                        EnrolledNO += 1;

                    }
                }
                courseToStudents.Add(course.CourseID, EnrolledNO);
            }

            int nextGroupID = 1;
            List<StudentGroup> studentGroups = new List<StudentGroup>();

            // Loop through the mapping to create groups by subject
            foreach (var Course in courseToStudents)
            {
                // Splitting them into random groups
                int? courseGroupSize = courses.FirstOrDefault(c => c.CourseID == Course.Key)?.GroupSize;

                /* 
                 * So if there were 50 students, and class capacity was 20, there will be 50/20 = 2.5 -> 3 groups
                 * The capacity of each class is 50/3 = 16.67 -> 17
                 */
                int groupCount = (int)Math.Ceiling((double)Course.Value / courseGroupSize.Value);
                int actualCapacity = (int)Math.Ceiling((double)Course.Value / groupCount);

                // Create empty groups
                for (int i = 0; i < groupCount; i++)
                {
                    var group = new StudentGroup(nextGroupID++, Course.Key, actualCapacity);
                    studentGroups.Add(group);
                }

            }

            return studentGroups;
        }



        public Dictionary<int, Dictionary<int, int>> CheckConflicts(List<ClassInfo> Timetable, string Area)
        {
            Dictionary<int, Dictionary<int, int>> Results = new Dictionary<int, Dictionary<int, int>>();

            /*
             * 1. Build a mapping of which rooms/teacher/student are used in each period
             * 2. Identify which rooms/teacher/student appear more than once once in the same period
             * 3. For each conflicting period work out how many conflicts exist in that period
             */


            if (Area == "Rooms")
            {
                Dictionary<int, List<int>> periodRoomMap = new Dictionary<int, List<int>>();
                Dictionary<int, Dictionary<int, int>> roomConflicts = new Dictionary<int, Dictionary<int, int>>();

                // Create the mapping
                foreach (var Class in Timetable)
                {
                    foreach (int period in Class.periodID)
                    {
                        if (!periodRoomMap.ContainsKey(period))
                            periodRoomMap.Add(period, new List<int>());

                        periodRoomMap[period].Add(Class.roomID);

                    }
                }

                // Recording the Conflicts                
                foreach (var periodEntry in periodRoomMap)
                {
                    // Group by period ID, and wherever there is more than one of the same, add it to a list
                    var conflictGroups = periodEntry.Value
                        .GroupBy(id => id)
                        .Where(group => group.Count() > 1)
                        .ToList();


                    foreach (var group in conflictGroups)
                    {
                        int room = group.Key;
                        // If there are 3 rooms in the same period, then there are 3-1=2 too many rooms
                        int conflictCount = group.Count() - 1;
                        int period = periodEntry.Key;

                        // Create a new key in roomconflicts, or just add the number of conflicts if it doesnt 
                        if (!roomConflicts.ContainsKey(room))
                        {
                            roomConflicts.Add(room, new Dictionary<int, int>());
                            roomConflicts[room].Add(period, conflictCount);
                        }
                        else
                        {
                            roomConflicts[room].Add(period, conflictCount);
                        }
                    }

                }

                Results = roomConflicts;
            }

            else if (Area == "Teachers")
            {
                Dictionary<int, List<int>> periodTeacherMap = new Dictionary<int, List<int>>();
                Dictionary<int, Dictionary<int, int>> teacherConflicts = new Dictionary<int, Dictionary<int, int>>();

                foreach (var Class in Timetable)
                {
                    foreach (int period in Class.periodID)
                    {
                        if (!periodTeacherMap.ContainsKey(period))
                            periodTeacherMap[period] = new List<int>();

                        periodTeacherMap[period].Add(Class.teacherID);
                    }
                }

                foreach (var periodEntry in periodTeacherMap)
                {
                    int period = periodEntry.Key;
                    var conflictGroups = periodEntry.Value
                        .GroupBy(tid => tid)
                        .Where(group => group.Count() > 1 && group.Key != -1); // skip fallback assignments

                    foreach (var group in conflictGroups)
                    {
                        int teacher = group.Key;
                        int conflictCount = group.Count() - 1;

                        if (!teacherConflicts.ContainsKey(teacher))
                        {
                            teacherConflicts.Add(teacher, new Dictionary<int, int>());
                            teacherConflicts[teacher].Add(period, conflictCount);
                        }
                        else
                        {
                            teacherConflicts[teacher].Add(period, conflictCount);
                        }

                    }
                }

                Results = teacherConflicts;
            }

            else if (Area == "Students")
            {
                Dictionary<int, List<int>> periodStudentMap = new Dictionary<int, List<int>>();
                Dictionary<int, Dictionary<int, int>> studentConflicts = new Dictionary<int, Dictionary<int, int>>();

                foreach (var gene in Timetable)
                {
                    foreach (int period in gene.periodID)
                    {
                        if (!periodStudentMap.ContainsKey(period))
                            periodStudentMap[period] = new List<int>();

                        foreach (int student in gene.groupPopulation.studentIDs) // prevent double-counting within a group
                        {
                            periodStudentMap[period].Add(student);
                        }
                    }

                }

                foreach (var periodEntry in periodStudentMap)
                {
                    int period = periodEntry.Key;
                    var conflictGroups = periodEntry.Value
                        .GroupBy(sid => sid)
                        .Where(group => group.Count() > 1 && group.Key != -1);

                    foreach (var group in conflictGroups)
                    {
                        int studentID = group.Key;
                        int conflictCount = group.Count() - 1;

                        if (!studentConflicts.ContainsKey(studentID))
                        {
                            studentConflicts.Add(studentID, new Dictionary<int, int>());
                            studentConflicts[studentID].Add(period, conflictCount);
                        }
                        else
                        {
                            studentConflicts[studentID].Add(period, conflictCount);
                        }

                    }
                }

                Results = studentConflicts;
            }

            return Results;

        }


        public List<ClassInfo> FixConflict(List<ClassInfo> Timetable, Dictionary<int, Dictionary<int, int>> conflicts, string ConflictType)
        {
            var availablePeriods = GenGlobals.Periods
                    .Where(p => p.Accessible)
                    .ToList();

            /*
             * 1. For each conflict, find all the rooms which are causing it
             * 2. Keep the first class as is, and attempt to move the others
             * 3. For each class to be moved, find all periods that do not use the same day, and do not cause a new conflict
             * 4. Randomly choose one to move the class to, if any exist
             */
            if (ConflictType == "Rooms")
            {
                foreach (var conflict in conflicts)
                {
                    int roomId = conflict.Key;
                    var allConflicts = conflict.Value;

                    // Loop through each conflicting period
                    foreach (var conflictEntry in allConflicts)
                    {
                        int conflictingPeriodId = conflictEntry.Key;
                        int conflictCount = conflictEntry.Value;

                        // Gets the specific conflicting classes for that room
                        var conflictingClasses = Timetable
                            .Where(g => g.roomID == roomId && g.periodID.Contains(conflictingPeriodId))
                            .ToList();

                        bool skipFirst = true;
                        foreach (var Class in conflictingClasses)
                        {
                            // Keep the first room ID the same
                            if (skipFirst)
                            {
                                skipFirst = false;
                                continue;
                            }

                            // Loop through the assigned periods
                            for (int i = 0; i < Class.periodID.Length; i++)
                            {
                                // Find the conflicting period
                                if (Class.periodID[i] == conflictingPeriodId)
                                {
                                    var groupStudents = Class.groupPopulation.studentIDs;

                                    var usedDays = Class.periodID
                                        .Where(pid => pid != conflictingPeriodId) // Exclude the conflicting period
                                        .Select(pid => GenGlobals.Periods.First(p => p.PeriodID == pid).DayOfWeek) // Find the days of the week of the other periods
                                        .ToList();

                                    var possiblePeriods = availablePeriods
                                        .Where(p =>
                                            !usedDays.Contains(p.DayOfWeek) && // Unique day for this gene
                                            !Timetable.Any(g =>
                                                g.periodID.Contains(p.PeriodID) && (
                                                    g.roomID == Class.roomID ||
                                                    g.teacherID == Class.teacherID ||
                                                    g.groupPopulation.studentIDs.Intersect(groupStudents).Any()
                                                )
                                            )
                                        )
                                        .ToList();

                                    if (possiblePeriods.Count != 0)
                                    {
                                        // Pick a random period from those available and replace it
                                        var replacementPeriod = possiblePeriods[GenGlobals.rng.Next(possiblePeriods.Count)];
                                        Class.periodID[i] = replacementPeriod.PeriodID;

                                    }
                                }

                            }
                        }

                    }
                }

            }

            /*
             * 1. Find the classes with the conflicting teachers
             * 2. Keep the first class unchanged and attempt to fix the others
             * 3. First try move each conflicting class to another available period
             * 4. If no valid period exists, then try assign a different teacher to the class
             * 5. If that also doesnt work, try both a new tacher and different period
             */
            else if (ConflictType == "Teachers")
            {
                foreach (var conflict in conflicts)
                {
                    int teacherID = conflict.Key;
                    var AllConflicts = conflict.Value;

                    // Loop through each conflicting period
                    foreach (var conflictEntry in AllConflicts)
                    {
                        int conflictingPeriodId = conflictEntry.Key;
                        int conflictCount = conflictEntry.Value;

                        // Get the specific conflicting classes
                        var conflictingClasses = Timetable
                            .Where(g => g.teacherID == teacherID && g.periodID.Contains(conflictingPeriodId))
                            .ToList();

                        bool skipFirst = true;
                        foreach (var Class in conflictingClasses)
                        {
                            // Leave first class the same
                            if (skipFirst)
                            {
                                skipFirst = false;
                                continue;
                            }

                            // Find the conflicting period
                            for (int i = 0; i < Class.periodID.Length; i++)
                            {
                                if (Class.periodID[i] == conflictingPeriodId)
                                {
                                    int roomID = Class.roomID;

                                    // Find the periods where the teacher is already teaching
                                    var usedTeacherPeriods = Timetable
                                        .Where(g => g.teacherID == teacherID && g != Class)
                                        .SelectMany(g => g.periodID)
                                        .ToList();

                                    // Find the periods where the room is already being used
                                    var usedRoomPeriods = Timetable
                                        .Where(g => g.roomID == roomID && g != Class)
                                        .SelectMany(g => g.periodID)
                                        .ToList();

                                    // Find the days already used by the class
                                    var usedDays = Class.periodID
                                        .Where(pid => pid != conflictingPeriodId)
                                        .Select(pid => GenGlobals.Periods.First(p => p.PeriodID == pid).DayOfWeek)
                                        .ToList();

                                    var groupStudents = Class.groupPopulation.studentIDs; // Students in the group

                                    // Find the periods already used by the students
                                    var usedStudentPeriods = Timetable
                                        .Where(g => g != Class && g.groupPopulation.studentIDs.Intersect(groupStudents).Any())
                                        .SelectMany(g => g.periodID)
                                        .ToList();

                                    // Find all the periods which the students, teachers and room dont use and not on a day which is already used
                                    var possiblePeriods = GenGlobals.Periods
                                        .Where(p =>
                                            p.Accessible &&
                                            !usedTeacherPeriods.Contains(p.PeriodID) &&
                                            !usedRoomPeriods.Contains(p.PeriodID) &&
                                            !usedStudentPeriods.Contains(p.PeriodID) &&
                                            !usedDays.Contains(p.DayOfWeek)
                                        )
                                        .Select(p => p.PeriodID)
                                        .ToList();

                                    if (possiblePeriods.Count != 0)
                                    {
                                        // Assign new period
                                        var replacementPeriod = possiblePeriods[GenGlobals.rng.Next(possiblePeriods.Count)];
                                        Class.periodID[i] = replacementPeriod;

                                    }
                                    else
                                    {
                                        int courseID = Class.courseID;
                                        int currentPeriod = Class.periodID[i];
                                        roomID = Class.roomID;

                                        var eligibleTeachers = GenGlobals.Teachers
                                            .Where(t => t.CourseID == courseID && t.TeacherID != teacherID) // Correct course and not the current teacher
                                            .Select(t => t.TeacherID)
                                            .ToList();

                                        bool resolved = false;

                                        foreach (var newTeacherID in eligibleTeachers)
                                        {
                                            // STEP 1: Try same period with a new teacher

                                            // Check if teacher is busy in this period
                                            bool teacherBusy = Timetable.Any(g =>
                                                g.teacherID == newTeacherID
                                                && g.periodID.Contains(currentPeriod));

                                            // If no class found with teacher at that period, then assign new teacher to class
                                            if (!teacherBusy)
                                            {
                                                Class.teacherID = newTeacherID;
                                                resolved = true;
                                                break;
                                            }

                                            // STEP 2: Try new teacher with a new valid period
                                            usedTeacherPeriods = Timetable
                                                .Where(g => g.teacherID == newTeacherID && g != Class)
                                                .SelectMany(g => g.periodID)
                                                .ToList();

                                            // Find all the periods which the students, teachers and room dont use and not on a day which is already used
                                            possiblePeriods = GenGlobals.Periods
                                                .Where(p =>
                                                    p.Accessible &&
                                                    !usedTeacherPeriods.Contains(p.PeriodID) &&
                                                    !usedRoomPeriods.Contains(p.PeriodID) &&
                                                    !usedStudentPeriods.Contains(p.PeriodID) &&
                                                    !usedDays.Contains(p.DayOfWeek))
                                                .Select(p => p.PeriodID)
                                                .ToList();

                                            if (possiblePeriods.Count > 0)
                                            {
                                                // Assign the new teacher and period
                                                Class.teacherID = newTeacherID;
                                                Class.periodID[i] = possiblePeriods[GenGlobals.rng.Next(possiblePeriods.Count)];
                                                resolved = true;
                                                break;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            /*
             * 1. Find the classes with the conflicting students
             * 2. For each class to be moved, find all periods that do not use the same day, and do not cause a new conflict
             * 3. Randomly choose one to move the class to, if any exist
             */
            else if (ConflictType == "Students")
            {
                foreach (var conflict in conflicts)
                {
                    int studentId = conflict.Key;
                    var allConflicts = conflict.Value;

                    foreach (var conflictEntry in allConflicts)
                    {
                        int conflictingPeriodId = conflictEntry.Key;

                        var conflictingClasses = Timetable
                            .Where(g =>
                                g.groupPopulation.studentIDs.Contains(studentId)
                                && g.periodID.Contains(conflictingPeriodId))
                            .ToList();

                        foreach (var Class in conflictingClasses)
                        {
                            for (int i = 0; i < Class.periodID.Length; i++)
                            {
                                if (Class.periodID[i] == conflictingPeriodId)
                                {

                                    var groupStudents = Class.groupPopulation.studentIDs;

                                    var usedDays = Class.periodID
                                        .Where(pid => pid != conflictingPeriodId)
                                        .Select(pid => GenGlobals.Periods.First(p => p.PeriodID == pid).DayOfWeek)
                                        .ToList();

                                    var possiblePeriods = availablePeriods
                                        .Where(p =>
                                            !usedDays.Contains(p.DayOfWeek) &&
                                            !Timetable.Any(g =>
                                                g.periodID.Contains(p.PeriodID) && (
                                                    g.roomID == Class.roomID ||
                                                    g.teacherID == Class.teacherID ||
                                                    g.groupPopulation.studentIDs.Intersect(groupStudents).Any()
                                                )
                                            )
                                        )
                                        .ToList();

                                    if (possiblePeriods.Count > 0)
                                    {
                                        var replacementPeriod = possiblePeriods[GenGlobals.rng.Next(possiblePeriods.Count)];
                                        int newPeriod = replacementPeriod.PeriodID;
                                        Class.periodID[i] = newPeriod;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Timetable;

        }


        public List<ClassInfo> AssignTeachers(List<ClassInfo> Timetable)
        {
            // Create mapping of each teacher and thier available periods
            var teacherToPeriods = GenGlobals.teacherAvailability
                .Where(p => p.Accessible)
                .GroupBy(p => p.TeacherID)
                .ToDictionary(g => g.Key, g => g.Select(p => p.PeriodID).ToList());

            foreach (ClassInfo Class in Timetable)
            {
                // Teach the right course
                var eligibleTeachers = GenGlobals.Teachers
                    .Where(t => t.CourseID == Class.courseID)
                    .Select(t => t.TeacherID)
                    .ToList();

                // Make sure they are free for ALL the asigned periods
                var availableTeachers = eligibleTeachers
                    .Where(teacherId =>
                        Class.periodID
                            .All(periodId => GenGlobals.teacherAvailability // Checks against all three periods
                                .Any(ta => ta.TeacherID == teacherId && // Checks that any entry confirms the teacher is available
                                    ta.PeriodID == periodId &&
                                    ta.Accessible)
                                )
                            )
                    .ToList();

                var rankedTeachers = new List<TeacherRank>();

                // Loop through all teachers
                foreach (var tid in availableTeachers)
                {
                    int conflictCount = 0;

                    // Loop through each assigned period
                    foreach (var pid in Class.periodID)
                    {
                        // Find if it causes a conflict
                        bool conflict = Timetable.Any(g => g.teacherID == tid && g.periodID.Contains(pid));
                        if (conflict)
                            conflictCount++;
                    }

                    // Number of classes already teaching
                    int assignedCount = Timetable.Count(g => g.teacherID == tid);

                    rankedTeachers.Add(new TeacherRank(tid, conflictCount, assignedCount));
                }


                if (rankedTeachers.Count > 0)
                {
                    var bestSelection = rankedTeachers
                        .OrderBy(t => t.conflictCount) // Conflict count Ascending (0 first)
                        .ThenBy(t => t.assignedCount) // Then by how many classes they already teach
                        .ThenBy(t => GenGlobals.rng.Next()) // Tiebreaker
                        .FirstOrDefault();

                    Class.teacherID = bestSelection.teacherID;
                }
                else
                {
                    Class.teacherID = -1;
                    txtLogBox.AppendText($"No available teachers for CourseID {Class.courseID}, GroupID {Class.groupPopulation.studentGroupID}. TeacherID set to -1. " + Environment.NewLine);
                }
            }

            return Timetable;

        }

        // Greedy Algorithm Used
        public Dictionary<int, int> CheckTeacherShortage(List<StudentGroup> Groups)
        {
            Dictionary<int, int> teacherShort = new Dictionary<int, int>();

            txtLogBox.AppendText("===== Teacher Availability Check ===== " + Environment.NewLine);

            // Group by CourseID to get all groups per course
            var courseGroupMap = Groups
                .GroupBy(g => g.courseID)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Mapping of teachers to students
            Dictionary<int, List<int>> teacherTracking = GenGlobals.teachersFullInfo
                .Select(t => t.TeacherID)
                .ToDictionary(id => id, id => new List<int>());


            // Get every teacher ID
            var teacherIds = GenGlobals.teachersFullInfo
                .Select(t => t.TeacherID)
                .ToList();

            Dictionary<int, int> teacherGroupsAllowed = new Dictionary<int, int>();

            foreach (int id in teacherIds)
            {
                // How many periods is this teacher available
                int availablePeriodCount = GenGlobals.teacherAvailability
                    .Where(p => p.Accessible && p.TeacherID == id)
                    .Count();

                // if a teacher is available for 19 periods and each class consists of three periods, 19/3 = 6.333
                float availabilityRatio = (float)availablePeriodCount / (float)GenGlobals.periodNumber;

                // So the teacher can have 6 classes
                teacherGroupsAllowed[id] = (int)Math.Floor(availabilityRatio);
            }


            foreach (var course in courseGroupMap)
            {
                foreach (var group in course.Value)
                {
                    // All teachers that can teach this course
                    var eligibleTeachers = GenGlobals.Teachers
                        .Where(t => t.CourseID == course.Key)
                        .Select(t => t.TeacherID)
                        .ToList();

                    bool groupAssigned = false;

                    // Attempt to asign each group to the first applicable teacher with availability
                    foreach (var teacher in eligibleTeachers)
                    {
                        if (teacherTracking[teacher].Count() < teacherGroupsAllowed[teacher])
                        {
                            teacherTracking[teacher].Add(group.studentGroupID);
                            groupAssigned = true;
                            break;
                        }
                    }

                    // If a group cant be assigned to ANY teacher, then there is a shortage
                    if (!groupAssigned)
                    {
                        if (!teacherShort.ContainsKey(course.Key))
                        {
                            teacherShort.Add(course.Key, 1);
                        }
                        else
                        {
                            teacherShort[course.Key] += 1;
                        }

                    }
                    groupAssigned = false;

                }
            }

            foreach (var course in teacherShort)
            {
                txtLogBox.AppendText($"There are {course.Value} groups without a teacher for Course ID {course.Key}" + Environment.NewLine);
            }

            return teacherShort;
        }


        public bool CheckRoomShortage(List<StudentGroup> Groups)
        {
            bool shortage = false;

            txtLogBox.AppendText("===== Room Availability Check ===== " + Environment.NewLine);

            // Group by CourseID to get all groups per course
            var courseGroupMap = Groups
                .GroupBy(g => g.courseID)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Mapping of rooms to assigned groups (tracking how many groups each room takes)
            Dictionary<int, List<int>> roomTracking = GenGlobals.Rooms
                .Where(r => r.Enabled)
                .Select(r => r.RoomID)
                .ToDictionary(id => id, id => new List<int>());

            // Get every enabled RoomID
            var roomIds = GenGlobals.Rooms
                .Where(r => r.Enabled)
                .Select(r => r.RoomID)
                .ToList();

            // How many groups each room is allowed to host
            Dictionary<int, int> roomGroupsAllowed = new Dictionary<int, int>();

            foreach (int id in roomIds)
            {
                // How many periods is this room available
                int availablePeriodCount = GenGlobals.Periods
                    .Where(p => p.Accessible)
                    .Count();

                float availabilityRatio = (float)availablePeriodCount / (float)GenGlobals.periodNumber;

                // So the room can support X full groups
                roomGroupsAllowed[id] = (int)Math.Floor(availabilityRatio);
            }


            foreach (var course in courseGroupMap)
            {
                int courseId = course.Key;
                var courseGroups = course.Value;

                // Room requirements come from the course definition
                var courseInfo = GenGlobals.Courses.First(c => c.CourseID == courseId);
                int requiredRoomType = courseInfo.RoomTypeID;

                // Find the maximum group size for this course
                int maxGroupSize = courseGroups.Max(g => g.groupCapacity);

                // All rooms that satisfy type, desk count and are enabled
                var eligibleRooms = GenGlobals.Rooms
                    .Where(r =>
                        r.Enabled &&
                        r.RoomTypeID == requiredRoomType &&
                        r.NoDesks >= maxGroupSize)
                    .Select(r => r.RoomID)
                    .ToList();

                foreach (var group in courseGroups)
                {
                    bool groupAssigned = false;

                    // Attempt to assign each group to the first room with remaining capacity
                    foreach (var roomId in eligibleRooms)
                    {
                        if (roomTracking[roomId].Count() < roomGroupsAllowed[roomId])
                        {
                            roomTracking[roomId].Add(group.studentGroupID);
                            groupAssigned = true;
                            break;
                        }
                    }

                    // If a group cannot be assigned to ANY eligible room, then there is a room shortage
                    if (!groupAssigned)
                    {
                        txtLogBox.AppendText(
                            $"There are no rooms available for Course ID {courseId} for Group {group.studentGroupID}"
                            + Environment.NewLine);

                        shortage = true;
                    }

                    groupAssigned = false;
                }
            }

            return shortage;
        }



        public List<ClassInfo> AssignStudents(List<ClassInfo> Timetable)
        {
            foreach (ClassInfo cls in Timetable)
            {
                cls.groupPopulation.studentIDs.Clear();
            }

            // Build Course to List of Students dictionary
            Dictionary<int, List<int>> courseToStudents = GenGlobals.studentCourses
                .GroupBy(sc => sc.CourseID)
                .ToDictionary(g => g.Key, g => g.Select(sc => sc.StudentID).ToList());

            // Mapping of the students and thier assigned periods
            Dictionary<int, List<int>> studentPeriods = new Dictionary<int, List<int>>();

            // Used to fill in gaps after (Creates a copy)
            Dictionary<int, List<int>> remainingStudents = courseToStudents
                .ToDictionary(entry => entry.Key, entry => new List<int>(entry.Value));

            // Assign students to each class's group
            foreach (ClassInfo Class in Timetable)
            {
                int courseID = Class.courseID;
                List<int> Assigned = new List<int>();
                int groupSize = Class.groupPopulation.groupCapacity;

                if (!courseToStudents.ContainsKey(courseID))
                {
                    txtLogBox.AppendText($"No students enrolled in Course {courseID} " + Environment.NewLine);
                    continue;
                }

                // Enrolled students in that course
                List<int> availableStudents = courseToStudents[courseID];

                foreach (int student in availableStudents)
                {
                    bool conflict = false;

                    if (studentPeriods.ContainsKey(student))
                    {
                        // Loop through all the periods of the class and see if the student is free
                        foreach (int period in Class.periodID)
                        {
                            if (studentPeriods[student].Contains(period))
                            {
                                conflict = true;
                                break;
                            }
                        }
                    }

                    if (!conflict)
                    {
                        Assigned.Add(student);

                        // Make a new entry if not in Student Periods
                        if (!studentPeriods.ContainsKey(student))
                            studentPeriods[student] = new List<int>();

                        // Add the periods assigned to the mapping
                        foreach (int period in Class.periodID)
                            studentPeriods[student].Add(period);
                    }

                    // If class full then stop
                    if (Assigned.Count == groupSize)
                        break;
                }

                // Students have been assigned
                foreach (int student in Assigned)
                    remainingStudents[courseID].Remove(student);

                // Put students in the C# class
                Class.groupPopulation.studentIDs = Assigned;
            }

            // Fill the remaining spaces with students not assigned
            foreach (ClassInfo Class in Timetable)
            {
                int groupID = Class.groupPopulation.studentGroupID;
                int groupSize = Class.groupPopulation.groupCapacity;
                int courseID = Class.courseID;

                int currentSize = Class.groupPopulation.studentIDs.Count;

                // How many more needed
                int Needed = groupSize - currentSize;

                // Skip if full
                if (Needed <= 0) continue;

                List<int> fallbackPool = remainingStudents[courseID];

                while (Needed > 0 && fallbackPool.Count > 0)
                {
                    int index = GenGlobals.rng.Next(fallbackPool.Count);
                    int student = fallbackPool[index];

                    // Add the fallback student
                    Class.groupPopulation.studentIDs.Add(student);
                    fallbackPool.RemoveAt(index);
                    Needed--;

                    // Update StudentPeriods tracking
                    if (!studentPeriods.ContainsKey(student))
                    {
                        studentPeriods.Add(student, new List<int>());
                    }

                    // Assign all the periods of the class to the student
                    for (int i = 0; i < Class.periodID.Length; i++)
                    {
                        int pid = Class.periodID[i];
                        studentPeriods[student].Add(pid);
                    }
                }
            }

            return Timetable;
        }


        public List<ClassInfo> ResolveConflictByRelocation(List<ClassInfo> Timetable)
        {
            // Find the conflicts
            var studentConflicts = CheckConflicts(Timetable, "Students");

            foreach (var conflictEntry in studentConflicts)
            {
                int studentId = conflictEntry.Key;
                var conflictedPeriods = conflictEntry.Value.Keys.ToList();

                // Return all classes where the student appears AND has an overlapping period
                var conflictingClasses = Timetable
                    .Where(g =>
                        g.groupPopulation.studentIDs.Contains(studentId) &&
                        g.periodID.Any(p => conflictedPeriods.Contains(p)))
                    .ToList();

                foreach (var Class in conflictingClasses)
                {
                    int courseId = Class.courseID;

                    // Find a group which the student is not currently in, has a free space and is of the right course
                    var eligibleGroups = Timetable
                        .Where(g =>
                            g != Class &&
                            g.courseID == courseId &&
                            g.groupPopulation.studentIDs.Count < g.groupPopulation.groupCapacity)
                        .ToList();

                    // simulate moving the student to each potential group
                    foreach (var Candididate in eligibleGroups)
                    {

                        // Have to create a clone otherwise will point to the same place in memory
                        var simulatedTimetable = Timetable.Select(g => new ClassInfo
                        {
                            courseID = g.courseID,
                            teacherID = g.teacherID,
                            roomID = g.roomID,
                            periodID = (int[])g.periodID.Clone(),
                            groupPopulation = new StudentGroup(
                                g.groupPopulation.studentGroupID,
                                g.groupPopulation.courseID,
                                g.groupPopulation.groupCapacity)
                            {
                                studentIDs = new List<int>(g.groupPopulation.studentIDs)
                            }
                        }).ToList();

                        // The class that contains the student
                        var simCurrent = simulatedTimetable
                            .First(g => g.groupPopulation.studentGroupID == Class.groupPopulation.studentGroupID);

                        // The target class
                        var simTarget = simulatedTimetable
                            .First(g => g.groupPopulation.studentGroupID == Candididate.groupPopulation.studentGroupID);

                        // Move the student
                        simCurrent.groupPopulation.studentIDs.Remove(studentId);
                        simTarget.groupPopulation.studentIDs.Add(studentId);

                        var newConflicts = CheckConflicts(simulatedTimetable, "Students");

                        bool conflictResolved = !newConflicts.ContainsKey(studentId);

                        if (conflictResolved)
                        {
                            // Commit the change to the actual timeTable
                            Class.groupPopulation.studentIDs.Remove(studentId);
                            Candididate.groupPopulation.studentIDs.Add(studentId);

                            break;
                        }

                    }
                }

            }

            return Timetable;

        }

        public List<ClassInfo> ResolveConflictByStudentSwap(List<ClassInfo> Timetable)
        {
            // Detect current conflicts
            var studentConflicts = CheckConflicts(Timetable, "Students");

            bool swapMade = false;

            foreach (var Conflict in studentConflicts)
            {
                int studentOG = Conflict.Key;
                var conflictedPeriods = Conflict.Value.Keys.ToList();

                // Find where the student appears and has a conflict
                var conflictedClasses = Timetable
                    .Where(g => g.groupPopulation.studentIDs.Contains(studentOG)
                    && g.periodID.Any(p => conflictedPeriods.Contains(p))).ToList();

                // This may change every loop
                var currentConflicts = CheckConflicts(Timetable, "Students");
                int conflictCount = currentConflicts.Count;

                // Loop through each conflicted class
                foreach (var Class in conflictedClasses)
                {
                    // Find the other classes of the same course which the student is not enrolled in
                    var sameCourseClasses = Timetable
                        .Where(c => c.courseID == Class.courseID
                        && !c.groupPopulation.studentIDs.Contains(studentOG))
                        .ToList();

                    // Try swapping with every student in each eligible group
                    foreach (var Group in sameCourseClasses)
                    {
                        foreach (var student in Group.groupPopulation.studentIDs)
                        {
                            // Copy of the original timetable
                            var simulatedTimetable = Timetable.Select(g => new ClassInfo
                            {
                                courseID = g.courseID,
                                teacherID = g.teacherID,
                                roomID = g.roomID,
                                periodID = (int[])g.periodID.Clone(),
                                groupPopulation = new StudentGroup(
                                g.groupPopulation.studentGroupID,
                                g.groupPopulation.courseID,
                                g.groupPopulation.groupCapacity)
                                {
                                    studentIDs = new List<int>(g.groupPopulation.studentIDs)
                                }
                            }).ToList();

                            // Simulated versions of the two groups
                            var simClassOG = simulatedTimetable.First(g => g.groupPopulation.studentGroupID == Class.groupPopulation.studentGroupID);
                            var simCandidate = simulatedTimetable.First(g => g.groupPopulation.studentGroupID == Group.groupPopulation.studentGroupID);

                            // Perform swap
                            simClassOG.groupPopulation.studentIDs.Remove(studentOG);
                            simCandidate.groupPopulation.studentIDs.Remove(student);

                            simClassOG.groupPopulation.studentIDs.Add(student);
                            simCandidate.groupPopulation.studentIDs.Add(studentOG);

                            // Check to see if conflict resolved
                            var newConflicts = CheckConflicts(simulatedTimetable, "Students");
                            if (newConflicts.Count < conflictCount)
                            {
                                // Make the actual swap
                                Class.groupPopulation.studentIDs.Remove(studentOG);
                                Group.groupPopulation.studentIDs.Remove(student);

                                Class.groupPopulation.studentIDs.Add(student);
                                Group.groupPopulation.studentIDs.Add(studentOG);

                                swapMade = true;
                                break;

                            }
                        }
                        if (swapMade)
                            break;
                    }

                    if (swapMade)
                        break;

                }

            }

            return Timetable;
        }

        public void CreateTempTeachers(Dictionary<int, int> TeacherShortage)
        {
            int count = 0;
            txtLogBox.AppendText("Generating temp teachers... " + Environment.NewLine);
            var Periods = GenGlobals.Periods
                .Where(p => p.Accessible)
                .Select(p => p.PeriodID)
                .ToList();

            int groupsAllowed = (int)Math.Floor((double)Periods.Count / GenGlobals.periodNumber);


            foreach (var shortage in TeacherShortage)
            {
                int newTeacherCount = (int)Math.Ceiling((double)shortage.Value / groupsAllowed);
                for (int i = 1; i <= newTeacherCount; i++)
                {
                    count++;
                    int teachID = GenGlobals.teachersFullInfo.Count + 1;

                    Teachers tempTeacher = new Teachers
                    {
                        TeacherID = teachID,
                        Title = "Mi",
                        FirstName = "Temp",
                        Surname = count.ToString(),
                        Sex = "X"
                    };
                    GenGlobals.teachersFullInfo.Add(tempTeacher);


                    TeacherCourses tempCourse = new TeacherCourses
                    {
                        TeacherCourseID = GenGlobals.Teachers.Count + 1,
                        TeacherID = teachID,
                        CourseID = shortage.Key,
                        Accessible = true
                    };
                    GenGlobals.Teachers.Add(tempCourse);


                    foreach (var p in Periods)
                    {
                        TeacherPeriods tempPeriods = new TeacherPeriods
                        {
                            TeacherPeriodID = GenGlobals.teacherAvailability.Count + 1,
                            TeacherID = teachID,
                            PeriodID = p,
                            Accessible = true
                        };
                        GenGlobals.teacherAvailability.Add(tempPeriods);

                    }

                    txtLogBox.AppendText($"New teacher added of ID {teachID}, name of Temp{count.ToString()}, course ID {shortage.Key}" + Environment.NewLine);

                }

            }

        }

        public bool ShouldStop()
        {
            Application.DoEvents();
            if (GenGlobals.stopRequested)
            {
                System.Threading.Thread.Sleep(200);
                txtLogBox.AppendText("..." + Environment.NewLine);
                System.Threading.Thread.Sleep(200);
                txtLogBox.AppendText("..." + Environment.NewLine);
                System.Threading.Thread.Sleep(100);
                txtLogBox.AppendText("Stop requested → exiting algorithm..." + Environment.NewLine);
                GenGlobals.stopRequested = false;
                return true;
            }
            else
            {
                return false;
            }

        }

        public void GenerateTimetable()
        {
            int conflictsRoom = -1;
            int conflictsTeacher = 0;
            int conflictsStudent = 0;
            int acceptableConflictCount = 0;

            int finalConflict = 1000;


            List<ClassInfo> timeTable = new List<ClassInfo>();

            int lastChunkCount = 0;
            int maxConflicts = 70;

            // To be used as a base for checks after
            List<StudentGroup> MockGroup = CreateStudentGroups(GenGlobals.studentCourses, GenGlobals.Courses);

            // Checks if there are enough teachers and rooms
            Dictionary<int, int> checkTeach = CheckTeacherShortage(MockGroup);
            bool checkRoom = CheckRoomShortage(MockGroup);

            if (checkRoom)
            {
                txtLogBox.AppendText("Timetable generation stopped due to room shortage." + Environment.NewLine);
                return;

            }

            // Make temp teachers if shortage is found
            if (checkTeach.Count != 0)
            {
                txtLogBox.AppendText("Timetable generation stopped due to teacher shortage." + Environment.NewLine);
                CreateTempTeachers(checkTeach);

            }


            /*
             *  Loop will continue until the total number of conflicts are below the number set by the user
             *  1. Generate a base timetable template (Classes, room and periods)
             *  2. Fix any room conflicts
             *  3. Assign teachers, and then fix any conflicts
             *  4. Repeat steps 1-3 until no conflicts 
             *  5. Assign students, and then fix any conflicts (gradually increase allowed conflicts to prevent block)
             *  6. Try improve the schedule by swapping students and reallocating classes
             *  7. If the final number of conflicts is still above the threshold, regenerate and start again.
             */

            GenGlobals.algoRun = true;
            do
            {
                if (ShouldStop())
                    return;

                Stopwatch sw = Stopwatch.StartNew();

                do
                {
                    if (ShouldStop())
                        return;
                    do
                    {
                        if (ShouldStop())
                            return;

                        timeTable = this.CreateTimeTable();
                        if (timeTable == null)
                        {
                            txtLogBox.AppendText("Timetable generation stopped due to errors." + Environment.NewLine);
                            return;
                        }

                        timeTable = FixConflict(timeTable, CheckConflicts(timeTable, "Rooms"), "Rooms");
                        conflictsRoom = CheckConflicts(timeTable, "Rooms").Count();

                        timeTable = AssignTeachers(timeTable);
                        timeTable = FixConflict(timeTable, CheckConflicts(timeTable, "Teachers"), "Teachers");
                        conflictsTeacher = CheckConflicts(timeTable, "Teachers").Count();

                    } while (conflictsTeacher > 3);

                    timeTable = FixConflict(timeTable, CheckConflicts(timeTable, "Teachers"), "Teachers");
                    conflictsTeacher = CheckConflicts(timeTable, "Teachers").Count();

                } while ((conflictsTeacher + conflictsRoom) != 0);

                do
                {
                    if (ShouldStop())
                        return;

                    timeTable = AssignStudents(timeTable);
                    timeTable = FixConflict(timeTable, CheckConflicts(timeTable, "Students"), "Students");
                    conflictsStudent = CheckConflicts(timeTable, "Students").Count();

                    // Check how many 0.XXs chunks have elapsed
                    int currentChunks = (int)(sw.Elapsed.TotalSeconds / 0.01);

                    if (currentChunks > lastChunkCount)
                    {
                        // Increase the number of conflicts after a set time
                        if (acceptableConflictCount < maxConflicts)
                            acceptableConflictCount += 5;

                        lastChunkCount = currentChunks;
                    }

                } while (conflictsStudent > acceptableConflictCount);

                int newNumberConflictsStudent = conflictsStudent;

                do
                {
                    if (ShouldStop())
                        return;

                    conflictsStudent = newNumberConflictsStudent;
                    timeTable = ResolveConflictByRelocation(timeTable);
                    timeTable = ResolveConflictByStudentSwap(timeTable);
                    newNumberConflictsStudent = CheckConflicts(timeTable, "Students").Count();

                } while (newNumberConflictsStudent < conflictsStudent);

                finalConflict = CheckConflicts(timeTable, "Students").Count() + CheckConflicts(timeTable, "Teachers").Count() + CheckConflicts(timeTable, "Rooms").Count();

            } while (finalConflict > GenGlobals.maxConflicts);

            GenGlobals.algoRun = false;

            txtLogBox.AppendText("Student Conflicts: " + CheckConflicts(timeTable, "Students").Count() + Environment.NewLine);
            txtLogBox.AppendText("Teacher Conflicts: " + CheckConflicts(timeTable, "Teachers").Count() + Environment.NewLine);
            txtLogBox.AppendText("Room Conflicts: " + CheckConflicts(timeTable, "Rooms").Count() + Environment.NewLine);

            txtLogBox.AppendText("Finished " + Environment.NewLine);
            WriteToSQL(timeTable, "WeeklyScheduleTemp");

            btnWriteToSQL.Enabled = true;
            btnWriteToSQL.BackColor = System.Drawing.SystemColors.Control;

            GenGlobals.timetableStored = null;
            GenGlobals.timetableStored = timeTable;
            AppGlobals.tempMade = true;

        }


        public void WriteToSQL(List<ClassInfo> timeTable, string weeklyScheduleTable)
        {
            string cStr = AppGlobals.dbLocal;

            // Decide student-group table automatically
            string studentGroupTable =
                (weeklyScheduleTable == "WeeklyScheduleTemp")
                ? "StudentGroupTemp"
                : "StudentGroup";

            using (SqlConnection conn = new SqlConnection(cStr))
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // =========================================
                        // 1. Clear target tables
                        // =========================================
                        using (SqlCommand clearWeekly =
                            new SqlCommand($"TRUNCATE TABLE {weeklyScheduleTable}", conn, tran))
                        {
                            clearWeekly.ExecuteNonQuery();
                        }

                        using (SqlCommand clearStudentGroup =
                            new SqlCommand($"TRUNCATE TABLE {studentGroupTable}", conn, tran))
                        {
                            clearStudentGroup.ExecuteNonQuery();
                        }

                        // =========================================
                        // 2. Write WeeklySchedule / WeeklyScheduleTemp
                        //    (Group × Period)
                        // =========================================
                        foreach (var gene in timeTable)
                        {
                            int groupID = gene.groupPopulation.studentGroupID;
                            int courseID = gene.courseID;
                            int teacherID = gene.teacherID;
                            int roomID = gene.roomID;

                            foreach (int periodID in gene.periodID)
                            {
                                using (SqlCommand insertWeekly = new SqlCommand($@"
                            INSERT INTO {weeklyScheduleTable}
                                (GroupID, CourseID, TeacherID, RoomID, PeriodID)
                            VALUES
                                (@GroupID, @CourseID, @TeacherID, @RoomID, @PeriodID)",
                                    conn, tran))
                                {
                                    insertWeekly.Parameters.AddWithValue("@GroupID", groupID);
                                    insertWeekly.Parameters.AddWithValue("@CourseID", courseID);
                                    insertWeekly.Parameters.AddWithValue("@TeacherID", teacherID);
                                    insertWeekly.Parameters.AddWithValue("@RoomID", roomID);
                                    insertWeekly.Parameters.AddWithValue("@PeriodID", periodID);

                                    insertWeekly.ExecuteNonQuery();
                                }
                            }
                        }

                        // =========================================
                        // 3. Write StudentGroup / StudentGroupTemp
                        //    (Group × Student)
                        // =========================================
                        foreach (var gene in timeTable)
                        {
                            int groupID = gene.groupPopulation.studentGroupID;

                            foreach (int studentID in gene.groupPopulation.studentIDs)
                            {
                                using (SqlCommand insertStudent = new SqlCommand($@"
                            INSERT INTO {studentGroupTable}
                                (GroupID, StudentID)
                            VALUES
                                (@GroupID, @StudentID)",
                                    conn, tran))
                                {
                                    insertStudent.Parameters.AddWithValue("@GroupID", groupID);
                                    insertStudent.Parameters.AddWithValue("@StudentID", studentID);

                                    insertStudent.ExecuteNonQuery();
                                }
                            }
                        }

                        // =========================================
                        // 4. Commit
                        // =========================================
                        tran.Commit();

                        txtLogBox.AppendText(
                            $"{weeklyScheduleTable} and {studentGroupTable} updated successfully."
                            + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        MessageBox.Show(
                            "WriteToSQL failed:\n" + ex.Message,
                            "Database Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }

            // Enable edit buttons when final table is written
            if (weeklyScheduleTable == "WeeklySchedule" && !GenGlobals.editAllowed)
            {
                InitialiseAlgoEdit(true);
                GenGlobals.editAllowed = true;
            }
        }




        public static void dbRead(string cStr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cStr))
                {
                    conn.Open(); // Will throw if can't connect
                }

                Console.WriteLine("Success!");
            }
            catch
            {
                Console.WriteLine("Error!");
            }
        }


        public static void SQLtoTimetable()
        {
            string cStr = AppGlobals.dbLocal;

            Dictionary<int, ClassInfo> groupMap = new Dictionary<int, ClassInfo>();

            using (SqlConnection conn = new SqlConnection(cStr))
            {
                conn.Open();

                // =====================================
                // 1. Read WeeklySchedule (structure)
                // =====================================
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT GroupID, CourseID, TeacherID, RoomID, PeriodID
            FROM WeeklySchedule", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupID = (int)reader["GroupID"];
                        int courseID = (int)reader["CourseID"];
                        int teacherID = (int)reader["TeacherID"];
                        int roomID = (int)reader["RoomID"];
                        int periodID = (int)reader["PeriodID"];

                        if (!groupMap.ContainsKey(groupID))
                        {
                            int groupCapacity =
                                GenGlobals.Courses.First(c => c.CourseID == courseID).GroupSize;

                            groupMap[groupID] = new ClassInfo
                            {
                                courseID = courseID,
                                teacherID = teacherID,
                                roomID = roomID,
                                periodID = new int[GenGlobals.periodNumber],
                                groupPopulation = new StudentGroup(groupID, courseID, groupCapacity)
                            };
                        }

                        ClassInfo gene = groupMap[groupID];

                        // Add period if not already present
                        for (int i = 0; i < gene.periodID.Length; i++)
                        {
                            if (gene.periodID[i] == periodID)
                                break;

                            if (gene.periodID[i] == 0)
                            {
                                gene.periodID[i] = periodID;
                                break;
                            }
                        }
                    }
                }

                // =====================================
                // 2. Read StudentGroup (membership)
                // =====================================
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT GroupID, StudentID
            FROM StudentGroup", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupID = (int)reader["GroupID"];
                        int studentID = (int)reader["StudentID"];

                        if (groupMap.ContainsKey(groupID))
                        {
                            groupMap[groupID]
                                .groupPopulation
                                .studentIDs
                                .Add(studentID);
                        }
                    }
                }
            }

            GenGlobals.timetableFromSQL = groupMap.Values.ToList();
        }



    }
    public static class GenGlobals
    {
        public static bool Debug = false;

        public static List<StudentCourses> studentCourses = new List<StudentCourses>();
        public static List<Courses> Courses = new List<Courses>();
        public static List<Rooms> Rooms = new List<Rooms>();
        public static List<TeacherCourses> Teachers = new List<TeacherCourses>();
        public static List<WeekPeriods> Periods = new List<WeekPeriods>();
        public static List<Teachers> teachersFullInfo = new List<Teachers>();
        public static List<StudentGroup> allStudentGroups = new List<StudentGroup>();
        public static List<TeacherPeriods> teacherAvailability = new List<TeacherPeriods>();

        public static int maxConflicts = 0;
        public static int maxTime = 60;

        public static Random rng = new Random();

        //Number of periods in a week per class
        public static int periodNumber = 3;

        public static bool stopRequested = false;
        public static bool algoRun = false;

        public static List<ClassInfo> timetableStored;

        public static List<ClassInfo> timetableFromSQL;

        public static bool editAllowed = false;
    }

    public class StudentGroup
    {
        public int studentGroupID { get; set; }
        public int courseID { get; set; }
        public List<int> studentIDs { get; set; }

        public int groupCapacity;

        public StudentGroup(int groupID, int courseID, int groupCap)
        {
            studentGroupID = groupID;
            this.courseID = courseID;
            studentIDs = new List<int>();
            groupCapacity = groupCap;
        }

    }

    public class ClassInfo
    {
        public int courseID { get; set; }
        public int teacherID { get; set; }
        public int roomID { get; set; }

        public int[] periodID = new int[GenGlobals.periodNumber];
        public StudentGroup groupPopulation { get; set; }
    }

    public struct TeacherRank
    {
        public int teacherID;
        public int conflictCount;
        public int assignedCount;

        public TeacherRank(int teacherID, int conflictCount, int assignedCount)
        {
            this.teacherID = teacherID;
            this.conflictCount = conflictCount;
            this.assignedCount = assignedCount;
        }
    }

    public struct RoomRank
    {
        public int roomID;
        public int conflictCount;
        public int assignedCount;

        public RoomRank(int roomID, int conflictCount, int assignedCount)
        {
            this.roomID = roomID;
            this.conflictCount = conflictCount;
            this.assignedCount = assignedCount;
        }
    }

    public struct PeriodRank
    {
        public int periodID;
        public string dayOfWeek;
        public int Usage;


        public PeriodRank(int periodID, string dayOfWeek, int Usage)
        {
            this.periodID = periodID;
            this.dayOfWeek = dayOfWeek;
            this.Usage = Usage;
        }
    }


}
