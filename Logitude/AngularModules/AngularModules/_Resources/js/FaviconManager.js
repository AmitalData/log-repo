function changeFavicon(src) {
    var link = document.createElement('link');
        oldLink = document.getElementById('dynamic-favicon');
    link.id = 'dynamic-favicon';
    link.rel = 'icon';
    link.href = src;

    if (oldLink) {
        document.head.removeChild(oldLink);
    }
    document.head.appendChild(link);
}
function changeTitle(title) {
    document.title = title;
}
