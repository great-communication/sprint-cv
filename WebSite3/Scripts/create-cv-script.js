$(function () { 
    $('#tabs').tabs({
        active: activeTab,
        activate: function (event, ui) {
            var currentTabIndex = ui.newTab.index();
            $('#tab-index').val(currentTabIndex);
        }
    });    
    
    //Save warning
    $('#first-name').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#last-name').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#email').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#phone').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#website').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#address').on('change paste keyup', function () {
        $('#save-reminder').html('Not saved!');
    });
    $('#summary').on('change paste keyup', function () {
        $('#save-reminder-2').html('Not saved!');
    });
    $('#cv-name').on('change paste keyup', function () {
        $('#save-reminder-3').html('Not saved!');
    });
    $('#complete-button').on('click', function () {
        $('#complete').val(true);
        $('#done').val(true);        
        $('#cv-form').submit();
    });
    //Client side validation
    $('#first-name').attr('maxlength', '100');
    $('#last-name').attr('maxlength', '100');
    $('#email').attr('maxlength', '100');
    $('#phone').attr('maxlength', '100');
    $('#website').attr('maxlength', '100');
    $('#address').attr('maxlength', '500');
    $('#summary').attr('maxlength', '2000');
    $('#cv-name').attr('maxlength', '20');  

    $('.datepicker').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
});