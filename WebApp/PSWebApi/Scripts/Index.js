$(document).ready(function () {

    var arrows = $('.arrow-right');
    for (let i = 0; i < arrows.length; i++) {
        arrows[i].style.transitionProperty = "transform";
        arrows[i].style.transitionDuration = "250ms";

        arrows[i].style.transform = $('#toggler__checkbox-id-' + (i + 1))[0].checked ? "rotate(90deg)" : "rotate(0deg)";
    }

    FilterProductStore("");

    $("#addProduct").click(function (event) {
        event.preventDefault();
        AddProduct();
    });

    $("#clearProduct").click(function (event) {
        event.preventDefault();
        $('#add-block-error-message')[0].innerHTML = "";
        $('#addName')[0].value = "";
        $('#addPrice')[0].value = 0;
    });

    $("#clearFilterProduct").click(function (event) {
        event.preventDefault();
        $('#subString')[0].value = "";
        FilterProductStore("");
    });

    $("#filterProduct").click(function (event) {
        event.preventDefault();
		FilterProductStore($('#subString').val());
    });

    $('.toggler__checkbox-id').change(function (event) {
        event.currentTarget.nextElementSibling.firstElementChild.style.transform = event.currentTarget.checked ? "rotate(90deg)" : "rotate(0deg)";  

        autoResizeTBody(document.getElementById('tableFilter').children[1]);
    });
});

var substr = $('#subString');
var svgTrash = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">';
    svgTrash += '<path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>';
    svgTrash += '<path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>';
    svgTrash += '</svg>'
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
            if (data.code == 500) {
                $('#add-block-error-message')[0].innerHTML = "";
                alert(data.message);
            } else if (data.code == 400)
                $('#add-block-error-message')[0].innerHTML = data.message;
            else {
                $('#add-block-error-message')[0].innerHTML = "";
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
            if ([400, 500].indexOf(data.code) > -1)
                alert(data.message)
            else
               FilterProductStore(substr.val());
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
// вывод полученных данных на экран
function WriteResponse(products, attr) {
    var strResult = "<table id='tableFilter' class='table table-condensed table-fixed'><thead><tr><th class='col-xs-2' data-toggle='tooltip' data-placement='top' title='ID'>ID</th><th class='col-xs-4' data-toggle='tooltip' data-placement='top' title='Наименование'>Наименование</th><th class='col-xs-4' data-toggle='tooltip' data-placement='top' title='Стоимость, руб.'>Стоимость, руб.</th><th class='col-xs-2'>&nbsp;</th></tr></thead><tbody>";
    $.each(products, function (index, product) {
        strResult += "<tr><td class='col-xs-2' data-toggle='tooltip' data-placement='top' title='" + product.Id + "'>" + product.Id + "</td><td class='col-xs-4' data-toggle='tooltip' data-placement='top' title='" + product.Name + "'> " + product.Name + "</td><td class='col-xs-4' data-toggle='tooltip' data-placement='top' title='" + product.Price + "'>" + product.Price +
            "</td><td class='col-xs-2'><a id='delItem' data-item='" + product.Id + "' onclick='DeleteItem(this);' class='bi bi-trash' >" + svgTrash + "</a></td></tr>";
    });
    strResult += "</tbody></table>";

    strResult += '<div class="table-footer-block">' +
                    '<div class="form-inline row">' +
                        '<div class="row">' +
                            '<div class="col-xs-1" style="text-align: center;">' +
                                '<button id="refreshFilterTable" class="btn btn-primary" style="padding: 1px 6px; margin-top: 2.5px;"><span class="glyphicon glyphicon-refresh"></span></button>' +
                            '</div>' +
                            '<div class="col-xs-11">' +
                                '<label class="table-footer-count">Кол-во строк: ' + products.length + '</label>' +
                            '</div>' +
                        '</div>'+
                    '</div>' +
                '</div>';

    $(attr).html(strResult);

    autoResizeTBody(document.getElementById('tableFilter').children[1]);

    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    })

    $('#refreshFilterTable').click(function () {
        FilterProductStore($("#subString").val());
    });
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
            if (data.code == 500)
                alert(data.message)
            else
                WriteResponse(data, "#tableFilterBlock");
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function autoResizeTBody(elementResizeable) {
    let viewportOffset = elementResizeable.getBoundingClientRect();
    let top = viewportOffset.top;
    let left = viewportOffset.left;

    let differHeight = window.innerHeight - viewportOffset.top - 75;

    elementResizeable.style.height = differHeight + "px";
}