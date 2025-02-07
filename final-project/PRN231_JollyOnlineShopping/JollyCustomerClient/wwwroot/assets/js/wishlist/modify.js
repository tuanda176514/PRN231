const WISH_URL = "https://localhost:8888/api/wishList";

const CountWishlist = () => {
  var userId = $("#user-info").data("userid");
  if (userId) {
    $.ajax({
      url: `${WISH_URL}/TotalItem?uid=${userId}`,
      method: "GET",
      contentType: "application/json",
      success: function (response) {
        $("#wishlist-count").text(response);
      },
      error: function () {
        toastError("Lỗi hệ thống");
      },
    });
  }
};
const ModifyItem = (pid) => {
  var userId = $("#user-info").data("userid");
    if (userId) {
        return $.ajax({
            url: `${WISH_URL}/ModifyItem?uid=${userId}&pid=${pid}`,
            method: "POST",
            contentType: "application/json",
            success: function (response) {
                if (response) {
                    toastSuccess("Thêm vào danh sách yêu thích thành công");
                } else {
                    toastSuccess("Xóa khỏi danh sách yêu thích thành công");
                }
                CountWishlist();
            },
            error: function () {
                toastError("Lỗi hệ thống");
            },
        });
    } else {
        window.location.href = "/User/Login"  
    }
};

const AddToWishlist = (pid) => {
    
}

$(document).ready(() => {
  CountWishlist();
});
