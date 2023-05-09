(function () {
    'use strict';
    var slides = document.querySelectorAll('.carousel_item'),
        prevAnchor = document.querySelector('.carousel_anchor.prev'),
        nextAnchor = document.querySelector('.carousel_anchor.next'),
        carouselCount = 0,
        scrollTimeout,
        video = document.querySelector('video'),
        schedule = {
            first: 15000,
            default: 5000,
        };

    // prev anchor setting  
    if (prevAnchor) {

        prevAnchor.addEventListener('click', function (e) {
            e = e || window.event;
            e.preventDefault();
            carouselCount -= 100;
            slider();
            if (e.type !== 'autoClick') {
                clearTimeout(scrollTimeout);
                if (carouselCount == 0) {
                    scrollTimeout = setTimeout(autoScroll, schedule.first);
                } else {
                    scrollTimeout = setTimeout(autoScroll, schedule.default);
                }
            }
        });
    }

    // next anchor event 
    if (nextAnchor) {

        nextAnchor.addEventListener('click', sliderEvent);
        nextAnchor.addEventListener('autoClick', sliderEvent);
    }

    function sliderEvent(e) {

        e = e || window.event;
        e.preventDefault();
        carouselCount += 100;
        slider();
        if (e.type !== "autoClick") {
            //clearInterval(scrollTimeout);
            clearTimeout(scrollTimeout);
            if (carouselCount == 0) {
                scrollTimeout = setTimeout(autoScroll, schedule.first);
            } else {
                scrollTimeout = setTimeout(autoScroll, schedule.default);
            }
        }
    }

    function slider() {
        carouselCount = (carouselCount == -100) ? 0 : (carouselCount == slides.length * 100) ? 0 : carouselCount;
    
        // reset first video
        if (carouselCount == 0) {

            video.currentTime = 0;
            video.play();
        }
        for (var i = 0; i < slides.length; i += 1) {
            slides[i].setAttribute('style', 'transform:translateX(-' + carouselCount + '%)');
        }
    }

    // create new Event to dispatch click for auto scroll
    var autoClick = new Event('autoClick');
    function autoScroll() {
        nextAnchor.dispatchEvent(autoClick);
        if (carouselCount == 0) {
            scrollTimeout = setTimeout(autoScroll, schedule.first);
        } else {
            scrollTimeout = setTimeout(autoScroll, schedule.default);
        }
    }

    // set timing of dispatch click events
    //scrollTimeout = setInterval(autoScroll, interval);
    scrollTimeout = setTimeout(autoScroll, schedule.first);
    // start video first time
    video.play();
})();