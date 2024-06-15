document.getElementById("command").addEventListener("keypress", e => {
    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();

        $.ajax({
            method: "post",
            url: "SQLite?handler=Command",
            data: $('form').serialize(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-CSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            error: function (xhr, status, err) {
                alert(err);
            }
        }).done(function (res) {
            var ele = document.getElementById("view");

            try {
                ele.innerHTML = JSON.stringify(JSON.parse(res), null, 4);
            }
            catch {
                ele.innerHTML = res;
            }
        });
    }
});
