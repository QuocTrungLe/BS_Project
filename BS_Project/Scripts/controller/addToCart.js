$(document).ready(function () {
    //if ($('#username').text() !== "") {
    //    $.ajax({
    //        url: "/OrderBook/LoadOrderCart",
    //        data: {
    //        },
    //        datatype: "json",
    //        type: 'POST',
    //        success: function (data) {
    //            if (data >= 3) {
    //                $('div[name=addToCart]').css("background-color", "red");
    //                $('div[name=addToCart]').html("Hết lượt đặt sách");
    //            }
    //            $('#totalBook').text(data);
    //        },
    //        error: function () {
    //            alert("Error occured!!");
    //        }
    //    });
    //} 

    $('div[name=addToCart]').click(function () {
        //Lay gia tri tu thuoc tinh id cua div addToCart
        debugger
        if ($('#username').text() !== "") {
            var bookID = $(this).attr('id');
            $.ajax({
                url: "/CartItem/AddItem",
                data: {
                    bookID: bookID, quantity: 1
                },
                datatype: "json",
                type: 'POST',
                success: function (data) {
                    if (data.totalBuy >= 10) {
                        $('#totalBook').text(data.totalBuy);
                        if (data.Status) {
                            $('#mySuccess').modal('show');
                        } else {
                            $('#myFail').modal('show');
                        }
                        $('div[name=addToCart]').css("background-color", "red");
                        $('div[name=addToCart]').html("Your cart's full");


                    } else {
                        $('#mySuccess').modal('show');
                        $('#totalBook').text(data.totalBuy);
                        $('div[name=addToCart]').css("background-color", "#2fdab8");
                        $('div[name=addToCart]').html("Add To Cart");
                    }
                },
                error: function () {
                    alert("Error occured!!");
                }
            });
        } else {
            $('#myModalCart').modal('show');
        }

    });

    $('form#btnOrderCart').submit(function (e) {
        e.preventDefault();
        $('#viewFail').modal('show');
    });
});