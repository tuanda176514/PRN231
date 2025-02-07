const API_URL = "https://localhost:8888/products";

const notifyCreateProduct = () => {
  try {
    fetch(`${API_URL}/create`, {
      method: "POST",
      body: new FormData(document.getElementById("employee_form")),
    })
      .then((response) => {
        if (response.ok) {
          swalSuccess("Sản phẩm đã được thêm thành công!").then(() => {
            window.location.href = "/product";
          });
        } else {
          return response.json();
        }
      })
      .then((errorData) => {
        if (errorData) {
          swalFailed("Ảnh phải có định dạng png, jpg, gif và ít nhất 4 ảnh");
        }
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  } catch (e) {
    console.error("Error:", e);
  }
};

const notifyEditProduct = (id) => {
  try {
    fetch(`${API_URL}/edit/${id}`, {
      method: "PUT",
      body: new FormData(document.getElementById("employee_form")),
    })
      .then((response) => {
        if (response.ok) {
          swalSuccess("Sản phẩm đã được sửa thành công!").then(() => {
            window.location.href = "/product";
          });
        } else {
          return response.json();
        }
      })
      .then((errorData) => {
        if (errorData) {
          swalFailed("Ảnh phải có định dạng png, jpg, gif và ít nhất 2 ảnh");
        }
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  } catch (e) {
    console.error("Error:", e);
  }
};

const notifyDeleteProduct = (id) => {
  swalConfirm("Bạn có chắc muốn xóa sản phẩm này?").then((result) => {
    if (result.isConfirmed) {
      try {
        fetch(`${API_URL}/delete/${id}`, {
          method: "DELETE",
        })
          .then((response) => {
            if (response.ok) {
              swalSuccess("Sản phẩm đã được xóa thành công!").then(() => {
                window.location.href = "/product";
              });
            } else {
              return response.json();
            }
          })
          .then((errorData) => {
            if (errorData) {
              swalFailed(`Xóa sản phẩm thất bại! ${errorData.Message}`);
            }
          })
          .catch((error) => {
            console.error("Error:", error);
          });
      } catch (e) {
        console.error("Error:", e);
      }
    }
  });
};

//function deleteImage() {
//    var id = $("#productId").val();
//    var uuid = $("#uuid").val();
//    if (swalConfirm("Bạn có chắc muốn xóa ảnh này?")) {
//        $("#btn-delete").attr("disabled", true);
//        $.ajax({
//            url: `${API_URL}/image/delete/${id}/${uuid}`,
//            method: "DELETE",
//            contentType: "application/json",
//            success: function () {
//                swalSuccess("Xóa ảnh thành công!");
//                window.location.href = `/Product/Edit/${id}`;
//            },
//            error: function () {
//                $("#btn-delete").attr("disabled", false);
//                swalFailed("Không thể xóa ảnh!");
//            },
//        });
//    }
//}

const deleteImage = () => {
  swalConfirm("Bạn có chắc muốn xóa ảnh này?").then((result) => {
    if (result.isConfirmed) {
      var id = $("#productId").val();
      var uuid = $("#uuid").val();
      $("#btn-delete").attr("disabled", true);
      $.ajax({
        url: `${API_URL}/image/delete/${id}/${uuid}`,
        method: "DELETE",
        contentType: "application/json",
        success: function () {
          swalSuccess("Xóa ảnh thành công!");
          window.location.href = `/Product/Edit/${id}`;
        },
        error: function () {
          $("#btn-delete").attr("disabled", false);
          swalFailed("Không thể xóa ảnh!");
        },
      });
    }
  });
};
