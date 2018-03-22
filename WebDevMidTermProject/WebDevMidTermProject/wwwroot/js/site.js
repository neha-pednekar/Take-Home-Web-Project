// Write your JavaScript code.

$(document).ready(function () {

            $("#customers").on("click", ".js-delete", function () {
                var button = $(this);

                bootbox.confirm("Are you sure you want to delete this customer?", function (result) {
                    if (result) {
                        $.ajax({
                            url: "/api/customers/" + button.attr("data-customer-id"),
                            method: "DELETE",
                            success: function () {
                                table.row(button.parents("tr")).remove().draw();
                            }
                        });
                    }
                });
            });
        });
