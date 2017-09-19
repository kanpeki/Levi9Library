$(document).tooltip({
	 show: null
});

$('.navbar-toggle').click(function () {
	$(this).toggleClass('open');
});

$('#wrap').click(function () {
	if($('.navbar-toggle').hasClass('open')) {
		$('.navbar-collapse').collapse('toggle');
		$('.navbar-toggle').toggleClass('open');
	}
});