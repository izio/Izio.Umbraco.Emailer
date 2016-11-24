angular.module("umbraco")
    .controller("Emailer.PickerController",
        function ($scope, $http) {
            $http.get("/umbraco/backoffice/api/EmailerApi/GetAll/").success(function (data) {
                $scope.forms = data;
            }).error(function () {
                
            });
        });