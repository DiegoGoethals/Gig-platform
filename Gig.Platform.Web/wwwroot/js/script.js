function getCurrentLocation() {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject('Geolocation is not supported by your browser');
        } else {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve(JSON.stringify({
                        Latitude: position.coords.latitude,
                        Longitude: position.coords.longitude
                    }));
                },
                (error) => {
                    switch (error.code) {
                        case error.PERMISSION_DENIED:
                            reject('User denied the request for Geolocation.');
                            break;
                        case error.POSITION_UNAVAILABLE:
                            reject('Location information is unavailable.');
                            break;
                        case error.TIMEOUT:
                            reject('The request to get user location timed out.');
                            break;
                        default:
                            reject('An unknown error occurred.');
                            break;
                    }
                },
                {
                    enableHighAccuracy: true, // Request high accuracy, typically using GPS
                    timeout: 5000, // 5 seconds timeout
                    maximumAge: 0 // Don't use cached location data
                }
            );
        }
    });
}

function getBrowserLanguage() {
    return (navigator.languages && navigator.languages.length) ? navigator.languages[0] :
        navigator.userLanguage || navigator.language || navigator.browserLanguage || 'en';
}