$(function () {

    var noticeids = [];

    $("div[data-notice-id]").each(function (i, e) {
        noticeids.push($(e).data("notice-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Notice/GetLiked",
        data: { ids: noticeids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNotice = $("div[data-notice-id=" + id + "]");
                var btn = likedNotice.find("button[data-liked]");
                var span = btn.find("span.like-star");

                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }

        }

    }).fail(function () {

    });



    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noticeid = btn.data("notice-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");

        console.log(liked);
        console.log("like count : " + spanCount.text());

        $.ajax({
            method: "POST",
            url: "/Notice/SetLikeState",
            data: { "noticeid": noticeid, "liked": !liked }
        }).done(function (data) {

            console.log(data);

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                console.log("like count(after) : " + spanCount.text());


                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");

                if (liked) {
                    spanStar.addClass("glyphicon-star");
                } else {
                    spanStar.addClass("glyphicon-star-empty");
                }

            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })

    });
});