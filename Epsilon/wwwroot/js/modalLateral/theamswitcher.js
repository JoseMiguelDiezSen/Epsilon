/* Theam switcher js file */
jQuery(document).ready(function() {
// Pattern Selector function//////////////////////////////////	
	jQuery('.patterns a').click(function(e) {
		e.preventDefault();
			jQuery(this).parent().find('img').removeClass('active');
			jQuery(this).find('img').addClass('active');

			var name = jQuery(this).attr('name');
				jQuery('body').css('background', 'url(themes/switch/images/pattern/'+name+'.png) repeat center center scroll');
				jQuery('body').css('background-size', 'auto');
	});
// Style Selector function ////////////////////////////////////
	jQuery('.style a').click(function(e) {
		e.preventDefault();
		jQuery(this).parent().find('img').removeClass('active');
		jQuery(this).find('img').addClass('active');

		var name = jQuery(this).attr('name');

		if(name == 'green') {
			jQuery('#callCss').attr('href', '');
		} else {
			jQuery('#callCss').attr('href', 'themes/'+name+'/bootstrap.min.css');
		}

	});
	
	/* Settings Button */
	$(document).on('click', '#themesBtn', function(e) {
		e.preventDefault();
		if (window.toggleModalLateral) {
			window.toggleModalLateral(true);
		} else {
			$('#secectionBox').stop().animate({ right: '0' }, 400);
			$('#themesBtn').stop().animate({ right: '-90' }, 150);
		}
	});

	$(document).on('click', '#hideme', function(e) {
		e.preventDefault();
		if (window.toggleModalLateral) {
			window.toggleModalLateral(false);
		} else {
			$('#secectionBox').stop().animate({ right: '-999' }, 400);
			$('#themesBtn').stop().animate({ right: '0' }, 400);
		}
	});
});


