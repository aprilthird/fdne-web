(function() {
    function execute() {
        let pictures = new Pictures({
            figures: $('.picture'),
            mediaClass: '.media'
        }).init(); 
        let swiper = new SwiperPlugin({
            container: $1('.gallery_mask'),
            draggable: $1('.gallery_album'),
            options: {
                abscissa: false,
                ordinate: true,
                limitless: false,
            },
            variables: {
                className: 'gallery_translate',
                currentX: '--ix',
                currentY: '--iy',
                translateX: '--tx',
                translateY: '--ty',
            }
        }).init();
    }

    let options = { once:true };
    document.addEventListener('DOMContentLoaded', execute, options);
})();
