var table
if (window.location.href.endsWith('/order')) {
     table = $("#my_data_table").DataTable({
        "order": [[0, 'desc']] // This will sort by the first column in ascending order
        // Add other options as needed
    });
} else {
    table = $("#my_data_table").DataTable();
}
// #myInput is a <input type="text"> element
$('#filter-search').on('keyup', function () {
    table.search(this.value).draw();
});

