(function($) {
  "use strict"; // Start of use strict

  $(document).ready(function() {
		var page = window.location.href.substring(window.location.href.lastIndexOf('/') + 1).toLowerCase();
		switch(page){
			case '':
			case 'home':
			case 'about':
				$('#experience_page').removeClass('active');
				$('#education_page').removeClass('active');
				$('#skills_page').removeClass('active');
				$('#about_page').addClass('active');
				break;
			case 'experience':
				$('#about_page').removeClass('active');
				$('#education_page').removeClass('active');
				$('#skills_page').removeClass('active');
				$('#experience_page').addClass('active');
				break;
			case 'education':
				$('#about_page').removeClass('active');
				$('#experience_page').removeClass('active');
				$('#skills_page').removeClass('active');
				$('#education_page').addClass('active');
				break;
			case 'skills':
				$('#about_page').removeClass('active');
				$('#experience_page').removeClass('active');
				$('#education_page').removeClass('active');
				$('#skills_page').addClass('active');
				break;
			default:
				$('#about_page').removeClass('active');
				$('#experience_page').removeClass('active');
				$('#education_page').removeClass('active');
				$('#skills_page').removeClass('active');
				break;
		}
  });
  
})(jQuery); // End of use strict