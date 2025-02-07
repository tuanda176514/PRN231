if (window.location.href.includes("successRegister")) {
    swalSuccess("Đăng kí tài khoản thành công!").then(() => {
        window.location.href = "/User/Login";
    });
}
if (window.location.href.includes("loginError")) {
    swalFailed("Đăng nhập thất bại kiểm tra lại tài khoản hoặc mật khẩu của bạn");
}
if (window.location.href.includes("loginErrorStatus")) {
    swalFailed("Tài khoản của bạn chưa được kích hoạt vui lòng kiểm tra lại!");
}
if (window.location.href.includes("errorRegister")) {
    swalFailed("Đăng kí thất bại Email đã được sử dụng");
} 