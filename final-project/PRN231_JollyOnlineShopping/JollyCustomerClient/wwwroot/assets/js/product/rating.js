const notifyReview = () => {
  swalInfo("Bạn cần đăng nhập để đánh giá, đăng nhập ngay bây giờ!").then(
    () => {
      window.location.href = "/User/Login";
    }
  );
};

if (window.location.href.includes("success")) {
  toastSuccess("Đánh giá thành công");
}
