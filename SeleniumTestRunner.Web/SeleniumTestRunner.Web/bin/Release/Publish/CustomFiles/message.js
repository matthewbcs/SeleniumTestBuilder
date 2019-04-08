$(function() {
    
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
       // $scope.StepsList[response.StepIndexPos].DidPass = response.DidPass;
        //$scope.StepsList[response.StepIndexPos].DidFail = response.DidFail;
        //$scope.$digest();
        response = null;
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
       
    });
});