/**
 * URL Parameters and protocol description are here:
 *
 * https://www.drawio.com/doc/faq/supported-url-parameters
 *
 * Parameters for developers:
 *
 * - dev=1: For developers only
 * - test=1: For developers only
 * - export=URL for export: For developers only
 * - ignoremime=1: For developers only (see DriveClient.js). Use Cmd-S to override mime.
 * - createindex=1: For developers only (see etc/build/README)
 * - filesupport=0: For developers only (see Editor.js in core)
 * - savesidebar=1: For developers only (see Sidebar.js)
 * - pages=1: For developers only (see Pages.js)
 * - lic=email: For developers only (see LicenseServlet.java)
 * --
 * - networkshapes=1: For testing network shapes (temporary)
 */
// Parses URL parameters
var urlParams = (function()
{
    var result = new Object();
    var params = window.location.search.slice(1).split('&');
    
    for (var i = 0; i < params.length; i++)
    {
        var idx = params[i].indexOf('=');
        
        if (idx > 0)
        {
            result[params[i].substring(0, idx)] = params[i].substring(idx + 1);
        }
    }
    
    return result;
})();

// Forces CDN caches by passing URL parameters via URL hash
if (window.location.hash != null && window.location.hash.substring(0, 2) == '#P')
{
    try
    {
        urlParams = JSON.parse(decodeURIComponent(window.location.hash.substring(2)));
        
        if (urlParams.hash != null)
        {
            window.location.hash = urlParams.hash;
        }
    }
    catch (e)
    {
        // ignore
    }
}

// Global variable for desktop
var mxIsElectron = navigator.userAgent != null && navigator.userAgent.toLowerCase().indexOf(' electron/') > -1 && 
                    navigator.userAgent.indexOf(' draw.io/') > -1;

// Redirects page if required
if (urlParams['dev'] != '1')
{
    (function()
    {
        var proto = window.location.protocol;
        
        if (!mxIsElectron)
        {
            var host = window.location.host;

            // Redirects apex, drive and rt to www
            if (host === 'draw.io' || host === 'rt.draw.io' || host === 'drive.draw.io')
            {
                host = 'www.draw.io';
            }
            
            var href = proto + '//' + host + window.location.href.substring(
                    window.location.protocol.length +
                    window.location.host.length + 2);

            // Redirects if href changes
            if (href != window.location.href)
            {
                window.location.href = href;
            }
        }
    })();
}

/**
 * Adds meta tag to the page.
 */
function mxmeta(name, content, httpEquiv)
{
    try
    {
        var s = document.createElement('meta');
        
        if (name != null) 
        {
            s.setAttribute('name', name);
        }

        s.setAttribute('content', content);
        
        if (httpEquiv != null) 
        {
            s.setAttribute('http-equiv', httpEquiv);
        }

        var t = document.getElementsByTagName('meta')[0];
        t.parentNode.insertBefore(s, t);
    }
    catch (e)
    {
        // ignore
    }
};

/**
 * Synchronously adds scripts to the page.
 */
function mxscript(src, onLoad, id, dataAppKey, noWrite, onError)
{
    var defer = onLoad == null && !noWrite;
    
    if ((urlParams['dev'] != '1' && typeof document.createElement('canvas').getContext === "function") ||
        onLoad != null || noWrite)
    {
        var s = document.createElement('script');
        s.setAttribute('type', 'text/javascript');
        s.setAttribute('defer', 'true');
        s.setAttribute('src', src);

        if (id != null)
        {
            s.setAttribute('id', id);
        }
        
        if (dataAppKey != null)
        {
            s.setAttribute('data-app-key', dataAppKey);
        }
        
        if (onLoad != null)
        {
            var r = false;
        
            s.onload = s.onreadystatechange = function()
            {
                if (!r && (!this.readyState || this.readyState == 'complete'))
                {
                    r = true;
                    onLoad();
                }
            };
        }

        if (onError != null)
        {
            s.onerror = function(e)
            {
                onError('Failed to load ' + src, e);
            };
        }
        
        var t = document.getElementsByTagName('script')[0];
        
        if (t != null)
        {
            t.parentNode.insertBefore(s, t);
        }
    }
    else
    {
        document.write('<script src="' + src + '"' + ((id != null) ? ' id="' + id +'" ' : '') +
            ((dataAppKey != null) ? ' data-app-key="' + dataAppKey +'" ' : '') + '></scr' + 'ipt>');
    }
};

/**
 * Asynchronously adds scripts to the page.
 */
