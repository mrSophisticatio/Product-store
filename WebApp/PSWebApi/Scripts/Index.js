$(document).ready(function () {
    GetAllProducts();

    $("#addProduct").click(function (event) {
        event.preventDefault();
        AddProduct();
    });

    $("#filterProduct").click(function (event) {
        event.preventDefault();
		FilterProductStore($('#subString').val());
    });
});
var substr = $('#subString');
// Получение всех продуктов по ajax-запросу
function GetAllProducts() {

    $("#createBlock").css('display', 'block');

    $.ajax({
        url: '/api/product',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data, "#tableBlock");
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
// Добавление нового продукта
function AddProduct() {
    // получаем значения для нового продукта
    var product = {
        Name: $('#addName').val(),
        Price: $('#addPrice').val()
    };

    $.ajax({
        url: '/api/product/',
        type: 'POST',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            GetAllProducts();
            if (substr.val() !== ""){
                FilterProductStore(substr.val());
            }
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
// Удаление продукта
function DeleteProduct(id) {

    $.ajax({
        url: '/api/product/' + id,
        type: 'DELETE',
        contentType: "application/json;charset=utf-8", //тип данных, которые мы отправляем
        success: function (data) {
            GetAllProducts();
            if (substr.val() !== ""){
                FilterProductStore(substr.val());
            }
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

// вывод полученных данных на экран
function WriteResponse(products, attr) {
    var strResult = "<table><th>ID____</th><th>Название____</th><th>Цена</th>";
    $.each(products, function (index, product) {
        strResult += "<tr><td>" + product.Id + "</td><td> " + product.Name + "</td><td>" + product.Price +
            "</td><td><a id='delItem' data-item='" + product.Id + "' onclick='DeleteItem(this);' >Удалить</a></td></tr>";
    });
    strResult += "</table>";
    $(attr).html(strResult);
}
// обработчик удаления
function DeleteItem(el) {
    // получаем id удаляемого объекта
    var id = $(el).attr('data-item');
    DeleteProduct(id);
}
// поиск продукта
function FilterProductStore(subString) {

    $.ajax({
		url: '/api/product/?subString=' + subString,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data, "#tableFilterBlock");
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}