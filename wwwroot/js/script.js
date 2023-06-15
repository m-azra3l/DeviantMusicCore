var recentTweetsHeightHandler = function() {
    var element = document.getElementById('recent-tweets');
    var windowWidth = window.innerWidth;
    var heightSample =  document.getElementById('ad-rotator-carousel-container').offsetHeight;
    var height = heightSample - 48;
    if (!isNull(element)) {
        if (windowWidth >= 577 && windowWidth <= 991) {
            element.style.height = `${height}px`;
        } else {
            element.style.height = '';
        }
    }
}

window.addEventListener('resize', recentTweetsHeightHandler);
recentTweetsHeightHandler();