
var app = angular.module('myapp', ['psi.sortable']);
 
app.controller('ctrl', ['$scope', '$http', function ($scope, $http) {


    var employeeDetails = {};
  
  

    // to and then from  - used to put controller data into scope so can use in angular 
    $.extend($scope, window.controllerData.TestBuilderModel);

    $scope.addStep = function () {
        var step = {
            StepType: $scope.SelectedStep.StepType,
            StepTypeLabel:  $scope.SelectedStep.StepTypeLabel ,
            StepTypeLabelFull: $scope.SelectedStep.StepTypeLabelFull,
            StepLabel: $scope.SelectedStep.StepLabel,
            StepItemCode: $scope.SelectedStep.StepItemCode,
            StepParams: $scope.SelectedStep.StepParams
        };

        // handle params 
        var params = [];
        for (var i = 0; i < $scope.SelectedStep.StepParams.length; i++) {
            var p = {
                ParamLabel: $scope.SelectedStep.StepParams[i].ParamLabel,
                ParamValue: $scope.SelectedStep.StepParams[i].ParamValue,
            };
            params.push(p);
        }
       
        step.StepParams = params;

        $scope.StepsList.push(step);
    };

    $scope.RunTest = function () {

        var postData = {};
        postData.StepsList = $scope.StepsList;
 

        // check if any steps 
        if (postData.StepsList.length < 1 ) {
            alert("No Steps to run");
            return;
        }


        $http.post('/Home/RunSeleniumTest', postData).then(
            function (response) {
                
                alert(response.data.Message);
                if (response.data.WasSucess === true) {
                    window.location.reload();
                }
                
            }
        );
    };

}]);