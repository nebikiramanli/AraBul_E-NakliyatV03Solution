$(function () {

    $('#modal_noticedetail').on('show.bs.modal',
        function (e) {

            var btn = $(e.relatedTarget);
            noticeid = btn.data("notice-id");

            $("#modal_noticedetail_body").load("/Notice/GetNoticeText/" + noticeid);
        });

});