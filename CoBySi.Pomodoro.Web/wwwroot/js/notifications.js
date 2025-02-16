window.requestNotificationPermission = () => {
    return Notification.requestPermission();
}

window.showNotification = (title, options) => {
    if (Notification.permission === 'granted') {
        const n = new Notification(title, options);
        document.addEventListener("visibilitychange", () => {
            if (document.visibilityState === "visible") {
                // The tab has become visible so clear the now-stale Notification.
                n.close();
            }
        });
    }
}

window.playSound = (sound) => {
    document.getElementById(sound).play();
}