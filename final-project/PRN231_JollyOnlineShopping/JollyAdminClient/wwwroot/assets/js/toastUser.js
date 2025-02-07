if (window.location.href.includes("successCreate")) {
    swalSuccess("Tạo tài khoản thành công!").then(() => {
        window.location.href = "/User";
    });
}
if (window.location.href.includes("successUpdate")) {
    swalSuccess("Cập nhật tài khoản thành công!").then(() => {
        window.location.href = "/User";
    });
}
if (window.location.href.includes("errCreate")) {
    swalFailed("Mail đã tồn tại, xin vui lòng kiểm tra lại!").then(() => {
        window.location.href = "/User/Create";
    });
}
if (window.location.href.includes("successChangeStatus")) {
    swalSuccess("Chuyển đổi trạng thái người dùng thành công!").then(() => {
        window.location.href = "/User"
    });
}
const changeStatus = (id) => {
    swalConfirm("Bạn có chắc chắc đổi trạng thái người dùng này không?").then((t) => {
        if (t.isConfirmed) {
            window.location.href = `/User/ChangeStatus?id=${id}`;
        }
    });
}