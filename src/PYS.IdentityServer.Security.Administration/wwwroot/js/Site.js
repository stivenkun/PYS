
var alert = function (title,message) {
    $("#modal-alert2").iziModal({
        title: title,
        subtitle: message,
        icon: 'icon-power_settings_new',
        headerColor: '#BD5B5B',
        width: 600,
        timeout: 5000,
        timeoutProgressbar: true,
        transitionIn: 'fadeInDown',
        transitionOut: 'fadeOutDown',
        pauseOnHover: true
    });
    $('#modal-alert2').iziModal('open');
}
