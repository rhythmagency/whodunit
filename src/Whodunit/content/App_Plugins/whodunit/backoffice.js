(function () {

    // SUPPORT FUNCTIONS

    // Pads a number with leading zeros.
    function padNumber(num, zeros) {
        var strNum = num.toString();
        while (strNum.length < zeros) {
            strNum = "0" + strNum;
        }
        return strNum;
    }

    // Formats a date as a date string.
    function formatDate(date) {
        var parts = [
            date.getFullYear().toString(),
            padNumber(date.getMonth() + 1, 2),
            padNumber(date.getDate(), 2)
        ];
        var combined = parts.join("-");
        return combined;
    }



    // ANGULAR COMPONENTS

    // dashboard controller function.
    const whodunitController = function ($scope, $routeParams, whodunitResource) {

        // Variables.
        var now = (new Date(Date.now()));
        var yesterday = new Date(now);
        yesterday.setDate(yesterday.getDate() - 1);
        var strYesterday = formatDate(yesterday);
        var strToday = formatDate(now);
        var dateConfig = {
            pickDate: true,
            pickTime: false,
            useSeconds: false,
            format: "YYYY-MM-DD"
        };

        // Scope variables.
        $scope.showLink = false;
        $scope.csvUrl = "#";
        $scope.disableGenerateButton = false;

        // Scope models.
        $scope.startDate = {
            label: "Start Date",
            view: "datepicker",
            editor: "Umbraco.Date",
            value: strYesterday,
            config: dateConfig
        };
        $scope.endDate = {
            label: "End Date",
            view: "datepicker",
            editor: "Umbraco.Date",
            value: strToday,
            config: dateConfig
        };

    };
    angular.module("umbraco").controller("whodunitController", whodunitController);

})();