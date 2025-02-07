const swalSuccess = (text) => {
  return Swal.fire({
    text: `${text}`,
    icon: "success",
    buttonsStyling: false,
    confirmButtonText: "Ok",
    customClass: {
      confirmButton: "btn btn-primary",
    },
  });
};

const swalFailed = (text) => {
  return Swal.fire({
    text: `${text}`,
    icon: "error",
    buttonsStyling: false,
    confirmButtonText: "Ok",
    customClass: {
      confirmButton: "btn btn-primary",
    },
  });
};

const swalInfo = (text) => {
    return Swal.fire({
        text: `${text}`,
        icon: "info",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-primary",
        },
    });
};

const swalConfirm = (text) => {
  return Swal.fire({
    text: `${text}`,
    icon: "warning",
    showCancelButton: true,
    buttonsStyling: false,
      confirmButtonText: "Xác nhận",
    cancelButtonText: "Hủy bỏ",
    customClass: {
      confirmButton: "btn btn-primary",
      cancelButton: "btn btn-secondary",
    },
  });
};

const demoConfirm = () => {
  swalConfirm("Demo confirm").then((t) => {
    if (t.isConfirmed) {
      {
        //do something
        alert("Demo something");
      }
    }
  });
};

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": false,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

const toastSuccess = (text) => {
    toastr["success"](text)
}

const toastWarning = (text) => {
    toastr["warning"](text)
}

const toastInfo = (text) => {
    toastr["info"](text)
}

const toastError = (text) => {
    toastr["error"](text)
}
