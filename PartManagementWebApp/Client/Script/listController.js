(function(app) {
    var listController = function($scope, partService) {
        partService
            .getAll()
            .success(function(data) {
                $scope.parts = data.value;
            });
        $scope.create = function() {
             $scope.edit = {
                  part: {
                       PartNumber: ""
                  }
             };
        };
        $scope.delete = function(part) {
            partService
                .delete(part)
                .success(function() {
                    removePartById(part.Id);
                });
        };
        var removePartById = function(id) {
            for (var i = 0; i < $scope.parts.length; i++) {
                if ($scope.parts[i].Id == id) {
                    $scope.parts.splice(i, 1);
                    break;
                }
            }
        };
    };
    listController.$inject = ["$scope", "partService"];
    app.controller("listController", listController);
}(angular.module("partManagementApp")));
