(function () {
    const TRACKING_ENDPOINT = 'api/Tracking/TrackEvent';
    const STORAGE_KEY = 'sfmcPageVisits';
    const TRACKING_KEY = '<Your Data Extension Tracking Key>';

    const visitStartTime = Date.now();

    const extractDevice = (userAgent) => {
        if (/mobile/i.test(userAgent)) return 'Mobile';
        if (/tablet|ipad|playbook|silk/i.test(userAgent)) return 'Tablet';
        if (/android/i.test(userAgent) && !/mobile/i.test(userAgent)) return 'Tablet';
        if (/windows|macintosh|linux/i.test(userAgent)) return 'Desktop';
        return 'Unknown';
    };

    const getPayload = () => {
        const userAgent = navigator.userAgent;
        const userEmail = window.SitecoreContext?.userEmail || 'anonymous';
        return {
            pageUrl: window.location.href,
            device: extractDevice(userAgent),
            email: userEmail,
            timestamp: new Date().toISOString()
        };
    };

    const addToQueue = (payload) => {
        const queue = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');
        queue.push(payload);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(queue));
    };

    const flushQueue = () => {
        const queue = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');
        if (queue.length === 0) return;

        const enriched = queue.map(item => ({
            PageUrl: item.pageUrl,
            Device: item.device,
            Email: item.email,
            TimeOnPageInSeconds: Math.round((Date.now() - visitStartTime) / 1000)
        }));

        const payload = {
            TrackingKey: TRACKING_KEY,
            EventData: enriched
        };

        const jsonBlob = new Blob([JSON.stringify(payload)], { type: 'application/json' });

        const success = navigator.sendBeacon
            ? navigator.sendBeacon(TRACKING_ENDPOINT, jsonBlob)
            : sendWithFetch(jsonBlob);

        if (success !== false) {
            localStorage.removeItem(STORAGE_KEY);
        }
    };

    const sendWithFetch = (blob) => {
        try {
            fetch(TRACKING_ENDPOINT, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: blob
            });
            return true;
        } catch (e) {
            console.error('Tracking fetch failed:', e);
            return false;
        }
    };

    // Run on load
    const payload = getPayload();
    addToQueue(payload);

    // Send on unload or tab hidden
    window.addEventListener('beforeunload', flushQueue);
    document.addEventListener('visibilitychange', () => {
        if (document.visibilityState === 'hidden') flushQueue();
    });
})();
