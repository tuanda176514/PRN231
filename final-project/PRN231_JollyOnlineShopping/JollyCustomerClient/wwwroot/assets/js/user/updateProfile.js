if (window.location.href.includes("updateProfileSuccess")) {
    swalSuccess("Cập nhật thông tin thành công!").then(() => {
        window.location.href = "/Home";
    });
}
if (window.location.href.includes("updateAddressSuccess")) {
    var userId = $("#user-info").data("userid");
    swalSuccess("Cập nhật địa chỉ thành công!").then(() => {
        window.location.href = "/User/MyProfile?id=" + userId;
    });
}
if (window.location.href.includes("deleteAddressSuccess")) {
    var userId = $("#user-info").data("userid");
    swalSuccess("Xoá địa chỉ thành công!").then(() => {
        window.location.href = "/User/MyProfile?id=" + userId;
    });
}
const deleteAddress = (id, userId) => {
    swalConfirm("Bạn có chắc chắc xoá địa chỉ này không?").then((t) => {
        if (t.isConfirmed) {
            window.location.href = `/User/DeleteAddress?id=${id}&userId=${userId}`;
        }
    });
}
