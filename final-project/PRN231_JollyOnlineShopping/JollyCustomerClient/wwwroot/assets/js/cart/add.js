const API_URL = "https://localhost:8888";
var shoppingCart = (function () {
    cart = [];

    function Item(productid, price, quantity, color, size) {
        this.productid = productid;
        this.price = price;
        this.quantity = quantity;
        this.color = color;
        this.size = size
    }
    $(document).ready(function () {
        $("#btn-submit").click(function (event) {
            event.preventDefault();
            addToCart(); 
        });
    });

    let selectedSize;

    $('#form-submit').on('submit', function (event) {
        event.preventDefault();

        swalConfirm("Bạn có chắc chắn muốn thanh toán không?").then((t) => {
            if (t.isConfirmed) {
                $('#form-submit').off('submit').submit();
            } else {
            }
        });
    });


    $('.swatch_pr_item').on('click', function () {
        const i = $(this).find('input[name="size"]').attr('id').split('_')[1];
        selectedSize = $('#size_' + i).val(); 
        console.log('Kích thước đã chọn: ' + selectedSize);
    });
    function addToCart() {
        const userId = $("#user-info").data("userid");
        var btn_submit = $("#btn-submit");
        btn_submit.attr("disabled", true);
        const productid = $('#productid').val();
        const quantity = $('#quantity').val();
        const color = $('#color').val();
        const name = $('#name').val();
        const price = $('#price').val();
        const image = $('#image').val();
        

        if (!userId) {
            const cartItem = {
                color: color,
                id: -1,
                image: image,

                price: parseInt(price, 10),
                name: name,
                productId: productid,
                quantity: parseInt(quantity, 10),
                size: selectedSize == null ? 'S' : selectedSize
            };

            let cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];

            const existingCartItem = cartItemsInSession.find(item =>
                item.productId === productid &&
                item.size === (selectedSize == null ? 'S' : selectedSize)
            );

            if (existingCartItem) {
                existingCartItem.quantity += cartItem.quantity;
            } else {
                cartItemsInSession.push(cartItem);
            }

            sessionStorage.setItem('cartItems', JSON.stringify(cartItemsInSession));

            window.location.href = "/product/detail/" + productid;

        }
        else {
            const cartItem = {
                productCartDTO: {
                    Id: productid,
                    Price: price,
                    Name: name
                },
                Quantity: quantity,
                Color: color,
                Size: selectedSize == null ? 'S' : selectedSize,
            };

            const cartDTO = {
                UserId: userId,
                Items: [cartItem],
            };

            $.ajax({
                url: API_URL + `/cart/add`,
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(cartDTO),
                success: function (response) {
                    window.location.href = "/product/detail/" + productid;
                },
                error: function (error) {
                    console.log(error);
                    btn_submit.attr("disabled", false);
                    alert("Error Add to cart: " + userId.toString());
                }
            });
        }
    }

    function countItemCart() {
        getAllCart();
        const userId = $("#user-info").data("userid");
        if (userId) {
            $.ajax({
                url: API_URL + `/cart/total/${userId}`,
                method: "GET",
                contentType: "application/json",
                success: function (response) {
                    const totalItemCount = response;
                    $(".total-count").text(totalItemCount);
                }
            });
        } else {
            const cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];
            const totalItemCount = cartItemsInSession.reduce((total, item) => total + parseInt(item.quantity), 0);
            $.ajax({
                contentType: "application/json",
                success: function (response) {
                    $(".total-count").text(totalItemCount);
                }
            });
        }
    }

    countItemCart();

    function deleteCartItem(itemid) {
        const userId = $("#user-info").data("userid");
        if (userId) {
            $.ajax({
                url: API_URL + `/cart/item/delete/${itemid}`,
                method: "POST",
                contentType: "application/json",
                success: function (response) {
                    $.ajax({
                        url: API_URL + `/cart/${userId}`,
                        method: "GET",
                        contentType: "application/json",
                        success: function (response) {
                            console.log(response);
                            displayCart(response);
                           countItemCart();
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        } else {

            const productid = $('#productid').val();
            const size = $('#size').val();
            const cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];

            const existingCartItemIndex = cartItemsInSession.findIndex(item =>
                item.productId === productid &&
                item.size === size
            );
                    $.ajax({
                       
                        success: function (response) {
                            if (existingCartItemIndex !== -1) {
                                cartItemsInSession.splice(existingCartItemIndex, 1);

                                sessionStorage.setItem('cartItems', JSON.stringify(cartItemsInSession));
                                
                                displayCart(cartItemsInSession);
                                countItemCart();
                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
        }
        
        
    }

    function connectPlusHandlers() {
        $(".quantity").on("click", ".plus", function (event) {
            var itemid = $(this).closest('a').data('itemid');
            plusQuantity(itemid);
        });

    }

    function plusQuantity(itemid) {
        const userId = $("#user-info").data("userid");
        if (userId) {
            $.ajax({
                url: API_URL + `/cart/item/plus/${itemid}`,
                method: "POST",
                contentType: "application/json",
                success: function (response) {
                    $.ajax({
                        url: API_URL + `/cart/${userId}`,
                        method: "GET",
                        contentType: "application/json",
                        success: function (response) {
                            console.log(response);
                            displayCart(response);
                            countItemCart();
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }
        else {
            const productid = $('#productid').val();
            const size = $('#size').val();
            const cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];
            const existingCartItem = cartItemsInSession.find(item =>
                item.productId === productid &&
                item.size === size
            );

            $.ajax({
                success: function (response) {
                    if (existingCartItem) {
                        existingCartItem.quantity++;
                        sessionStorage.setItem('cartItems', JSON.stringify(cartItemsInSession));
                    }

                    displayCart(cartItemsInSession);
                    countItemCart();
                },
                error: function (error) {
                    console.log(error);
                }
            });
            
        }
    }

    function connectMinusHandlers() {
        $(".quantity").on("click", ".minus", function (event) {
            var itemid = $(this).closest('a').data('itemid');
            minusQuantity(itemid);
            
        });
    }


    function minusQuantity(itemid) {
        const userId = $("#user-info").data("userid");
        if (userId) {
            $.ajax({
                url: API_URL + `/cart/item/minus/${itemid}`,
                method: "POST",
                contentType: "application/json",
                success: function (response) {
                    $.ajax({
                        url: API_URL + `/cart/${userId}`,
                        method: "GET",
                        contentType: "application/json",
                        success: function (response) {
                            console.log(response);
                            for (let i = 0; i < response.length; i++) {
                                if (response[i].quantity === 0) {
                                    deleteCartItem(response[i].id);
                                }
                            }
                            displayCart(response);
                            countItemCart();
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            });
        }
        else {
            const productid = $('#productid').val();
            const size = $('#size').val();
            const cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];
            const existingCartItem = cartItemsInSession.find(item =>
                item.productId === productid &&
                item.size === size
            );

            $.ajax({
                success: function (response) {
                    if (existingCartItem) {
                        existingCartItem.quantity--;
                        if (existingCartItem.quantity <= 0) {
                            const itemIndex = cartItemsInSession.indexOf(existingCartItem);
                            if (itemIndex !== -1) {
                                cartItemsInSession.splice(itemIndex, 1);
                            }
                        }
                        sessionStorage.setItem('cartItems', JSON.stringify(cartItemsInSession));
                    }

                    displayCart(cartItemsInSession);
                    countItemCart();
                },
                error: function (error) {
                    console.log(error);
                }
            });
            
        }
        
    }

    function connectRemoveHandlers() {
        $(".cart_ac_remove").click(function (event) {
            var itemid = $(this).data('itemid');
            deleteCartItem(itemid);
        });
        
    }

    function displayCart(listItems) {
        var data = "";
        var total = 0;
        listItems.forEach(p => {
            data += `
            <div class="cart_item js_cart_item">
                <div class="ld_cart_bar"></div>
                <div class="row al_center">
                    <div class="col-12 col-md-12 col-lg-4">
                        <div class="page_cart_info flex al_center">
                            <a href="/product/detail/${p.productId}">
                                <img class="lazyload w__100 lz_op_ef"
                                     src="data:image/svg+xml,%3Csvg%20viewBox%3D%220%200%201128%201439%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%3C%2Fsvg%3E"
                                     data-src="${p.image}" alt="">
                            </a>
                            <div class="mini_cart_body ml__15">
                                <h5 class="mini_cart_title mg__0 mb__5">
                                    <a href="/product/detail/${p.productId}">${p.name}</a>
                                </h5>
                                <div class="mini_cart_meta">
                                    <p class="cart_selling_plan"></p>
                                </div>
                                <div class="mini_cart_tool mt__10">
                                <input value="${p.productId}" id="productid" hidden/>
                                    <a href="" data-itemid="${p.id}"
                                       class="cart_ac_remove js_cart_rem ttip_nt tooltip_top_right">
                                        <span class="tt_txt">Remove this item</span>
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
                                             stroke="currentColor" fill="none" stroke-linecap="round"
                                             stroke-linejoin="round">
                                            <polyline points="3 6 5 6 21 6"></polyline>
                                            <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2">
                                            </path>
                                            <line x1="10" y1="11" x2="10" y2="17"></line>
                                            <line x1="14" y1="11" x2="14" y2="17"></line>
                                        </svg>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-4 col-lg-2 tc__ tc_lg">
                        <div class="cart_meta_prices price">
                                <div class="cart_price">${p.price.toLocaleString('vi-VN')}<span>₫</span></div>
                        </div>
                    </div>
                    <div class="col-12 col-md-4 col-lg-1 tc__ tc_lg">
                     
                                <div class="cart_price">${p.size}</div>
                                <input id="size" value="${p.size}" hidden/>
                     
                    </div>
                        <div class="col-12 col-md-4 col-lg-1 tc__ tc_lg">
                            
                                <div class="cart_price">${p.color}</div>
                           
                        </div>
                    <div class="col-12 col-md-4 col-lg-2 tc mini_cart_actions">
                    <a href="" data-itemid="${p.id}">
                        <div  class="quantity pr mr__10 qty__true">
                            <input type="number" data-quantity="${p.quantity}" class="input-text qty text tc qty_cart_js" name="updates[]"
                                   value="${p.quantity}">
                            <div class="qty tc fs__14">
                                <button type="button" class="plus db cb pa pd__0 pr__15 tr r__0">
                                    <i class="facl facl-plus"></i>
                                </button>
                                <button type="button" class="minus db cb pa pd__0 pl__15 tl l__0 qty_1">
                                    <i class="facl facl-minus"></i>
                                </button>
                            </div>
                        </div>
                    </a>
                    </div>
                    <div class="col-12 col-md-4 col-lg-2 tc__ tr_lg">
                            <span class="cart-item-price fwm cd js_tt_price_it">${(p.price*p.quantity).toLocaleString('vi-VN')}<span>₫</span></span>
                    </div>
                </div>
            </div>`;
            total += p.price * p.quantity;
        });
        $("#cart-tb").html(data);
        $(".cart_tot_price").text(total.toLocaleString('vi-VN') + '₫');
        //$('#total_price').val(total);
        //document.getElementById("total_price").setAttribute('value', total.valueOf().toString());

        connectRemoveHandlers();
        connectPlusHandlers();
        connectMinusHandlers();
    }
    

    function getAllCart() {
        const userId = $("#user-info").data("userid");
        if (userId) {
            $.ajax({
                url: API_URL + `/cart/${userId}`,
                method: "GET",
                contentType: "application/json",
                success: function (response) {
                    console.log(response);
                    displayCart(response);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else {
            const cartItemsInSession = JSON.parse(sessionStorage.getItem('cartItems')) || [];
            console.log(cartItemsInSession);
            displayCart(cartItemsInSession);
        }
    }


    function getCartByUserId() {
        const userId = $("#user-info").data("userid");
        $.ajax({
            url: API_URL + `/cart/get-by-userId/` + { userId },
            method: "GET",
            contentType: "application/json",
            success: function (response) {
                window.location.href = "/product/detail/" + productid;
            },
            error: function (error) {
                console.log(error);
                btn_submit.attr("disabled", false);
                alert("Error: ");
            }
        });
    }


    // Load cart
    function loadCart() {
        cart = JSON.parse(sessionStorage.getItem('shoppingCart'));
    }
    if (sessionStorage.getItem("shoppingCart") != null) {
        loadCart();
    }

    var obj = {};

    // Add to cart
    obj.addItemToCart = function (productid, price, quantity, color, size) {
        for (var item in cart) {
            if (cart[item].productid === productid) {
                cart[item].quantity++;
                saveCart();
                return;
            }
        }
        var item = new Item(productid, price, quantity, color, size);
        cart.push(item);
        saveCart();
    }
    // Set count from item
    obj.setCountForItem = function (productid, quantity, price, color, size) {
        for (var i in cart) {
            if (cart[i].productid === productid) {
                cart[i].quantity = quantity;
                cart[i].color = color;
                cart[i].size = size;
                break;
            }
        }
    };
    // Remove item from cart
    obj.removeItemFromCart = function (productid) {
        for (var item in cart) {
            if (cart[item].productid === productid) {
                cart[item].quantity--;
                if (cart[item].quantity === 0) {
                    cart.splice(item, 1);
                }
                break;
            }
        }
        saveCart();
    }

    // Remove all items from cart
    obj.removeItemFromCartAll = function (productid) {
        for (var item in cart) {
            if (cart[item].productid === productid) {
                cart.splice(item, 1);
                break;
            }
        }
        saveCart();
    }

    // Clear cart
    obj.clearCart = function () {
        cart = [];
        saveCart();
    }

    // Count cart 
    obj.totalCount = function () {
        var totalCount = 0;
        for (var item in cart) {
            totalCount += cart[item].count;
        }
        return totalCount;
    }

    // Total cart
    obj.totalCart = function () {
        var totalCart = 0;
        for (var item in cart) {
            totalCart += cart[item].price * cart[item].count;
        }
        return Number(totalCart.toFixed(2));
    }

    // List cart
    obj.listCart = function () {
        var cartCopy = [];
        for (i in cart) {
            item = cart[i];
            itemCopy = {};
            for (p in item) {
                itemCopy[p] = item[p];

            }
            itemCopy.total = Number(item.price * item.count).toFixed(2);
            cartCopy.push(itemCopy)
        }
        return cartCopy;
    }

    // cart : Array
    // Item : Object/Class
    // addItemToCart : Function
    // removeItemFromCart : Function
    // removeItemFromCartAll : Function
    // clearCart : Function
    // countCart : Function
    // totalCart : Function
    // listCart : Function
    // saveCart : Function
    // loadCart : Function
    return obj;
})();


// *****************************************
// Triggers / Events
// ***************************************** 
// Add item
$('.add-to-cart').click(function (event) {
    event.preventDefault();
    var productid = $(this).data('productid');
    var price = Number($(this).data('price'));
    var quantity = Number($(this).data('quantity'));
    var color = $(this).data('color');
    var size = $(this).data('size');
    shoppingCart.addItemToCart(productid, price, quantity, color, size);
    displayCart();
});



// Clear items
$('.clear-cart').click(function () {
    shoppingCart.clearCart();
    displayCart();
});


function displayCart() {
    var cartArray = shoppingCart.listCart();
    var output = "";
    for (var i in cartArray) {
        output += "<tr>"
            + "<td>" + cartArray[i].name + "</td>"
            + "<td>(" + cartArray[i].price + ")</td>"
            + "<td><div class='input-group'><button class='minus-item input-group-addon btn btn-primary' data-name=" + cartArray[i].name + ">-</button>"
            + "<input type='number' class='item-count form-control' data-name='" + cartArray[i].name + "' value='" + cartArray[i].count + "'>"
            + "<button class='plus-item btn btn-primary input-group-addon' data-name=" + cartArray[i].name + ">+</button></div></td>"
            + "<td><button class='delete-item btn btn-danger' data-name=" + cartArray[i].name + ">X</button></td>"
            + " = "
            + "<td>" + cartArray[i].total + "</td>"
            + "</tr>";
    }
    $('.show-cart').html(output);
    $('.total-cart').html(shoppingCart.totalCart());
    $('.total-count').html(shoppingCart.totalCount());
}

// Delete item button

$('.show-cart').on("click", ".delete-item", function (event) {
    var productid = $(this).data('productid')
    shoppingCart.removeItemFromCartAll(productid);
    displayCart();
})


// -1
$('.show-cart').on("click", ".minus-item", function (event) {
    var productid = $(this).data('productid')
    shoppingCart.removeItemFromCart(productid);
    displayCart();
})
// +1
$('.show-cart').on("click", ".plus-item", function (event) {
    var productid = $(this).data('productid')
    shoppingCart.addItemToCart(productid);
    displayCart();
})

// Item count input
$('.show-cart').on("change", ".item-count", function (event) {
    var productid = $(this).data('productid');
    var count = Number($(this).val());
    shoppingCart.setCountForItem(productid, count);
    displayCart();
});

displayCart();
