(function() {
    var app = angular.module("partManagementApp", ["ngRoute"]);
    var config = function($routeProvider) {
        $routeProvider            
            .when("/list",                
                { templateUrl: "/client/view/list.html" })            
            .when("/details/:id",                
                { templateUrl: "/client/view/details.html" })            
            .otherwise(
                { redirectTo: "/list" });
    };
    config.$inject = ["$routeProvider"];
    app.config(config);
    app.constant("partApiUrl", "odata/Parts/");
}());