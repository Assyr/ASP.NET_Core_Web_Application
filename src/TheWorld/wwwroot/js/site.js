// site.js
(function () {

    //var ele = $("#username");
    //ele.text("John Chang");

    //var main = $("#main");

    ////on mouse enter main
    //main.on("mouseenter", function () {
    //    main.style.backgroundColor = "#888";
    //});

    ////on mouse leave main
    //main.on("mouseleave", function () {
    //    main.style.backgroundColor = "";
    //});

    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function () {
    //    var me = $(this); //Grab the object that called this function
    //    alert(me.text());
    //})

    var $sidebarAndWrapper = $("#sidebar, #wrapper");
    var $icon = $("#sidebarToggle i.fa"); //Go and grab the 'fa' class part of 'i' in our toggleMenu id
    //Now we have a direct reference to the text/font in togleMenu
    //We can use it below when checking the text

    $("#sidebarToggle").on("click", function () { //On click of our sidebarToggle
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.removeClass("fa-angle-right");
            $icon.addClass("fa-angle-left");
        }
    });
})();