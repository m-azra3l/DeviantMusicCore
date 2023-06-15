'use strict';

// Cookie utility functions

// utility function to retrieve a future expiration date in proper format;
// pass three integer parameters for the number of days, hours,
// and minutes from now you want the cookie to expire; all three
// parameters required, so use zeros where appropriate
function getExpDate(days, hours, minutes) {
    var expDate = new Date();
    if (typeof days == "number" && typeof hours == "number" && typeof hours == "number") {
        expDate.setDate(expDate.getDate() + parseInt(days));
        expDate.setHours(expDate.getHours() + parseInt(hours));
        expDate.setMinutes(expDate.getMinutes() + parseInt(minutes));
        return expDate.toUTCString();
    }
}

// utility function called by getCookie()
function getCookieVal(offset) {
    var endstr = document.cookie.indexOf (";", offset);
    if (endstr == -1) {
        endstr = document.cookie.length;
    }
    return unescape(document.cookie.substring(offset, endstr));
}

// primary function to retrieve cookie by name
function getCookie(name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while (i < clen) {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg) {
            return getCookieVal(j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) break; 
    }
    return null;
}

// store cookie value with optional details as needed
function setCookie(name, value, expires, path, domain, secure) {
    document.cookie = name + "=" + escape (value) +
        ((expires) ? "; expires=" + expires : "") +
        ((path) ? "; path=" + path : "") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
}

// remove the cookie by setting ancient expiration date
function deleteCookie(name, path, domain) {
    if (getCookie(name)) {
        document.cookie = name + "=" +
            ((path) ? "; path=" + path : "") +
            ((domain) ? "; domain=" + domain : "") +
            "; expires=Thu, 01-Jan-70 00:00:01 UTC";
    }
}

// capitalizes the first latter of a string
function capsFirstLetter(string) {
	return string.charAt(0).toUpperCase() + string.substring(1);
}

function URLParameterExists(paramaterName) {
	var URLParams = new URLSearchParams(location.search);
	if (URLParams.has(paramaterName)) {
		return true;
	} else {
		return false;
	}
}

function URLParameterNotEmpty(paramaterName) {
	var URLParams = new URLSearchParams(location.search);
    if (URLParams.get(paramaterName) !== '') {
        return true;
    } else {
        return false;
    }
}

function getURLParameter(paramaterName) {
	var URLParams = new URLSearchParams(location.search);
    return URLParams.get(paramaterName);
}

function isNumber(object) {
    if (typeof object === 'number') {
        return true;
    } else {
        return false;
    }
}

function isInteger(object) {
    if (Number.isInteger(object)) {
        return true;
    } else {
        return false;
    }
}

function isString(object) {
    if (typeof object === 'string') {
        return true;
    } else {
        return false;
    }
}

function isBoolean(object) {
    if (typeof object === 'boolean') {
        return true;
    } else {
        return false;
    }
}

function isUndefined(object) {
    if (typeof object === 'undefined') {
        return true;
    } else {
        return false;
    }
}

function isFunction(object) {
    if (typeof object === 'function') {
        return true;
    } else {
        return false;
    }
}

function isNull(object) {
    if (object === null) {
        return true;
    } else {
        return false;
    }
}

function isArray(object) {
    if (Array.isArray(object)) {
        return true;
    } else {
        return false;
    }
}

function isEmptyArray(object) {
    if (object.length === 0) {
        return true;
    } else {
        return false;
    }
}

function isObject(object) {
    if (object instanceof Object && !Array.isArray(object)) {
        return true;
    } else {
        return false;
    }
}

function isEmptyObject(object) {
    if (Object.keys(object).length === 0) {
        return true;
    } else {
        return false;
    }
}

// checks if a string is a valid JSON string or not
function isJSONString(string) {
	try {
		JSON.parse(string);
		return true;
	} catch (SyntaxError) {
		return false;
	}
}

function localStorageAvailable() {
    var storage;
    try {
        storage = window['localStorage'];
        var x = '__storage_test__';
        storage.setItem(x, x);
        storage.removeItem(x);
        return true;
    } catch(e) {
        return e instanceof DOMException && (
            // everything except Firefox
            e.code === 22 ||
            // Firefox
            e.code === 1014 ||
            // test name field too, because code might not be present
            // everything except Firefox
            e.name === 'QuotaExceededError' ||
            // Firefox
            e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
            // acknowledge QuotaExceededError only if there's something already stored
            (storage && storage.length !== 0);
    }
}

function sessionStorageAvailable() {
    var storage;
    try {
        storage = window['sessionStorage'];
        var x = '__storage_test__';
        storage.setItem(x, x);
        storage.removeItem(x);
        return true;
    } catch(e) {
        return e instanceof DOMException && (
            // everything except Firefox
            e.code === 22 ||
            // Firefox
            e.code === 1014 ||
            // test name field too, because code might not be present
            // everything except Firefox
            e.name === 'QuotaExceededError' ||
            // Firefox
            e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
            // acknowledge QuotaExceededError only if there's something already stored
            (storage && storage.length !== 0);
    }
}

function browserStorageAvailable() {
	if (localStorageAvailable() && sessionStorageAvailable()) {
		return true;
	} else {
		return false;
	}
}

function itemExistsInLocalStorage(itemKey) {
	if (localStorage.getItem(itemKey) !== null) {
		return true;
	} else {
		return false;
	}
}

function itemExistsInSessionStorage(itemKey) {
	if (sessionStorage.getItem(itemKey) !== null) {
		return true;
	} else {
		return false;
	}
}

function itemExistsInBrowserStorage(itemKey) {
	if (itemExistsInLocalStorage(itemKey) || itemExistsInSessionStorage(itemKey)) {
		return true;
	} else {
		return false;
	}
}

function clearLocalStorage() {
	localStorage.clear();
}

function clearSessionStorage() {
	sessionStorage.clear();
}

function clearBrowserStorage() {
	clearSessionStorage();
	clearLocalStorage();
}

function indexedDBAvailable() {
    if ('indexedDB' in window) {
		return true;
	} else {
		return false;
	}
}

function clearCacheStorage() {
	caches.keys().then(function(keyList) {
		if(keyList.length > 0) {
			return Promise.all(keyList.map(function(key) {
				return caches.delete(key).then(function(boolean) {
					if (boolean) {
						console.log('Deleted cache ' + key + '.');
					} else {
						console.log('Could not delete cache ' + key + '.');
					}
				});
			})).then(function() {
				console.log('Cache storage cleared successfully.');
			});
		} else {
			console.log('Cache storage is empty.');
		}
    });
}

function browserSupportsServiceWorker() {
	if ('serviceWorker' in navigator) {
		return true;
	} else {
		return false;
	}
}

function browserSupportsNotification() {
	if ('Notification' in window) {
		return true;
	} else {
		return false;
	}
}

function browserSupportsGeolocation() {
	if ('geolocation' in navigator) {
		return true;
	} else {
		return false;
	}
}

function notificationPermissionGranted() {
	if (Notification.permission === 'granted') {
		return true;
	} else {
		return false;
	}
}

function notificationPermissionDenied() {
	if (Notification.permission === 'denied') {
		return true;
	} else {
		return false;
	}
}

function notificationPermissionNeverRequested() {
	if (Notification.permission === 'default') {
		return true;
	} else {
		return false;
	}
}

function logError(object) {
	console.error(object);
}

function logWarning(object) {
	console.warn(object);
}

function logInfo(object) {
	console.info(object);
}

function logMessage(object) {
	console.log(object);	
}

function reloadPage() {
	location.reload();
}