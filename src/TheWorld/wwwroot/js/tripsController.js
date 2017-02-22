(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-trips")
    .controller("tripsController", tripsController);

    function tripsController() {

        var vm = this;

        vm.trips = [{
            name: "US Trip",
            created: new Date()
        }, {
            name: "World Trip",
            created: new Date()
        }];

        vm.newTrip = {};

        vm.addTrip = function () {
            vm.trips.push({ name: vm.newTrip.name, created: new Date() })
            //The above function pushes a new trip to our vm with the provided name and the current date
            vm.newTrip = {}; //Reset our newTrip model data to reset the data back in the form since the form is binded to newTrip
        };
    }

})();