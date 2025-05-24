MostrarGrid = () => {

    if ($('#gridEdiciones').is(':hidden')) {
        $("#gridEdiciones").show();
        $("#gridEdiciones").slideDown(700);
        $("#spnBtn").removeClass('fa fa-chevron-down').addClass('fa fa-chevron-up');
    }
    else if ($('#gridEdiciones').is(':visible')) {
        $("#gridEdiciones").slideUp(700);
        $("#spnBtn").removeClass('fa fa-chevron-up').addClass('fa fa-chevron-down');
    }
}