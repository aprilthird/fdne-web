function childWalking(element) {
    let e = element;
    while (e.children.length > 0) {
        e = e.children[0];
    }
    return e;
}

function active(element, name) {
    const e = document.getElementsByClassName(name);
    if (e.length > 0) {
        e[0].classList.remove(name);
    } else {
        console.log('class name not found');
    }
    element.classList.add(name);
}

function standout(e) {
    const className = 'js-standout-container';
    const activeClassName = 'link--active';
    const apostrophe = '&apos;';
    const element = childWalking(e);

    let container = document.getElementsByClassName(className);
    if (container.length > 0) {
        container = childWalking(container[0]);
        container.textContent = element.textContent;
    } else {
        console.log('object by class ' + className + ' not found');
    }

    active(e, activeClassName);
}
