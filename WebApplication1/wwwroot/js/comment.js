document.getElementById("comment").addEventListener("keypress", e => {
    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();

        $.ajax({
            method: "post",
            url: "Post?handler=Comment",
            data: $('form').serialize(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-CSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            error: function (xhr, status, err) {
                alert(err);
            }
        }).done(function (res) {
            var obj = JSON.parse(res);
            var val = document.getElementById("comment");
            var ele = document.getElementById("comments");

            val.value = "";
            ele.innerHTML = "";

            for (let i = obj.length - 1; i >= 0; i--) {
                ele.innerHTML += '<div class="border mt-4 card shadow-sm"><div class="p-3 card-body">' + obj[i]["content"] + '<div class="float-end">From ' + obj[i]["user"] + '</div></div></div>';
            }
        });
    }
});
