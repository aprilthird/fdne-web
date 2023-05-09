(function() {
    function execute() {
        let pictures = new Pictures().init(); 
    }

    let options = { once: true };
    document.addEventListener('DOMContentLoaded', execute, options);
})();
