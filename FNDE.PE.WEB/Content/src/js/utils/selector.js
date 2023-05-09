var $1 = (el, context) => {
    return (context || document).querySelector(el);
};

var $ = (el, context) => {
    return (context || document).querySelectorAll(el);
};

var $computedStyle = (el, pseudo) => {
    let cs = window.getComputedStyle(el, pseudo);
    return {
        width: parseFloat(cs.width),
        left: parseFloat(el.offsetLeft) + parseFloat(cs.paddingLeft),
    };
};