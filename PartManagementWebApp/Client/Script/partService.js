(function(app) {
    var partService = function ($http, partApiUrl) {
        var getAll = function() {
            return $http.get(partApiUrl);
        };
        var getById = function (id) {
            return $http.get(partApiUrl + "(" + id + ")");
        };
        var update = function (part) {
            return $http.put(partApiUrl + "(" + part.Id + ")", part);
        };
        var create = function (part) {
            return $http.post(partApiUrl, part);
        };
        var destroy = function(part) {
            return $http.delete(partApiUrl + "(" + part.Id + ")");
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            delete: destroy
        };
    };
    partService.$inject = ["$http", "partApiUrl"];
    app.factory("partService", partService);
}(angular.module("partManagementApp")));
