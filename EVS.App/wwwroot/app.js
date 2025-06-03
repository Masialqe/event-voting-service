window.LocalStorageHelper = {
    setItem: function (key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },

    getItem: function (key) {
        let item = localStorage.getItem(key);
        return item ? JSON.parse(item) : null;
    },

    removeItem: function (key) {
        localStorage.removeItem(key);
    }
};