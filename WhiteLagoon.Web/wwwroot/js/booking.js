var dataTable;


$(document).ready(function () {
loadDataTable();    

});


function loadDataTable() {
    dataTable = $('#tblBookings').DataTable({
        "ajax": {
            url: '/booking/getall'
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "5%" },
            { data: 'phone', "width": "5%" },
            { data: 'email', "width": "5%" },
            { data: 'status', "width": "5%" },
            { data: 'checkindate', "width": "5%" },
            { data: 'nights', "width": "5%" }
        ]
    });
}