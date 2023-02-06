window.hzmr = window.hzmr || []; window.hzmr.push("clipperBookmarklet:2878");
(function() {
	function addToQueryString(url, key, value) {
		var query = url.indexOf('?');
		var anchor = url.indexOf('#');
		if (query == url.length - 1) {
			// Strip any ? on the end of the URL
			url = url.substring(0, query);
			query = -1;
		}
		return (anchor > 0 ? url.substring(0, anchor) : url)
		+ (query > 0 ? "&" + key + "=" + value : "?" + key + "=" + value)
		+ (anchor > 0 ? url.substring(anchor) : "");
	}
	var ss = document.createElement('link');
	ss.rel = 'stylesheet';
	ss.href = addToQueryString('//st.hzcdn.com/res/2878/css/style?f=clipper', 'cb', Math.floor(Math.random()*1000000));
	document.getElementsByTagName('head')[0].appendChild(ss);
	
	var js = document.createElement('script');
	js.type = 'text/javascript';
	js.src = addToQueryString('//st.hzcdn.com/js/script?f=clipper&v=2878&s=101&l=en-US&d=1&m=1&j=22&p=1', 'cb', Math.floor(Math.random()*1000000));
	document.body.appendChild(js);
})();
