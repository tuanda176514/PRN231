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

const swalConfirm = (text) => {
  return Swal.fire({
    text: `${text}`,
    icon: "warning",
    showCancelButton: true,
    buttonsStyling: false,
      confirmButtonText: "Xác nhận",
    cancelButtonText: "Hủy bỏ",
    customClass: {
      confirmButton: "btn btn-primary mr-3",
      cancelButton: "btn btn-secondary",
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

const toastSuccess = (text) => {
    new Toast({
        message: text,
        type: "success",
    });
} 

const toastWarning = (text) => {
    new Toast({
        message: text,
        type: "warning",
    });;
} 

const toastInfo = (text) => {
    new Toast({
        message: text,
        type: "info",
    });
}

const toastError = (text) => {
    new Toast({
        message: text,
        type: "danger",
    });
} 
