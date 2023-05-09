(function () {
    class BasicNavbar {

        constructor(addons) {
            let _ = this;
            let _wrapper = $1(".f-navbar__list");

            _.def = {
                wrapper: _wrapper,
                anchors: $(":scope > .f-navbar__item", _wrapper),
                line: $1(":scope > .f-navbar__line", _wrapper),
                underline: $1(":scope > .f-navbar__underline", _wrapper),
            };

            if (!_.def.anchors) {
                return;
            }

            _.items = [];
            for (let el of _.def.anchors) {
                let sibling = el.firstElementChild.nextElementSibling,
                    style = $computedStyle(el.firstElementChild, null),
                    item = {
                        target: el,
                        submenu: {
                            target: sibling,
                            children: sibling ? sibling.children : null,
                        },
                        width: style.width,
                        left: style.left,
                    };
                _.items.push(item);
            };

            _.def.anchors = null;
        }

        initDropdowns(delay) {
            let _ = this;

            for (let item of _.items) {
                if (!item.submenu.target) {
                    return;
                }

                item.target.addEventListener("mouseover", function (event) {
                    if (item.submenu.children.length > 0) {
                        let i = 0;
                        for (let child of item.submenu.children) {
                            child.style.opacity = "1";
                            child.style.transitionDelay = `${(i++) * (delay || 0.1)}s`;
                        }
                    }
                }, false);

                item.target.addEventListener("mouseout", function (event) {
                    let e = event.relatedTarget;
                    if (e.parentNode == this || e == this) {
                        return;
                    }
                    for (let child of item.submenu.children) {
                        child.style.opacity = "0";
                        child.style.transitionDelay = "0s";
                    }
                }, false);
            };
            return _;
        }

        initUnderline() {
            let _ = this;
            if (!(_.def.line) && !(_.def.underline)) {
                return;
            }

            for (let item of _.items) {
                item.target.addEventListener("mouseover", function (event) {
                    _.def.line.style.width = "100%";
                    _.def.underline.style.width = `${item.width}px`;
                    _.def.underline.style.left = `${item.left}px`;
                    _.def.underline.style.opacity = "1";
                }, false);

                item.target.addEventListener("mouseout", function (event) {
                    let e = event.relatedTarget;
                    if (e.parentNode == this || e == this) {
                        return;
                    }

                    _.def.line.style.width = "0%";
                    _.def.underline.style.opacity = "0";

                }, false);
            }
        }
    }

    // TODO: Make closure
    let menu = new BasicNavbar();
    menu.initDropdowns();
    menu.initUnderline();
})();
