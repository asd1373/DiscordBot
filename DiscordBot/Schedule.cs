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
        public string dayName { get; set; }
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
        public static string WEEKDAY = "SELECT WEEKDAY(CURRENT_DATE());";

        //public static string sql_SchedDay = " SELECT t.`date`, t.timeStart, t.timeStop, t.discipline, t.cabinet, t.teacher, t.`type`, t.subgroup FROM timetable AS t  " +
        //                            " WHERE(subgroup = 0 || subgroup = 2) && class = 'АИСТбд-11' && `date` = CURDATE() + @sql_date;                              ";

        public string sql_SelectScheduleDay = "SELECT DAYNAME(t.date) 'dayName', t.`date`, t.timeStart, t.timeStop, t.discipline, t.cabinet, t.teacher, t.`type`, t.subgroup FROM timetable AS t" +
                                              " WHERE(subgroup = 0  || subgroup=1 || subgroup = 2) && class = @sql_group && `date` = (SELECT DATE_ADD(DATE(NOW() ) , INTERVAL -WEEKDAY(NOW() ) DAY ))+@sql_date";

                                   
    }
}
