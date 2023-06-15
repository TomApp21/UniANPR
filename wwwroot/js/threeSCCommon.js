function getWindowSize() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

function getCurrentPageTitle() {
    return document.title;
};

function reloadSite() {
    location.reload();
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            alert("Copied to clipboard");
        })
            .catch(function (error) {
                alert(error);
            });
    }
};