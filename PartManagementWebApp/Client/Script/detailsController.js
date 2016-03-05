(function(app) {
    var detailsController = function($scope, $routeParams, partService) {
        var id = $routeParams.id;
        partService
            .getById(id)
            .success(function(data) {
                $scope.part = data;
            });
        $scope.edit = function() {
            $scope.edit.part = angular.copy($scope.part);
        };
    };
    detailsController.$inject = ["$scope", "$routeParams", "partService"];
    app.controller("detailsController", detailsController);
}(angular.module("partManagementApp")));