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

    // Parses a date, such as "2015-01-01".
    function parseDate(strDate) {
        var firstPos = strDate.indexOf("-");
        var secondPos = strDate.substring(firstPos + 1).indexOf("-") + firstPos + 1;
        var strYear = strDate.substring(0, firstPos);
        var strMonth = strDate.substring(firstPos + 1, secondPos);
        var strDay = strDate.substring(secondPos + 1);
        var year = parseInt(strYear, 10);
        var month = parseInt(strMonth, 10);
        var day = parseInt(strDay, 10);
        return new Date(Date.UTC(year, month - 1, day));
    }

    // Generates the CSV.
    function generateCsv($scope, $http) {
        $scope.showLink = false;
        $scope.disableGenerateButton = true;
        var strStartDate = $scope.startDate.value;
        var startDate = parseDate(strStartDate);
        var strEndDate = $scope.endDate.value;
        var endDate = parseDate(strEndDate);
        $http.post("/umbraco/backoffice/whodunit/whodunitapi/gethistory", {
            startDate: startDate,
            endDate: endDate
        }).success(function (data) {
            $scope.csvUrl = JSON.parse(data);
            $scope.showLink = true;
            $scope.disableGenerateButton = false;
        });
    }

    // ANGULAR COMPONENTS

    // dashboard controller function.
    const whodunitController = function ($scope, $routeParams, $http, whodunitResource) {

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

        // Scope functions.
        $scope.generateCsv = function () {
            generateCsv($scope, $http);
        };

    };

    // Register controller.
    angular.module("umbraco").controller("whodunitController", whodunitController);

})();