var travelsController = function () {

    var url = "http://localhost/jtapi/api/travels/{depName}";

    function getTrip(id, callback)
    {
        $.ajax({
            url: url + id,
            type: "GET",
            contentType: "application/json"
        })
            .done(function (data, textStatus, jqXhr) {
                var response = {
                    status: jqXhr.status,
                    firstName: data.FirstName
                };
                callback(response);
            })
            .fail(function (data) {
                var response = { status: data.status };
                callback(response);
            });
        return {
            get: getTrip
        };
    } ();