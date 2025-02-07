$(document).ready(function () {
    var citis = document.getElementById("city");
    var districts = document.getElementById("district");
    var wards = document.getElementById("ward");

    if (!citis || !districts || !wards) {
        swalFailed("Hãy chọn địa chỉ của bạn!");
        btn_submitAddress.attr("disabled", false);
        return;
    }


    var Parameter = {
        url: "https://localhost:8888/apiCreateAddress/address.json",
        method: "GET",
        responseType: "application/json",
    };
    var data;

    var promise = axios(Parameter);
    promise.then(function (result) {
        data = result.data;
        renderCity(data);
    });

    function renderCity(data) {
        for (const x of data) {
            citis.options[citis.options.length] = new Option(x.Name, x.Id);
        }
        citis.onchange = function () {
            districts.length = 1;
            wards.length = 1;
            if (this.value != "") {
                const result = data.filter(n => n.Id === this.value);
                for (const k of result[0].Districts) {
                    districts.options[districts.options.length] = new Option(k.Name, k.Id);
                }
            }
        };
        districts.onchange = function () {
            wards.length = 1;
            const dataCity = data.filter((n) => n.Id === citis.value);
            if (this.value != "") {
                const dataWards = dataCity[0].Districts.find(n => n.Id === this.value)?.Wards;
                if (dataWards) {
                    for (const w of dataWards) {
                        wards.options[wards.options.length] = new Option(w.Name, w.Id);
                    }
                }
            }
        };
    }

    $("#form-createAddress").submit(function (e) {
        e.preventDefault();
        createAddress();
    });
    
    function createAddress() {
        var btn_submitAddress = $("#btn-submitAddress");
        btn_submitAddress.attr("disabled", true);
        var userId = $("#user-info").data("userid");

        var citisValue = $("#city").val();
        var districtsValue = $("#district").val();
        var wardsValue = $("#ward").val();

        if (!citisValue || !districtsValue || !wardsValue) {
            swalFailed("Hãy chọn địa chỉ của bạn!");
            btn_submitAddress.attr("disabled", false);
            return;
        }

        var cityName = mapIdToName(data, citisValue);
        var districtName = mapIdToName(data.find(n => n.Id === citisValue).Districts, districtsValue);
        var wardName = mapIdToName(data.find(n => n.Id === citisValue).Districts.find(n => n.Id === districtsValue).Wards, wardsValue);


        const requestData = {
            UserId: userId,
            City: cityName,
            District: districtName,
            Ward: wardName,
            Street: $('#street').val(),
        };
        console.log(requestData);

        $.ajax({
            url: `https://localhost:8888/users/address`,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(requestData),
            success: function (response) {
                swalSuccess("Tạo địa chỉ thành công!").then(() => {
                    window.location.href = "/User/MyProfile/" + userId;
                });
            },
            error: function () {
                btn_submitAddress.attr("disabled", false);
                toastError("Tạo địa chỉ thất bại!");
            },
        });
    }

    function mapIdToName(data, id) {
        const item = data.find(item => item.Id === id);
        return item ? item.Name : "";
    }
});
if (window.location.href.includes("errNullAddress")) {
    swalFailed("Bạn hãy tạo địa chỉ để có thể mua hàng!").then(() => {
    });
}





