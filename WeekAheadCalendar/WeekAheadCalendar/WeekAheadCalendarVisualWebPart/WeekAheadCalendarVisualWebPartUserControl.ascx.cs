using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace WeekAheadCalendar.WeekAheadCalendarVisualWebPart
{
    public partial class WeekAheadCalendarVisualWebPartUserControl : UserControl
    {
        public WeekAheadCalendarVisualWebPart MainVisualWebPart { get; set; }
        private DateTime _beginWeek;
        private DateTime _endWeek;
        private string ListName;
        private bool DebugMode;  

        protected void Page_Load(object sender, EventArgs e)
        {
            TryLoadParam();
            if (!IsPostBack)
            {
                SelectedDateHiddenField.Value = DateTime.Now.ToShortDateString();
                WeekAheadDataBind();
            }
            
        }
        private void WeekAheadDataBind()
        {
            DateTime dateForWeek;
            DateTime.TryParse(SelectedDateHiddenField.Value, out dateForWeek);
            if (dateForWeek == new DateTime())
                dateForWeek = DateTime.Now;

            SetBeginEndWeek(dateForWeek);
            SelectedDate.Text = _beginWeek.ToShortDateString() + " - " + _endWeek.ToShortDateString();
            List<string> week = GetDataFromEventList();
            SelectedWeekGridView.DataSource = ToHorizontalOneRowDataTable(week, _beginWeek);

            SelectedWeekGridView.DataBind();
        }

        private List<string> GetDataFromEventList()
        {
              SPQuery query = new SPQuery();
            query.ViewFields = String.Concat(
                "<FieldRef Name='Title' />",
                "<FieldRef Name='Location' />",

                
                "<FieldRef Name='EventDate' />",
                "<FieldRef Name='EndDate' />"//,
                );
            List<SPListItem> items = new List<SPListItem>();
            try
            {
                
                items = new SpConfig(ListName).ListSp.GetItems(query).Cast<SPListItem>().ToList();
            }
            catch (Exception ee)
            {
                ShowError("Error(get SPListItem):" + ee.Message);
            }
            List<string> result = new List<string>();

            foreach (DateTime day in EachDay(_beginWeek, _endWeek))
            {
                List<EventItem> line = items.Where(p => ObjectToDateTime(p["EventDate"]).Date <= day.Date && day.Date <= ObjectToDateTime(p["EndDate"]).Date)
                    .Select(p => new EventItem()
                     {
                         Title= p["Title"].ToString(),
                         LinkTitle = p.ParentList.DefaultDisplayFormUrl + "?ID="+p.ID  //p["LinkTitle"].ToString()
                        }).ToList();
                var sumline = "<div>";
                var i = 0;
                var isExpand = false;
                foreach (var oneLien in line)
                {
                    if (i == 4)
                    {
                        sumline += "<div>";
                        isExpand = true;
                    }
                    sumline += String.Format(" <div><a href=\"{1}\">{0}</a> </div>", oneLien.Title, oneLien.LinkTitle);
                    i++;
                }
                if (isExpand)
                {
                    sumline += "</div><a class=\"collapse-day-items-weekahead\">"+(i-4).ToString()+" more items</a>";
                }
                sumline += "</div>";
                result.Add(sumline);
            }
               
            return result;
        }
        private void TryLoadParam()
        {
            try
            {
                ListName = MainVisualWebPart.ListName;
                DebugMode = MainVisualWebPart.DebugMode;
                

                
            }
            catch (Exception ee)
            {
                ShowError("Error(Users Properties are missing):" + ee.Message);
            }
            
        }

        private void SetBeginEndWeek(DateTime dateForWeek)
        {
            _beginWeek = dateForWeek.BeginOfWeek(DayOfWeek.Sunday);
            _endWeek = dateForWeek.EndOfWeek(DayOfWeek.Saturday);
        }
        protected void NextWeekButton_Click(object sender, EventArgs e)
        {
            DateTime dt;
            DateTime.TryParse(SelectedDateHiddenField.Value, out dt);
            if (dt == new DateTime())
                dt = DateTime.Now;
            SelectedDateHiddenField.Value = dt.AddDays(7).ToShortDateString();

            WeekAheadDataBind();
        }
        protected void PreviousWeekButton_Click(object sender, EventArgs e)
        {
            DateTime dt;
            DateTime.TryParse(SelectedDateHiddenField.Value, out dt);
            if (dt == new DateTime())
                dt = DateTime.Now;
            SelectedDateHiddenField.Value = dt.AddDays(-7).ToShortDateString();

            WeekAheadDataBind();
        }
        private void ShowError(string mes)
        {
            if (DebugMode)
            {
                var label = new Label();
                label.Text = mes;
                label.ForeColor = System.Drawing.Color.Red;

                this.Controls.Add(label);
                this.Controls.Add(new LiteralControl("<br />"));
            }

        }
        public DataTable ToHorizontalOneRowDataTable( IList<string> dataList,DateTime date)
        {
            
            DataTable table = new DataTable();
           // foreach (PropertyDescriptor prop in properties)
                for (int i = 0; i < dataList.Count; i++)
                {
                    table.Columns.Add(date.AddDays(i).Day.ToString());
                }
                
            
                DataRow row = table.NewRow();
                for (int i = 0; i < dataList.Count; i++)
                {
                    row[date.AddDays(i).Day.ToString()] =dataList[i];
                }
               

                table.Rows.Add(row);
           
            return table;
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        private DateTime ObjectToDateTime(object o)
        {
            try
            {
                var date = o is DateTime ? (DateTime)o : DateTime.Now.AddDays(-50);
                return date;
            }
            catch (Exception ee)
            {
                ShowError("Error(get date Time):" + ee.Message);
                return DateTime.Now.AddDays(-50);
            }
        }

        protected void SelectedWeekGridView_PreRender(object sender, EventArgs e)
        {
          
            for (int i = 0; i < SelectedWeekGridView.Rows.Count; i++)
            {
                for (int j = 0; j < SelectedWeekGridView.Rows[i].Cells.Count; j++)
                {
                    string encoded = SelectedWeekGridView.Rows[i].Cells[j].Text;
                    SelectedWeekGridView.Rows[i].Cells[j].Text = Context.Server.HtmlDecode(encoded);
                }
            }
        }
    }

    internal class EventItem    
    {
        public string Title { get; set; }         
        public string Location { get; set; }
        public string LinkTitle { get; set; }    
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }   
    }
    

    //extension method
    public static class DateTimeExtensions
    {
        public static DateTime BeginOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = endOfWeek- dt.DayOfWeek ;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(1 * diff).Date;
        }
    }

    public static class Extentions
    {
        public static DataTable ToHorizontalDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }
            return table;
        }
        
    }
    public class SpConfig
    {
        public SPWeb Web { get; set; }
        public SPList ListSp { get; set; }
        //public SPListItemCollection ListItemCollection { get; set; }
        public SpConfig()
        {
            Web = SPContext.Current.Web;
        }
        public SpConfig(string listname)
        {
            Web = SPContext.Current.Web;
            ListSp = Web.Lists[listname];
        }
    }

}
