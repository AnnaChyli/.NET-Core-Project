/* site.js */

(function () {

	//get both divs for sidebar and wrapper
	var $siderbarAndWrapper = $("#sidebar, #wrapper");
	var $icon = $("#sidebarToggle i.fa");


	$("#sidebarToggle").on("click",
		function() {

			$siderbarAndWrapper.toggleClass("hide-sidebar");

			if ($siderbarAndWrapper.hasClass("hide-sidebar"))
			{
				$icon.removeClass("fa-angle-left");
				$icon.addClass("fa-angle-right");
			}
			else
			{
				$icon.addClass("fa-angle-left");
				$icon.removeClass("fa-angle-right");
			}
		});

})();


/*jQuery
	var elem = $("#username");
	elem.text("Anna Chyli");

	var mainV = $("#main");
	mainV.mouseenter(function() {
		mainV.css("background-color","yellow");
		});

	mainV.mouseleave(function() {
		mainV.css("background-color", "");
		});

	//get all anchors <a>
	var menuItems = $("ul.menu li a");
	menuItems.on("click",
		function() {
			var me = $(this);
			alert(me.text());
		});    
 */
/*
//JavaScript
var ele = document.getElementById("username");
ele.innerHTML = "Anna Chilikina";

var main = document.getElementById("main");

main.onmouseenter = function() {
	main.style.backgroundColor = "#888";
};

main.onmouseleave = function() {
	main.style.backgroundColor = "";
};
*/