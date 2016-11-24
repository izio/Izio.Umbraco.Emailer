angular.module("umbraco")
    .controller("Emailer.CreateController",
        function ($scope, $routeParams, $http, $location, notificationsService, navigationService) {

            $scope.loaded = true;
            $scope.form = {};

            $scope.cancel = function () {
                $scope.create.$dirty = false;
                navigationService.hideMenu();
            };

            $scope.save = function (form) {

                form.SubmissionLimit = 10;
                form.ConfirmationMessage = "Thank you for your email.";
                form.TemplateSubject = "[subject]";
                form.TemplateBody = "[body]";

                $http.post("/umbraco/backoffice/api/EmailerApi/Save", form).success(function (data) {
                    navigationService.hideMenu();
                    navigationService.syncTree({ tree: "emailer", path: [-1, -1], forceReload: true });
                    notificationsService.success("Success, the form " + form.Name + " has been saved");

                    $scope.create.$dirty = false;
                    $location.url("/izioEmailer/emailer/edit/" + data.Id);
                }).error(function () {
                    notificationsService.error("Error", "Failed to create new emailer form, please try again later / refresh page");
                });
            };
        })
    .controller("Emailer.EditController",
	    function ($scope, $routeParams, $http, notificationsService, navigationService) {

	        $scope.tabs = [{ id: 1, label: "Settings" }, { id: 2, label: "Message Template" }, { id: 3, label: "Auto Responder" }];
	        $scope.ConfirmationMessage = {
	            label: "bodyText",
	            view: "rte",
	            config: {
	                editor: {
	                    toolbar: ["code", "undo", "redo", "cut", "styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "link", "umbmediapicker", "umbmacro", "table", "umbembeddialog"],
	                    stylesheets: [],
	                    dimensions: { height: 400 }
	                }
	            }
	        }
	        $scope.TemplateBody = {
	            label: "bodyText",
	            view: "rte",
	            config: {
	                editor: {
	                    toolbar: ["code", "undo", "redo", "cut", "styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "link", "umbmediapicker", "umbmacro", "table", "umbembeddialog"],
	                    stylesheets: [],
	                    dimensions: { height: 400 }
	                }
	            }
	        }
	        $scope.ResponderBody = {
	            label: "bodyText",
	            view: "rte",
	            config: {
	                editor: {
	                    toolbar: ["code", "undo", "redo", "cut", "styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "link", "umbmediapicker", "umbmacro", "table", "umbembeddialog"],
	                    stylesheets: [],
	                    dimensions: { height: 400 }
	                }
	            }
	        }
	        $scope.loaded = false;

	        $http.get("/umbraco/backoffice/api/EmailerApi/GetById/" + $routeParams.id).success(function (data) {
	            $scope.form = {
	                Name: data.Name,
	                Reference: data.Reference,
	                DestinationAddress: data.DestinationAddress,
	                SubmissionLimit: data.SubmissionLimit,
                    ConfirmationMessage: data.ConfirmationMessage,
	                TemplateSubject: data.TemplateSubject,
	                ResponderEnabled: data.ResponderEnabled,
	                ResponderAddress: data.ResponderAddress,
	                ResponderSubject: data.ResponderSubject
	            };

	            $scope.ConfirmationMessage.value = data.ConfirmationMessage;
	            $scope.TemplateBody.value = data.TemplateBody;
	            $scope.ResponderBody.value = data.ResponderBody;

	            $scope.loaded = true;

	        }).error(function () {
	            notificationsService.error("Error", "Failed to load emailer form, please try again later / refresh page");
	        });

	        $scope.cancel = function () {
	            $scope.create.$dirty = false;
	            navigationService.hideMenu();
	        };

	        $scope.save = function (form) {

	            form.Id = $routeParams.id;
	            form.ConfirmationMessage = $scope.ConfirmationMessage.value;
	            form.ResponderBody = $scope.ResponderBody.value;
	            form.TemplateBody = $scope.TemplateBody.value;
	            form.IsDirty = true;

	            $http.post("/umbraco/backoffice/api/EmailerApi/Save", form).success(function () {
	                navigationService.hideMenu();
	                navigationService.syncTree({ tree: "emailer", path: [-1, -1], forceReload: true });
	                notificationsService.success("Success, the form " + form.Name + " has been saved");

	                $scope.edit.$dirty = false;
	            }).error(function () {
	                notificationsService.error("Error", "Failed to save emailer form, please try again later / refresh page");
	            });
	        };
	    })
    .controller("Emailer.DeleteController",
        function ($scope, $routeParams, $http, $location, notificationsService, navigationService) {

            $scope.loaded = true;

            $scope.cancel = function () {
                navigationService.hideMenu();
            };

            $scope.delete = function (id) {
                $http.post("/umbraco/backoffice/api/EmailerApi/Delete/"+ id).success(function () {
                    navigationService.hideMenu();
                    navigationService.syncTree({ tree: "emailer", path: [-1, -1], forceReload: true });
                    notificationsService.success("Success, the form has been deleted");
                    $location.url("/izioEmailer/emailer/");
                }).error(function () {
                    notificationsService.error("Error", "Failed to delete emailer form, please try again later / refresh page");
                });
            };
        })