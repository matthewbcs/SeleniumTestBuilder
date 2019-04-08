
var app = angular.module('myapp', ['psi.sortable']);
 
app.controller('ctrl', ['$scope', '$http','$timeout', function ($scope, $http,$timeout) {


    var employeeDetails = {};

    // Declare a proxy to reference the hub. 
    var chat = $.connection.testMessageHub;

    // Create a function that the hub can call to broadcast messages.
    chat.client.broadcastMessage = function (name, message) {
        // Html encode display name and message. 
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page. 
        
        var response = JSON.parse(message);
        $('#discussion').append('<li>' + "index: " + response.StepIndexPos + "  - "+ response.StepTypeLabelFull + " - outcome info: Pass:" + response.DidPass  + "Fail: "+ response.DidFail  + " Fail Reason: " + response.FailMessage +"</li>");
        //$('#discussion').append('<li>' + encodedMsg + '</li>');
        $scope.StepsList[response.StepIndexPos].DidPass = response.DidPass;
        $scope.StepsList[response.StepIndexPos].DidFail = response.DidFail;
        $scope.$digest();
        response = null;
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
       
    });
  
  

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

    $scope.addSampleTest = function () {
        $scope.StepsList = $scope.SampleTest1StepsList;
    };
    $scope.Delete = function (itemvalue) {
        $scope.StepsList = $scope.StepsList.filter(item => item !== itemvalue);
    };
    $scope.RunTest = function () {

        var postData = {};
        postData.StepsList = $scope.StepsList;
 

        // check if any steps 
        if (postData.StepsList.length < 1 ) {
            alert("No Steps to run");
            return;
        }
        var didStepFail = false;
        //$scope.StepsList[i].IsRunning = false;
        $scope.TestRunning = true;

        $http.post('~/Home/RunSeleniumTest', postData).then(function(response) {

            $scope.TestRunning = false;

            //for (var i = 0; i < response.data.length; i++) {

            //    if (response.data[i].WasSuccess === true) {
            //        $scope.StepsList[i].DidPass = true;
            //        $scope.StepsList[i].DidFail = false;
            //    } else {
            //        $scope.StepsList[i].DidFail = true;
            //        $scope.StepsList[i].DidPass = false;
            //        $scope.StepsList[i].FailMessage = response.data[i].Message;
            //        didStepFail = true; // we want to end the test by breaking out
            //    }
            //}
        });

    };
    

  

  
    
   

}]);