$(document).ready(function () {
    $('#login-form').submit(function (e) {
        e.preventDefault();
        var Account = new Object();
        Account.Email = $('#Email').val();
        Account.Password = $('#Password').val();
        debugger;
        $.ajax({
            type: 'POST',
            url: 'http://localhost:8082/api/Accounts/Login',
            data: JSON.stringify(Account), //convert json
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                debugger;

                var gettoken = result.token;
                sessionStorage.setItem("tokenJWT", gettoken)

                $.post("/Home/Login", { email: Account.Email })
                    .done(function () {
                        Swal.fire({
                            icon: 'success',
                            title: result.message,
                            showConfirmButton: false,
                            timer: 2500
                        }).then((successAllert) => {
                            if (successAllert) {
                                location.replace("/Departments/Index");
                            } else {
                                location.replace("/Departments/Index");
                            }

                        });
                    })
                    .fail(function () {
                        alert("Fail!, Gagal Login");
                    })
                    .always(function () {
                        //alert
                    });
            },
            error: function (errorMessage) {
                Swal.fire('Gagal Login', errorMessage.message, 'error');
            }
        });
    })
});

$('#logoutForm').on("click", function () {
    sessionStorage.removeItem('tokenJWT');
    // redirect ke halaman login
    location.replace("/Home/Login");
});
