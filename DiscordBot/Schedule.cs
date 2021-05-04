using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otchetnost
{
    public class Schedule
    {
        public DateTime date { get; set; }
        public TimeSpan timeStart { get; set; }
        public TimeSpan timeStop { get; set; }
        public string discipline { get; set; }
        public string cabinet { get; set; }
        public string teacher { get; set; }
        public string type { get; set; }
        public string subgroup { get; set; }
    }

    public class SqlSchedule
    {
        public string sql_SelectScheduleDay = "SELECT t.`date`, t.timeStart, t.timeStop, t.discipline, t.cabinet, t.teacher, t.`type`, t.subgroup FROM timetable AS t" +
                                              " WHERE(subgroup = 0 || subgroup = 2) && class = @sql_group && `date` = (SELECT DATE_ADD(DATE(NOW() ) , INTERVAL -WEEKDAY(NOW() ) DAY ))+@sql_date";
    }
}
