const confirmShip = (pid, status) => {
  swalConfirm("Bạn có chắc chắn muốn xác nhận không?").then((result) => {
    if (result.isConfirmed) {
      window.location.href = `/order/Confirm?id=${pid}&status=${status}`;
    }
  });
};
