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
                }
            );
        }
    });
}