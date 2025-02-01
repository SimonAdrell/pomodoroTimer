window.requestNotificationPermission = () => {
    return Notification.requestPermission();
}

window.showNotification = (title, options) => {
    if (Notification.permission === 'granted') {
        new Notification(title, options);
    }
}

window.playSound = (sound) => {
    document.getElementById(sound).play();
}