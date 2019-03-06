angular.module('myapp', ['psi.sortable']);

function ctrl($scope) {
    setTimeout(function () {
        $scope.$apply(function () {

            var step = {
                stepType: '',
                stepDetail: '',
                param1: '',
                param2: '',
            };

           // $scope.list = ['first', 'second', 'third', 'last'];
           $scope.stepTypeList = ['Given', 'When', 'Then'];
            $scope.stepDetailList = ['I go to Url', 'I click on selector', 'this selector is visible on the page', ];
            $scope.list = [step];
        });

       

    }, 500);

    $scope.addStep = function () {
        var step = {
            stepType: '',
            stepDetail: '',
            param1: '',
            param2: '',
        };

        $scope.list.push(step);
    };
}