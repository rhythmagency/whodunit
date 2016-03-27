angular.module('umbraco').controller('Rhythm.WhodunitController', function($scope, $http) {
    
    $scope.getHistory = function() {
        var hiddenElement = document.createElement('a');
        hiddenElement.href = 'Whodunit/Whodunit/GetHistory';
        hiddenElement.click();
    }

})