function mxinclude(src)
{
    var g = document.createElement('script');
    g.type = 'text/javascript';
    g.async = true;
    g.src = src;
    
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(g, s);
};

/**
 * Adds meta tags with application name (depends on offline URL parameter)
 */
(function()
{
    var name = 'diagrams.net';
    mxmeta('apple-mobile-web-app-title', name);
    mxmeta('application-name', name);

    if (mxIsElectron)
    {
        mxmeta(null, 'default-src \'self\'; script-src \'self\' \'sha256-6g514VrT/cZFZltSaKxIVNFF46+MFaTSDTPB8WfYK+c=\' ' +
            (urlParams['dev'] != '1' ? '' : ' \'unsafe-eval\'') + '; ' +
            'connect-src \'self\' https://*.draw.io https://*.diagrams.net https://fonts.googleapis.com https://fonts.gstatic.com; ' +
            'img-src * data:; media-src *; font-src *; frame-src \'none\'; style-src \'self\' \'unsafe-inline\' ' +
            'https://fonts.googleapis.com; base-uri \'none\';child-src \'self\';object-src \'none\';', 'Content-Security-Policy');
    }
})();

// Checks for local storage
var isLocalStorage = false;

try
{
    isLocalStorage = urlParams['local'] != '1' && typeof(localStorage) != 'undefined';
}
catch (e)
{
    // ignored
}

var mxScriptsLoaded = false, mxWinLoaded = false;

function checkAllLoaded()
{
    if (mxScriptsLoaded && mxWinLoaded)
    {
        App.main();				
    }
};

var t0 = new Date();

// Changes paths for local development environment
if (urlParams['dev'] == '1')
{
    // Used to request grapheditor/mxgraph sources in dev mode
    var mxDevUrl = '';
    
    // Used to request draw.io sources in dev mode
    var drawDevUrl = '';
    var geBasePath = 'js/grapheditor';
    var mxBasePath = 'mxgraph/src';
    
    if (document.location.protocol == 'file:')
    {
        // Forces includes for dev environment in node.js
        mxForceIncludes = true;
    }

    mxForceIncludes = false;

    mxscript(drawDevUrl + 'js/PreConfig.js');
    mxscript(drawDevUrl + 'js/diagramly/Init.js');
    mxscript(geBasePath + '/Init.js');
    mxscript(mxBasePath + '/mxClient.js');
    
    // Adds all JS code that depends on mxClient. This indirection via Devel.js is
    // required in some browsers to make sure mxClient.js (and the files that it
    // loads asynchronously) are available when the code loaded in Devel.js runs.
    mxscript(drawDevUrl + 'js/diagramly/Devel.js');
    
    // Electron
    if (mxIsElectron)
    {
        mxscript('js/desktop/DesktopLibrary.js');
        mxscript('js/desktop/ElectronApp.js');
    }
    
    mxscript(drawDevUrl + 'js/PostConfig.js');
}
else
{
    (function()
    {
        var hostName = window.location.hostname;
        
        // Supported domains are *.draw.io and the packaged version in Quip
        var supportedDomain = (hostName.substring(hostName.length - 8, hostName.length) === '.draw.io') ||
            (hostName.substring(hostName.length - 13, hostName.length) === '.diagrams.net');
        
        function loadAppJS()
        {
            mxscript('js/app.min.js', function()
            {
                mxScriptsLoaded = true;
                checkAllLoaded();
                
                // Electron
                if (mxIsElectron)
                {
                    mxscript('js/diagramly/DesktopLibrary.js', function()
                    {
                        mxscript('js/diagramly/ElectronApp.js', function()
                        {
                            mxscript('js/extensions.min.js', function()
                            {
                                mxscript('js/stencils.min.js', function()
                                {
                                    mxscript('js/shapes-14-6-5.min.js', function()
                                    {
                                        mxscript('js/PostConfig.js');
                                    });
                                });
                            });
                        });
                    });
                }
                else if (!supportedDomain || navigator.onLine)
                {
                    mxscript('js/PostConfig.js');
                }
            });
        };
        
        if (!supportedDomain || mxIsElectron || navigator.onLine)
        {
            mxscript('js/PreConfig.js', loadAppJS);
        }
        else
        {
            loadAppJS();
        }
    })();
}

// Adds basic error handling
window.onerror = function()
{
    var status = document.getElementById('geStatus');
    
    if (status != null)
    {
        status.innerHTML = 'Page could not be loaded. Please try refreshing.';
    }
};


