var table = null;
$(document).ready(function () {
    table = $('#TB_Department').DataTable({
        "ajax": {
            url: "http://localhost:8082/api/Departments",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            headers: {
                "Authorization": "Bearer " + sessionStorage.getItem("tokenJWT")
            },
            /*success: function (result) {
                console.log(result)
            }*/
        },
        
        "columns": [
            {
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1 + "."
                }
            },
            { "data": "name" },
            {
                // Menambahkan kolom "Action" berisi tombol "Edit" dan "Delete" dengan Bootstrap
                "data": null,
                "render": function (data, type, row) {
                    var modalId = "modal-edit-" + data.id;
                    var deleteId = "modal-delete-" + data.id;
                    return '<button class="btn btn-warning " data-placement="left" data-toggle="modal" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')"><i class="fa fa-edit"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="modal" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"><i class="fa fa-trash"></i></button >'
                }
            }
        ],

        "order": [[1, "asc"]],
        //"responsive": true,
        //Buat ngilangin order kolom No dan Action
        "columnDefs": [
            {
                "targets": [0, 2],
                "orderable": false
            }
        ],
        //Agar nomor tidak berubah
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            api.column(1, { page: 'current' }).data().each(function (group, i) {
                $(rows).eq(i).find('td:first').html(i + 1);
            });
        }
    })
})

function Save() {
    var Department = new Object(); //object baru
    Department.Name = $('#Name').val(); //value insert dari id pada input
    $.ajax({
        type: 'POST',
        url: 'http://localhost:8082/api/Departments',
        data: JSON.stringify(Department),
        contentType: "application/json; charset=utf-8",
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("tokenJWT")
        },
    }).then((result) => {
        //debugger;
        if (result.status == 200) {
            /*alert(result.message);
            $('#TB_Department').DataTable().ajax.reload();*/
            Swal.fire({
                icon: 'success',
                title: 'Success...',
                text: 'Data has been added!',
                showConfirmButtom: false,
                timer: 1500
            })
            //$('#Modal').modal('hide');
            table.ajax.reload();
        }
        else {
            Swal.fire({
                icon: 'warning',
                title: 'Data Gagal dimasukkan!',
                showConfirmButtom: false,
                timer: 1500
            })
            $('#Modal').modal('hide');
            table.ajax.reload();
        }
    })
}

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Update').hide();
    $('#Save').show();
}

function GetById(id) {
    //debugger;
    $.ajax({
        url: "http://localhost:8082/api/Departments/" + id,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("tokenJWT")
        },
        success: function (result) {
            //debugger;
            var obj = result.data; //data yg kita dapat dr API  
            $('#Id').val(obj.id);
            $('#Name').val(obj.name);
            $('#Modal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}

function Update() {
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.Name = $('#Name').val();
    //debugger;
    $.ajax({
        url: 'http://localhost:8082/api/Departments',
        type: 'PUT',
        data: JSON.stringify(Department),
        contentType: "application/json; charset=utf-8",
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("tokenJWT")
        },
    }).then(result => {
        //debugger;
        if (result.status == 200) {
            Swal.fire({
                icon: 'success',
                title: 'Success...',
                text: 'Data has been update!',
                showConfirmButtom: false,
                timer: 2000
            })
            $('#Modal').modal('hide');
            table.ajax.reload();
        }
        else {
            alert("Data gagal Diperbaharui");
        }
    });
}

function Delete(Id) {
    //debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "http://localhost:8082/api/Departments/" + Id,
                type: "DELETE",
                dataType: "json",

                headers: {
                    "Authorization": "Bearer " + sessionStorage.getItem("tokenJWT")
                },
            }).then((result) => {
                //debugger;
                if (result.status == 200) {
                    Swal.fire(
                        'Deleted!',
                        'Your data has been deleted.',
                        'success'
                    )
                    table.ajax.reload();
                }
                else {
                    Swal.fire(
                        'Error!',
                        result.message,
                        'error'
                    )
                }
            });
        }
    })
}


/*function Delete(Id) {
    //debugger;
    $.ajax({
        url: "http://localhost:8082/api/Departments/" + Id,
        type: "DELETE",
        dataType: "json",
    }).then((result) => {
        debugger;
        if (result.status == 200) {
            *//*alert(result.message);
            $('#TB_Department').DataTable().ajax.reload();*//*
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.value) {
                    form.submit();   
                }
            })
            $('#Modal').modal('hide');
            $('#TB_Department').DataTable().ajax.reload();
        }
        else {
            alert(result.message);
        }
    });
}*/

const closeButton = document.querySelector('.btn.btn-danger[data-dismiss="modal"]');
closeButton.addEventListener('click', function () {
    // kode untuk menutup modal
});





