const uri = "https://localhost:44354/api/BookItems";
let books = null;

$(document).ready(function () {
    getData();
});
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#books");
            $(tBody).empty();
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.title))
                    .append($("<td></td>").text(item.author))
                    .append($("<td></td>").text(item.genre))
                    .append($("<td></td>").text(item.isbn))
                    .append(
                        $("<td></td>").append(
                            $("<button>Edit</button>").on("click", function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button>Delete</button>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );
                tr.appendTo(tBody);
            });
            books = data;
        }
    });
}
function addItem() {
    const item = {
        title: $("#add-title").val(),
        author: $("#add-author").val(),
        genre: $("#add-genre").val(),
        isbn: $("#add-isbn").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateBookItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-title").val("");
            $("#add-author").val("");
            $("#add-genre").val("");
            $("#add-isbn").val("");
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(books, function (key, item) {
        if (item.id === id) {
            $("#edit-title").val(item.title);
            $("#edit-author").val(item.author);
            $("#edit-genre").val(item.genre);
            $("#edit-isbn").val(item.isbn);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    const item = {
        id: id,
        title: $("#edit-title").val(),
        author: $("#edit-author").val(),
        genre: $("#edit-genre").val(),
        isbn: $("#edit-isbn").val(),
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateBookItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            closeInput();
        }
    });
}

function closeInput() {
    $("#spoiler").css({ display: "none" });
}