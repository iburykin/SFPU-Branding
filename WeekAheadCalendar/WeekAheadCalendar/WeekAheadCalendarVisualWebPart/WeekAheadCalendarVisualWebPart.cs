using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace WeekAheadCalendar.WeekAheadCalendarVisualWebPart
{
    [ToolboxItemAttribute(false)]
    public class WeekAheadCalendarVisualWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WeekAheadCalendar/WeekAheadCalendarVisualWebPart/WeekAheadCalendarVisualWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            WeekAheadCalendarVisualWebPartUserControl control = (WeekAheadCalendarVisualWebPartUserControl)Page.LoadControl(_ascxPath);
            control.MainVisualWebPart = this;
            Controls.Add(control);
        }
        [WebBrowsable(true),
        WebDisplayName("Debug Mode"),
        Personalizable(PersonalizationScope.Shared),
        Category("Debug Mode"),
        DefaultValue(false)]
        public bool DebugMode { get; set; }

        [WebBrowsable(true),
        WebDisplayName("List Name"),
        Personalizable(PersonalizationScope.Shared),
        Category("Settings"),
        DefaultValue(false)]
        public string ListName { get; set; }
    }
}
