//simpleControls.js
(function () { //Immediately invoked function
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor); //Create a directive called waitCursor that will use the 'waitCursor' function implementation / return

    //waitCursor definition
    function waitCursor() {
        return {
            scope: {
                show: "=displayWhen"
            },
            restrict: "E",
            templateUrl: "/views/waitCursor.html"
        };
    }
})();