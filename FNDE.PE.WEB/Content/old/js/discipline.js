(function() {
    function execute() {
        let pictures = new Pictures().init(); 
        let liker = new LinkMenu();
    }

    let options = { once: true };
    document.addEventListener('DOMContentLoaded', execute, options);
})();
