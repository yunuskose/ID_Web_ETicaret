window.fourSq=window.fourSq||{};
(function(a){var e=a.widget=a.widget||{},f={count:0,registry:{},wRegistry:window.___fourSq_widget_load={},register:function(b){b.id="loader_"+f.count++;f.registry[b.src]=b;f.wRegistry[b.id]=c.bind(b.lc,b)},instance:function(b){return f.registry[b]||new g(b)}},g=function(b){this.src=b;this.q=[];this.loaded=!1;f.register(this)};g.prototype={add:function(b){b&&(this.loaded?b():this.q.push(b));return this},load:function(){if(!this.loaded){var b=h.createScript(this.src);b.setAttribute("onLoad",'___fourSq_widget_load["'+
this.id+'"]();');var a=c.bind(this.lc,this);b.onreadystatechange=function(){"loaded"!=this.readyState&&"complete"!=this.readyState||a()};(h.head()||h.body()).appendChild(b)}return this},lc:function(){this.loaded||(c.each(this.q,function(b){b()}),this.q=void 0,this.loaded=!0)}};var d=e.Browser={};(function(){var b=navigator.userAgent.toLowerCase(),c=navigator.platform.toLowerCase(),a=b.match(/(opera|ie|firefox|chrome|version)[\s\/:]([\w\d\.]+)?.*?(safari|version[\s\/:]([\w\d\.]+)|$)/)||[null,"unknown",
0],f="ie"==a[1]&&document.documentMode;d.name="version"==a[1]?a[3]:a[1];d.version=f||parseFloat("opera"==a[1]&&a[4]?a[4]:a[2]);d.Platform={name:b.match(/ip(?:ad|od|hone)/)?"ios":(b.match(/(?:webos|android)/)||c.match(/mac|win|linux/)||["other"])[0]};d[d.name]=!0;d[d.name+parseInt(d.version,10)]=!0;d.Platform[d.Platform.name]=!0;d.isTouchDevice=d.Platform.ios||d.Platform.webos||d.Platform.android})();var c=e.Util={require:function(b,a){a=a||function(){};b=c.isString(b)?[b]:b||[];var d=1,e=function(){--d||
a()};c.each(b,function(b){d++;f.instance(b).add(e).load()});e()},nextId:function(){var b=0;return function(){return"fourSq_widget_id_"+ +new Date+"_"+b++ +"_"+Math.floor(1E3*Math.random())}}(),slice:function(b,a,d){b=c.toArray(b);return void 0!==d?b.slice(a,d):b.slice(a)},bind:function(b,a){var d=c.slice(arguments,2);return function(){return b.apply(a,d.concat(c.slice(arguments,0)))}},hasOwnPropertyH:function(b,a){return Object.prototype.hasOwnProperty.call(b,a)},testType:function(b,a){var c=Object.prototype.toString;
return c.call(b)==c.call(a)},isString:function(b){return c.testType(b,"")},isArray:function(b){return c.testType(b,[])},isObject:function(b){return c.testType(b,{})},isFunction:function(b){return c.testType(b,function(){})},isIndexed:function(b){return b&&b.length==+b.length},isDomElement:function(b){return b&&"object"===typeof b&&b.nodeType},isEmpty:function(b){return 0==c.toArray(b).length},toArray:function(b){b=b||[];return c.isArray(b)?b:c.isIndexed(b)?c.map(b,function(b){return b}):c.map(b,function(b,
a){return[a,b]})},each:function(b,a,d){b=b||[];a=a||function(){};if(c.isIndexed(b))for(var f=0,e=b.length;f<e&&!1!==a.call(d,b[f],f);f++);else for(f in b)if(c.hasOwnPropertyH(b,f)&&!1===a.call(d,b[f],f))break},filter:function(b,a,d){b=b||[];var f=void 0;c.isIndexed(b)?(f=[],c.each(b,function(b){a.call(d,b)&&f.push(b)})):(f={},c.each(b,function(b,c){a.call(d,b,c)&&(f[c]=b)}));return f},map:function(b,a,d){var f=[];c.each(b,function(b,c){f.push(a.call(d,b,c))});return f},keys:function(b){return c.map(b,
function(b,a){return a})},values:function(b){return c.map(b,function(b,a){return b})},flatMap:function(b,a,d){var f=[];c.each(b,function(b,c){f=f.concat(a.call(d,b,c))});return f},mapValues:function(b,a,d){var f={};c.each(b,function(b,c){f[c]=a.call(d,b,c)});return f},extend:function(b,a){b=b||{};var d=c.slice(arguments,1);c.each(d,function(a,d){for(var f in a)c.hasOwnPropertyH(a,f)&&(b[f]=a[f])});return b},extract:function(b,a){var d={};c.each(a,function(a){var c=b[a];c&&(d[a]=c)});return d},join:function(b,
a,d){var f="",e="";c.each(b,d?function(b,c){f+=e+c+d+b;e=a}:function(b,c){f+=e+b;e=a});return f},toQueryString:function(b){b=c.mapValues(b,function(b,a){return encodeURIComponent(b)});return c.join(b,"\x26","\x3d")},hyphenate:function(b){return b.replace(/[A-Z]/g,function(b){return"-"+b.charAt(0).toLowerCase()})},camelCase:function(b){return b.replace(/-\D/g,function(b){return b.charAt(1).toUpperCase()})},trim:function(b){return b.replace(/^\s+|\s+$/g,"")},preventDefaultAndStopPropagation:function(b){b.preventDefault&&
b.preventDefault();b.stopImmediatePropagation&&b.stopImmediatePropagation();b.stopPropagation&&b.stopPropagation()}};e.Window={queryParams:function(b){b=b||window.location.href;b=b.substring(b.indexOf("?"));b=b.replace(/&amp;/g,"\x26");for(var a=/[?&](.*?)=([^&#]*)/g,c={},d=null;d=a.exec(b);){var f=d[1],d=decodeURIComponent(d[2]);c[f]=d}return c}};var h=e.Dom={relCanonical:function(){var b=h.head()?c.slice(h.getByTagName(h.head(),"LINK"),0):[],a=void 0;c.each(b,function(b){if("canonical"==b.getAttribute("rel")){a=
c.trim(b.getAttribute("href")||"");if(0!=a.indexOf("http"))try{var d=window.location.href,f=d.substring(0,d.indexOf("/",8));a=0==a.indexOf("/")?f+a:f+"/"+a}catch(e){a=void 0}return!1}});return a},head:function(){return document.head||h.getByTagName(document,"head")[0]},body:function(){return document.body||h.getByTagName(document,"body")[0]},documentWidth:function(){return document.body.clientWidth},viewportWidth:function(){return window.innerWidth||h.documentWidth()},bind:function(b,a,c){window.addEventListener?
b.addEventListener(a,c,!1):b.attachEvent("on"+a,c)},replace:function(b,a){if(c.isDomElement(b)&&c.isDomElement(a)){var d=b.parentNode;d.insertBefore(a,b);d.removeChild(b)}},append:function(b,a){c.isDomElement(b)&&c.isDomElement(a)&&b.appendChild(a)},prepend:function(b,a){c.isDomElement(b)&&c.isDomElement(a)&&(b.firstChild?b.insertBefore(a,b.firstChild):b.appendChild(a))},remove:function(b){b.parentNode.removeChild(b)},text:function(b){if(!b)return"";c.isArray(b)||(b=[b]);var a="",d;if(c.hasOwnPropertyH(h.body(),
"innerText"))d=function(b){a+=b.innerText};else{var f=function(b){3===b.nodeType?a+=b.nodeValue:b.hasChildNodes()&&c.each(b.childNodes,f)};d=f}c.each(b,function(b){d(b);a+=" "});return a},containsClass:function(b,a){return 0<=(" "+b.className+" ").indexOf(" "+a+" ")},getByTagName:function(b,a){b=b||document;return b.getElementsByTagName(a)},getByClassName:function(b,a){b=b||document;if(c.isFunction(b.getElementsByClassName))return c.slice(b.getElementsByClassName(a));var d=function(b,a){return h.containsClass(b,
a)?[b]:b.hasChildNodes()?c.flatMap(b.childNodes,function(b){return d(b,a)}):[]};return d(b,a)},create:function(b){return document.createElement(b)},createText:function(b){return document.createTextNode(b)},createScript:function(b){var a=h.create("script");a.type="text/javascript";a.src=b;a.async=!0;return a},getDocument:function(b){return b?b.ownerDocument:document},getComputedStyle:function(b,a){if(b.currentStyle)return b.currentStyle[c.camelCase(a)];var d=h.getDocument(b).defaultView;return(d=d?
d.getComputedStyle(b,null):null)?d.getPropertyValue(c.hyphenate(a)):null}};e.Class={create:function(b,a){var d=function(){this.init.apply(this,arguments)};b&&c.extend(d.prototype,b);a&&c.extend(d,a);d.extend=e.Class.extend;return d},extend:function(b){var a=this,d=function(){return a.apply(this,arguments)};c.extend(d,a);var f=function(){};f.prototype=a.prototype;d.prototype=new f;c.extend(d.prototype,b);return d.prototype.constructor=d}};a.widget=e;e.Util=c;c.require=c.require;e.Window=e.Window;e.Window.queryParams=
e.Window.queryParams})(window.fourSq);(function(a){var e=a.Util,f=window.___fourSq||{secure:!1},g={BASE_URL:"https://foursquare.com"};g.BASE_CDN_URL=(!0===f.secure?"https://platform-s":"http://platform/")+".foursquare.com";g.BASE_JS_PATH="/js/lib";g.BASE_IMG_PATH="/img";g.BASE_HTML_PATH="/html";g.BASE_MODULE_URL=g.BASE_CDN_URL+"/js/modules/";var g=e.extend(g,f),d=function(a,b,c){return b?(a=a[b],void 0!==a?a:c):a},c=a.Config={global:function(a,b){return d(f,a,b)},master:function(a,b){return d(g,a,b)},setLang:function(a){f.lang=a;return"en"!=
a},getLang:function(){var a;return"en"==(a=f.lang)?"":a},callIfDeclared:function(a){a=c.global(a);e.isFunction(a)&&a()}},h=c.master("BASE_CDN_URL")+c.master("BASE_JS_PATH"),b=a.Resources={logger:function(a){return c.master("BASE_URL")+"/intent/"+(a?"logger_sl":"logger")},json2:function(){return h+"/easyXDM/json2.js"},easyXDM:function(){return h+"/easyXDM/easyXDM.min.js"},easyXDMSwf:function(){return h+"/easyXDM/easyxdm.swf"},intent:function(a){return c.master("BASE_URL")+"/intent/"+a},proxy:function(){return c.master("BASE_CDN_URL")+
c.master("BASE_HTML_PATH")+"/proxy.html"},blank:function(){return c.master("BASE_CDN_URL")+c.master("BASE_HTML_PATH")+"/blank.html"},asset:function(a){return c.master("BASE_CDN_URL")+c.master("BASE_IMG_PATH")+"/"+a+".png"},modulePath:function(a){a=c.master("BASE_MODULE_URL")+a;var b=c.master("ENV","prod");return"prod"==b?a+".js":"devWithoutValidate"==b?a+"?mode\x3dprod":a+"?mode\x3ddev"},asyncBundle:function(){return this.modulePath("widgets.asyncbundle")}};a.Config=c;c.global=c.global;c.master=c.master;
c.setLang=c.setLang;a.Resources=b;b.modulePath=b.modulePath})(window.fourSq.widget);(function(a){var e=a.Util;a.Events={bind:function(a,e){this._callbacks=this._callbacks||{};e&&(this._callbacks[a]||(this._callbacks[a]=[])).push(e);return this},trigger:function(a){this._callbacks=this._callbacks||{};var g=e.slice(arguments,1),d=this;e.each(this._callbacks[a]||[],function(a){try{a.apply(d,g)}catch(f){}});return this}}})(window.fourSq.widget);(function(a){var e=a.Browser,f=a.Class,g=a.Config,d=a.Dom,c=a.Util,h=a.Resources,b=a.Window;a.XDM={hasPostMessage:function(){return window.postMessage||document.postMessage},require:function(b){a.XDM.hasPostMessage()||window.easyXDM?b&&b():c.require(h.easyXDM(),b)},urlForRedirect:function(a,b){return b?a.replace("://","://"+b+"."):a},decorate:function(b){b.config.remote?(b.config.container&&(b.config.props.style.display="none"),a.XDM.decorateLocal(b)):a.XDM.decorateRemote(b)},decorateLocal:function(b){var d=
b.rpc.local,f=d.show||function(){};d.show=c.bind(function(){var a=this.getFrame();a&&(a.style.display="block");f()},b);d.redirect=c.bind(function(b){g.setLang(b)&&(this.config.remote=a.XDM.urlForRedirect(this.config.remote,b),this.destroy(),this.reset(),this.init(this.config,this.rpc))},b)},decorateRemote:function(a){a.rpc.remote.show=a.rpc.remote.redirect={}},isPopupProxy_:function(){var a=window.location.href;return 0<a.indexOf("display\x3dpopup")&&0<a.indexOf("\x26xdm_target\x3d")},rpcFactory:function(b,
c){return a.XDM.isPopupProxy_()?new a.XDM.PopupProxyRpc(b,c):a.XDM.hasPostMessage()?new a.XDM.postMessageRpc(b,c):new a.XDM.easyXDMRpc(b,c)}};var l=/^((http.?:)\/\/([^:\/\s]+)(:\d+)*)/,k=function(a){var b=a.toLowerCase().match(l);a=b[2];var c=b[3],b=b[4]||"";if("http:"==a&&":80"==b||"https:"==a&&":443"==b)b="";return a+"//"+c+b},m=function(a){var b;a.origin?b=k(a.origin):a.uri?b=k(a.uri):a.domain&&(b=window.location.protocol+"//"+a.domain);return b||""},n=function(a,b){var d=c.extend({},b.props),
f=d.style||{};delete d.style;delete d.src;c.extend(a,d);c.extend(a.style,f)};a.XDM.PopupProxy=f.create({init:function(){var b={local:{}};b.local.proxy=function(){};b.remote={};b.remote.proxy={};b.remote.initialized=function(){};this.proxyRpc=a.XDM.rpcFactory({},b);this.proxyRpc.initialized(c.bind(this.setupProxyMethods,this))},setupProxyMethods:function(b){var d=b.params,f={local:{},remote:{}},e=this.proxyRpc;c.each(b.keys.local,function(a){f.local[a]=function(){var b=c.slice(arguments);b.unshift(a);
e.proxy.apply(e,b)}});this.popupProxyRpc=new a.XDM.PopupProxyRpc(d,f)}});a.XDM.PopupProxyRpc=f.create({init:function(b,c){this.config=b;this.rpc=c;a.XDM.decorate(this);b.remote?this.initSource(c):this.initProvider(c)},initSource:function(b){this.guid=c.nextId();a.XDM.PopupProxyRpc.currentInstance_=this},initProvider:function(a){var d=b.queryParams();this.guid=d.guid;d=d.xdm_target;this.opener_=window.opener;this.window_=window;this.partner_=this.opener_.frames[d].fourSq.widget.XDM.PopupProxyRpc.currentInstance_;
this.partner_.partner_=this;this.respondersCount_=0;this.responders_={};c.each(a.remote,c.bind(function(a,b){"redirect"!=b&&(this[b]=function(){var a=c.slice(arguments),d=c.filter(a,c.isFunction),f=d[0],a=c.slice(a,0,a.length-d.length);this.send(b,this.serializeArgs_(a),this.serializeResponder_(f))})},this))},serializeResponder_:function(a){var b=void 0;a&&(b="responder_"+this.respondersCount_++,this.responders_[b]=c.bind(function(b){a.apply(this,this.deserializeArgs_(b))},this));return b},serializeArgs_:function(a){return JSON.stringify(c.slice(a))},
deserializeArgs_:function(a){a=JSON.parse(a);c.isString(a)&&(a=JSON.parse(a));return a},redirect:function(b){g.setLang(b)&&this.window_&&(b=a.XDM.urlForRedirect(this.window_.location.href,b),this.window_.location=b)},send:function(a,b,c){this.partner_&&this.partner_.receive(a,b,c)},receive:function(a,b,d){a=this.rpc.local[a];c.isFunction(a)&&(b=[].concat(this.deserializeArgs_(b)),d&&b.push(c.bind(function(){this.partner_.responders_[d](this.serializeArgs_(c.slice(arguments)))},this)),a.apply(window,
b))},getFrame:function(){},reset:function(){}});a.XDM.PostMessageSocket=f.create({counter:1,responders:{},messageType:{send:0,respond:1},init:function(a,b,f,e,g){this.guid=a;this.localMethods=b;this.remoteMethods=f;this.targetWindow=e;this.targetUrl=g;d.bind(window,"message",c.bind(this.receive,this))},receive:function(a){var b;try{b=JSON.parse(a.data)}catch(d){return}if(c.isObject(b)&&b.g==this.guid&&m(a)==k(this.targetUrl)){var f=b.rid;a=b.a||[];c.isString(a)&&(a=JSON.parse(a));var e=b.t;if(e===
this.messageType.send){if(b=this.localMethods[b.m]){var g;f&&a.push(g=c.bind(function(){this.send(this.serializeResponse(f,c.slice(arguments)))},this));a=b.apply(this,a);void 0!==a&&g&&g(a)}}else e===this.messageType.respond&&(g=this.responders[f])&&g[0].apply(this,a)}},callRemote:function(a,b,c){this.remoteMethods[a]&&this.send(this.serializeMethodCall(a,b,c))},send:function(a){this.targetWindow.postMessage(JSON.stringify(a),k(this.targetUrl))},serializeResponse:function(a,b){return{g:this.guid,
t:this.messageType.respond,rid:a,a:b}},serializeMethodCall:function(a,b,c){var d=c?"r_"+this.counter++:void 0;d&&(this.responders[d]=[c]);return{g:this.guid,t:this.messageType.send,rid:d,m:a,a:b}}});a.XDM.postMessageRpc=f.create({init:function(c,d){this.config=c;this.rpc=d;this.queryParams=b.queryParams();a.XDM.decorate(this);c.remote?this.initSource(d):this.initProvider(d)},isPopup_:function(){return!0==this.config.popup||"popup"==this.queryParams.display},isIframe_:function(){return!this.isPopup_()},
initSource:function(b){this.guid=c.nextId();var d=this.isPopup_()?this.createPopup().getWindow():this.createFrame().contentWindow;this.socket=new a.XDM.PostMessageSocket(this.guid,b.local,b.remote,d,this.config.remote);this.addRemoteMethods(b.remote)},initProvider:function(b){var c=this.queryParams;this.guid=c.xdm_c;this.parentUrl=c.xdm_e;c=this.isPopup_()?window.opener:window.parent;this.socket=new a.XDM.PostMessageSocket(this.guid,b.local,b.remote,"postMessage"in c?c:c.document,this.parentUrl);
this.addRemoteMethods(b.remote)},addRemoteMethods:function(a){c.each(a,c.bind(function(a,b){this[b]=function(){var a=c.slice(arguments),d=c.filter(a,c.isFunction),f=d[0],a=c.slice(a,0,a.length-d.length);this.socket.callRemote(b,a,f)}},this))},createPopup:function(){var b=this.config,c=b.remote,c=c+("\x26xdm_e\x3d"+encodeURIComponent(k(window.location.href))),c=c+("\x26xdm_c\x3d"+encodeURIComponent(this.guid));this.popup=new a.Popup({dimensions:b.dimensions,remote:c});this.popup.open();return this.popup},
createFrame:function(){var a=this.config,b=this.frame=d.create("iframe");b.id=b.name=this.guid+"_provider";var f=a.remote,f=f+("\x26xdm_e\x3d"+encodeURIComponent(k(window.location.href))),f=f+("\x26xdm_c\x3d"+encodeURIComponent(this.guid));b.src=f;f=document.getElementById(this.config.container);n(b,a);f||(f=d.body(),c.extend(b.style,{position:"absolute",top:"-2000px"}));d.append(f,b);return b},destroy:function(){this.getFrame()?this.getFrame().parentNode.removeChild(this.frame):this.getPopup()&&
this.getPopup().destroy()},getPopup:function(){return this.popup},getFrame:function(){return this.frame},reset:function(){}});a.XDM.easyXDMRpc=f.create({easyXDMRpc:void 0,easyXDMLoaded:!1,easyXDMOnLoadCallbacks:[],init:function(b,d){this.origConfig=c.extend({},b);this.config=b;this.rpc=d;c.extend(this.config,{swf:h.easyXDMSwf()});a.XDM.decorate(this);this.config.props&&e.ie&&9>e.version&&(this.config.props.style.display="block");a.XDM.require(c.bind(function(){this.easyXDMRpc=new window.easyXDM.Rpc(b,
d);this.runEasyXDMCallbacks();this.easyXDMLoaded=!0},this));c.each(d.remote,c.bind(function(a,b){this[b]=function(){this.callOrQueue(b,c.slice(arguments))}},this))},callOrQueue:function(a,b){this.easyXDMLoaded?this.easyXDMRpc[a].apply(this.easyXDMRpc,b):this.easyXDMOnLoadCallbacks.push(c.bind(function(){this.easyXDMRpc[a].apply(this.easyXDMRpc,b)},this))},runEasyXDMCallbacks:function(){c.each(this.easyXDMOnLoadCallbacks,function(a){a()})},destroy:function(){this.callOrQueue("destroy")},reset:function(){this.easyXDMRpc=
self.easyXDMLoaded=void 0;this.easyXDMOnLoadCallbacks=[]},getFrame:function(){var a=d.getByTagName(this.config.container,"iframe")[0];e.ie&&9>e.version&&n(a,this.origConfig);return a}});a.XDM=a.XDM;a.XDM.rpcFactory=a.XDM.rpcFactory})(window.fourSq.widget);(function(a){var e=a.Util,f=a.Dom,g=a.Resources;a.ActivityLogger=a.Class.create({init:function(a){a.disabled?this.disabled=!0:(this.config=a,this.id=(a.id||e.nextId())+"_al",this.baseUrl=void 0!==a.baseUrl?a.baseUrl:g.logger(a.sl),this.defaultParams=a.defaultParams||{},this.setup())},setup:function(){var a=f.body();if(a){var c=this.container=f.create("div");e.extend(c.style,{width:"0px",height:"0px",position:"absolute",top:"-1000px",left:"-1000px"});f.append(a,c)}else this.disabled=!0},trigger:function(a,
c){if(!this.disabled)try{var g=f.create("IMG");g.style.height="1px";g.style.width="1px";g.onload=function(){try{g.parentNode.removeChild(g)}catch(a){}};var b=this.baseUrl+"?id\x3d"+this.id,b=b+("\x26dt\x3d"+ +new Date),b=b+("\x26activity\x3d"+encodeURIComponent(a));(c=e.extend(c||{},this.defaultParams||{}))&&(e.isObject(c)?e.each(c,function(a,c){var d=void 0;if(void 0!==a){if(e.isObject(a)||e.isArray(a))d=JSON.stringify(a);else{if(e.isFunction(a))return;d=a}b+="\x26"+c+"\x3d"+encodeURIComponent(d)}}):
e.isString(c)&&(b+="\x26"+c));g.src=b;this.container.appendChild(g)}catch(l){}}});a.ActivityLogger=a.ActivityLogger;a.ActivityLogger.trigger=a.ActivityLogger.trigger})(window.fourSq.widget);(function(a){var e=a.Util,f=!1,g=[];a.TooltipFactory={create:function(d,c,e){e=e||function(){};f?e(new a.Tooltip(d,c)):g.push({element:d,config:c,callback:e})},libLoaded:function(){e.each(g,function(d){d.callback(new a.Tooltip(d.element,d.config))});f=!0}}})(window.fourSq.widget);(function(a){var e=a.Util,f=a.Events,g=a.Config,d=a.Dom,c=new a.ActivityLogger({sl:!0,disabled:5!=Math.floor(10*Math.random())}),h=a.AbstractWidget=a.Class.create({init:function(a){a=a||{};this.config=e.extend({},g.global(),a,{guid:this.guid=e.nextId()});var c=this;e.each(a,function(a,b){if(0==b.indexOf("on")){var d=b.substring(2).toLowerCase();c.bind(d,function(){e.isString(a)&&e.isFunction(window[a])?window[a].apply(window,arguments):e.isFunction(a)&&a.apply(window,arguments)})}})},addToDom:function(a,
c){var f=d.create("div");f.id=this.guid;d[c](a,f);this.createButton();return this},append:function(a){return this.addToDom(a,"append")},replace:function(a){return this.addToDom(a,"replace")},attach:function(a){return this.replace(a)},getVariant:function(){return this.config.variant},isVariant:function(a){a=e.toArray(e.isString(a)?[a]:a);for(var c=this.getVariant(),d=0,f=a.length;d<f;d++)if(c==a[d])return!0;return!1},createButton:function(){},open:function(){}});e.extend(h.prototype,f);a.AbstractImageButtonWidget=
h.extend({init:function(a){h.prototype.init.call(this,a)},getButtonDimensions:function(){return{width:58,height:20}},getButtonContainerStyles:function(){var a=this.getButtonDimensions();return e.extend({border:"none",overflow:"hidden",display:"block",cursor:"pointer",position:"relative"},{height:a.height+"px",width:a.width+"px"})},getBackgroundImgSrc:function(){},getButtonAnchorStyles:function(){var a=this.getBackgroundImgSrc();return e.extend(this.getButtonContainerStyles(),{background:"url("+a+
") no-repeat scroll 0 0 transparent"})},bindButtonAnchorEvents:function(a){var c=this.getButtonDimensions().height;d.bind(a,"mouseover",function(){a.style.backgroundPosition="0 -"+c+"px"});d.bind(a,"mouseout",function(){a.style.backgroundPosition="0 0"});d.bind(a,"mousedown",function(){a.style.backgroundPosition="0 -"+2*c+"px"});var f=this;d.bind(a,"click",function(){f.open()})},logRenderExt_:function(){return{}},logRender_:function(a){a=this.logRenderExt_();a.guid=this.guid;c.trigger("imp",a)},createButton:function(){var a=
document.getElementById(this.guid);e.extend(a.style,this.getButtonContainerStyles());var c=d.create("a");e.extend(c.style,this.getButtonAnchorStyles());this.bindButtonAnchorEvents(c);d.append(a,c);this.trigger("load",this);this.logRender_(a)},attach:function(a){this.bindButtonAnchorEvents(a);this.trigger("load",this)}});h.prototype.append=h.prototype.append;h.prototype.replace=h.prototype.replace})(window.fourSq.widget);(function(a){var e=a.Browser,f=a.Resources,g=a.Util,d=a.AbstractImageButtonWidget,c=[2,"wide"];a.SaveTo=d.extend({init:function(a){d.prototype.init.call(this,a)},logRenderExt_:function(){return{type:"saveto"}},isWide:function(){return this.isVariant(c)},getBackgroundImgSrc:function(){var a="save";this.isWide()&&(a="save-w");return f.asset(a)},createButton:function(){d.prototype.createButton.apply(this,arguments);if("true"!=this.config.hideTooltip&&!e.isTouchDevice){var c=this.isWide()?"Remember this place for later.":
"Save to foursquare to remember this place for later.";a.TooltipFactory.create(document.getElementById(this.guid),{text:c})}},getButtonDimensions:function(){var a=d.prototype.getButtonDimensions.apply(this,arguments);this.isWide()&&g.extend(a,{width:126});return a}});a.SaveTo=a.SaveTo})(window.fourSq.widget);(function(a){var e=a.Browser,f=a.Resources,g=a.Util,d=a.AbstractImageButtonWidget,c=[2,"wide"];a.CreateTip=d.extend({init:function(a){d.prototype.init.call(this,a);this.endpointUrl_=a.endpointUrl},logRenderExt_:function(){return{type:"createtip"}},isWide:function(){return this.isVariant(c)},getBackgroundImgSrc:function(){var a="createtip";this.isWide()&&(a="createtip-w");return f.asset(a)},createButton:function(){d.prototype.createButton.apply(this,arguments);if("true"!=this.config.hideTooltip&&!e.isTouchDevice){var c=
this.isWide()?"Remember this place for later.":"Save to foursquare to remember this place for later.";a.TooltipFactory.create(document.getElementById(this.guid),{text:c})}},getButtonDimensions:function(){var a=d.prototype.getButtonDimensions.apply(this,arguments);this.isWide()&&g.extend(a,{width:126});return a}});a.CreateTip=a.CreateTip})(window.fourSq.widget);(function(a){var e=a.Browser,f=a.Dom,g=a.Resources,d=a.Util,c=a.AbstractImageButtonWidget,h=[2,"wide"];a.Follow=a.Like=c.extend({init:function(a){c.prototype.init.call(this,a)},logRenderExt_:function(){return{type:"like"}},hasUserName:function(){return!!this.config.userName},isWide:function(){return this.isVariant(h)},getButtonDimensions:function(){var a=c.prototype.getButtonDimensions.apply(this,arguments);this.isWide()?d.extend(a,{width:149}):d.extend(a,{width:64});return a},getBackgroundImgSrc:function(){var a=
"follow";this.isWide()&&(a="followus-w");return g.asset(a)},createButton:function(){c.prototype.createButton.apply(this,arguments);var b=document.getElementById(this.guid),g=f.getByTagName(b,"a")[0];this.hasUserName()&&(d.extend(b.style,{width:"auto"}),d.extend(g.style,{margin:"0 5px 0 0",cssFloat:"left",styleFloat:"left"}),g=f.create("a"),g.setAttribute("href","https://foursquare.com/user/"+this.config.fuid),g.setAttribute("target","_blank"),f.append(g,f.createText(this.config.userName)),f.append(b,
g));this.isWide()||"true"===this.config.hideTooltip||e.isTouchDevice||a.TooltipFactory.create(b,{text:"Follow "+(this.config.userName||"us")+" on Foursquare"})}});a.Follow=a.Follow;a.Like=a.Like})(window.fourSq.widget);(function(a){var e=a.Util,f=a.Dom,g=a.Factory={findMarkers:function(a){return f.getByClassName(a,"fourSq-widget")},markerConfig:function(a){var c={};e.each((a||{}).attributes||[],function(a){if(0==a.name.indexOf("data-")){var b=e.camelCase(a.name.substring(5));c[b]=a.value}});return c},go:function(a){a=g.findMarkers(a);e.each(a,function(a){var d=g.markerConfig(a),b=g.instance(d);"false"==d.replaceButton?b.attach(a):b.replace(a)})},instance:function(d){var c;switch(d.type){case "follow":case "like":c=
a.Follow;break;default:c=a.SaveTo}return new c(d)}}})(window.fourSq.widget);(function(a){var e=a.Config,f=a.Resources,g=a.Util,d=[];window.JSON&&window.JSON.stringify||d.push(f.json2());g.require(d,function(){e.callIfDeclared("onInit");e.global("explicit",!1)||a.Factory.go();window.setTimeout(function(){a.XDM.require(function(){e.callIfDeclared("onRpcReady");e.global("parse",!0)?g.require(f.asyncBundle(),function(){e.callIfDeclared("onParseReady");e.callIfDeclared("onReady")}):e.callIfDeclared("onReady")})},10)})})(window.fourSq.widget);