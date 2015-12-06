<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeekAheadCalendarVisualWebPartUserControl.ascx.cs" Inherits="WeekAheadCalendar.WeekAheadCalendarVisualWebPart.WeekAheadCalendarVisualWebPartUserControl" %>

<style>
#WeekAheadCalendar {
	padding: 10px;
	margin: 0 auto;
}
#WeekAheadCalendar .title-weekahead {
	font-size: 20px;
    font-weight: bold;
    color: #0076BD;
}
#WeekAheadCalendar .selecteddate-weekahead {
	margin-left: 20px;
}
#WeekAheadCalendar input.nav-button {
	min-width: 19px;
	height: 19px;
	border: 0;
}
#WeekAheadCalendar .previous-weekahead  { 
	background: url("/_layouts/15/1033/images/calprev.png") no-repeat 50% 0;
}
#WeekAheadCalendar .next-weekahead  { 
	background: url("/_layouts/15/1033/images/calnext.png") no-repeat 50% 0;
	margin: 0;
}
#WeekAheadCalendar table.week-days {
	width: 100%;
	border: 0;
	margin-top: 15px;
	border-collapse: collapse;
	border: solid 1px transparent;
}
#WeekAheadCalendar table.week-days  td{
	width: 14.28%;
	text-transform: uppercase;
	font-weight: 400;
	max-width: 75px;
	padding: 1px 3px;
}
#WeekAheadCalendar table.selected-week-weekahead {
	width: 100%;
	margin-top: 3px;
	border-collapse: collapse;
}
#WeekAheadCalendar table.selected-week-weekahead  th {
	border: solid 1px #c6c6c6;
	border-bottom: 0;
	padding: 2px 3px;
	text-align: left;
}
#WeekAheadCalendar table.selected-week-weekahead  td {
	width: 14.28%;
	min-width: 14.28%;
	max-width: 75px;
	height: 64px;
	padding: 2px 3px;
	vertical-align: top;
	border: solid 1px #c6c6c6;
	border-top: 0;
	overflow-x: hidden;
	/*word-wrap: break-word;
	word-break: break-all;*/
}
#WeekAheadCalendar table.selected-week-weekahead  td  > div {
	width: auto;
	max-width: 75px;
	overflow: hidden;
	white-space: nowrap;
}
#WeekAheadCalendar table.selected-week-weekahead td > div > a {
	cursor: pointer;
}
#WeekAheadCalendar a {
    line-height: 1.3;
}

</style>
<div id="WeekAheadCalendar">
    <asp:HiddenField ID="SelectedDateHiddenField" runat="server" />
    <asp:Label ID="Title" runat="server" Text="Label" CssClass="title-weekahead">Week Ahead</asp:Label>
    <hr />
    <div class="nav-weekahead">
        <asp:Button ID="PreviousWeekButton" runat="server" Text="" CssClass="previous-weekahead nav-button" OnClick="PreviousWeekButton_Click"/>
        <asp:Button ID="NextWeekButton" runat="server" Text="" CssClass="next-weekahead nav-button" OnClick="NextWeekButton_Click"/>
        <asp:Label ID="SelectedDate" runat="server" Text="Label" CssClass="selecteddate-weekahead"></asp:Label>
    </div>
	<table class="week-days">
		<tr>
			<td>Sunday</td><td>Monday</td><td>Tuesday</td><td>Wednesday</td><td>Thursday</td><td>Friday</td><td>Saturday</td>
		</tr>
	</table>
    <div class="week-weekahead">
        <asp:GridView ID="SelectedWeekGridView" runat="server" CssClass="selected-week-weekahead" OnPreRender="SelectedWeekGridView_PreRender" >
            
        </asp:GridView>
    </div>

</div>
<script>

function toggleCollapsableDiv(e) {
	console.log($(e));
	
	if ( $(e).hasClass("collapse-day-items-weekahead") ) {
		$(e).next().text($(e).text());
		$(e).prev().removeClass("hidden");
		$(e).text("collapse");
		$(e).removeClass("collapse-day-items-weekahead");
		$(e).addClass("expand-day-items-weekahead");		
	} else {
		$(e).prev().addClass("hidden");
		$(e).text($(e).next().text());
		$(e).removeClass("expand-day-items-weekahead");
		$(e).addClass("collapse-day-items-weekahead");
	}
};

jQuery(document).ready(function ($) {
	collapsableDiv = $("#WeekAheadCalendar table.selected-week-weekahead td > div > div:nth-child(5)")
	toggle = $("#WeekAheadCalendar table.selected-week-weekahead td > div > a");
	toggle.after("<div class='hidden'></div>");
	
	collapsableDiv.addClass("hidden");
	$("#WeekAheadCalendar a.collapse-day-items-weekahead").attr("onClick", "toggleCollapsableDiv(this)");
});
</script>