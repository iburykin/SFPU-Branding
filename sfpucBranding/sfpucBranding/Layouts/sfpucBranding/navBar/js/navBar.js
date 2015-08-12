
(function($){
$(document).ready(function(){

$(document).ready(function() {
  $("#cssmenu").menumaker({
    title: "Menu",
    format: "multitoggle"
  });

  $("#cssmenu").prepend("<div id='menu-line'></div>");

var foundActive = false, activeElement, linePosition = 0, menuLine = $("#cssmenu #menu-line"), lineWidth, defaultPosition, defaultWidth;

$("#cssmenu > ul > li").each(function() {
  if ($(this).hasClass('active')) {
    activeElement = $(this);
    foundActive = true;
  }
});

if (foundActive === false) {
  activeElement = $("#cssmenu > ul > li").first();
}

defaultWidth = lineWidth = activeElement.width();
defaultPosition = linePosition = activeElement.position().left;
var tt="0";
if (activeElement.is(':first-child')){
	var tt="1";  }


menuLine.css("width", lineWidth);
menuLine.css("left", linePosition);

//123
menuLine.addClass('hover1');
menuLine.removeClass('hover2');
menuLine.removeClass('hover3');

$("#cssmenu > ul > li").hover(function() {
  activeElement = $(this);
  lineWidth = activeElement.width();
  linePosition = activeElement.position().left;
  menuLine.css("width", lineWidth);
  menuLine.css("left", linePosition);
  if (activeElement.is(':first-child')){
  	menuLine.addClass('hover2');
  	menuLine.removeClass('hover3');
  	menuLine.removeClass('hover1');
  }
  
  
}, 
function() {
  menuLine.css("left", defaultPosition);
  menuLine.css("width", defaultWidth);
  if (tt=="1"){
  	menuLine.addClass('hover2');
  	menuLine.removeClass('hover3');
  	menuLine.removeClass('hover1');

  }
  if ($("#cssmenu > ul > li").is(':first-child')){
	menuLine.addClass('hover3');
  	menuLine.removeClass('hover1');
  	menuLine.removeClass('hover2');
  }
  
});

});


});
})(jQuery);
