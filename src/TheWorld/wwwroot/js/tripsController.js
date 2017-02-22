(function () {
    "use strict";

    //Getting the existing module
    angular.module("app-trips")
    .controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this;

        vm.trips = [];

        vm.newTrip = {};
        
        vm.errorMessage = "";
        vm.isBusy = true; //This represents if we're doing an operation, this starts out as true because we are always going to start with an operation

        //Use our api and angularjs' 'http' to retrieve the trips. 'get' is indicative of the 'get' in our trips api controller
        $http.get("/api/trips")
            .then(function (response) { //response is the response from our api call
                //Success
                angular.copy(response.data, vm.trips); //populate our vm.trips with the data from the response
            }, function (error) {
                //Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
        .finally(function () {
            vm.isBusy = false; //Finally once the above is over, whether successful or not - set the busy flag to false.
        });

        vm.addTrip = function () {
            //First show the isBusy flag when adding a new trip
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
                .then(function (response) {
                    //success
                    vm.trips.push(response.data); //Push the data to our response
                    vm.newTrip = {};
                }, function () {
                    vm.errorMessage = "Failed to save new trip";
                })
            .finally(function () {
                vm.isBusy = false;
            });
        };
    }

})();