
const notifyComment = () => {
    swalInfo('Bạn cần đăng nhập để bình luận, đăng nhập ngay bây giờ!').then(() => {
        window.location.href = "/User/Login";
    })
}
if (window.location.href.includes("success")) {
    toastSuccess("Bình luận thành công!");
}