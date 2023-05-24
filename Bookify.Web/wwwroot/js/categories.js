$(document).ready(function () {
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });
    $('.js-toggle-status').on('click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: "Are you sure about that",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: "btn-danger"
                },
                cancel: {
                    label: 'No',
                    className: "btn-secondary"
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: '/Categories/ToggleStatus/' + btn.data('id'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
                        },
                        success: function (lastUpdateOn) {
                            var status = btn.parents('tr').find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                            btn.parents('tr').find('.js-update-on').html(lastUpdateOn);
                        }
                    });
                }
            }
        });

    });

});