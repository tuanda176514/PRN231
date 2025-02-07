$(document).ready(function () {
  $("#my-form").on("submit", function (e) {
    e.preventDefault();
    var search = $("#p-name").val();
    window.location.href = `/product?name=${search}`;
  });
});
