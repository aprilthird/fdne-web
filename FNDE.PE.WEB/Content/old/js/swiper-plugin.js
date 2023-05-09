let SwiperPlugin = (function() {

    class swiper {

        constructor(addons) {
            if(typeof($extendObj) == "undefined") {
                console.log("load utils.js before running this script!");
            }

            let _ = this;
            _.def = {
                container: document.querySelector('main'),
                draggable: document.querySelector('div'),
                options: {
                    abscissa: true,
                    ordinate: true,
                    limitless: false
                },
                variables: {
                    className: 'translate',
                    currentX: '--ix',
                    currentY: '--iy',
                    translateX: '--tx',
                    translateY: '--ty',
                }
            }
            $extendObj(_.def, addons);
        }

        init() {
            let _ = this;
            let locked = false,
                currentX = null, 
                currentY = null,
                x0 = null, 
                y0 = null, 
                tx = 0, 
                ty = 0;

            let negativeMaxHeight = (_.def.draggable.offsetHeight - _.def.container.offsetHeight)*(-1),
                negativeMaxWidth  = _.def.draggable.offsetWidth*(-1);
            function unify(e) { 
                return e.changedTouches ? e.changedTouches[0] : e 
            };

            function lock(e) {
                let aux = unify(e);
                x0 = aux.clientX;
                y0 = aux.clientY;
                _.def.draggable.classList.toggle(_.def.variables.className, (locked = true));
                console.log("down");
            _.def.container.addEventListener("mousemove", drag, false);
            _.def.container.addEventListener("touchmove", drag, false);
            };

            function drag(e) {
                e.preventDefault();
                if(locked) {
                    if(_.def.options.abscissa) {
                        tx = Math.round((unify(e).clientX - x0));
                        _.def.draggable.style.setProperty(_.def.variables.translateX, `${tx}px`);
                    }
                    if(_.def.options.ordinate) {
                        ty = Math.round((unify(e).clientY - y0));
                        _.def.draggable.style.setProperty(_.def.variables.translateY, `${ty}px`);
                    }
                }
                console.log("drag");
            };

            function move(e) {
                if(locked) {
                    currentX += tx;
                    currentY += ty;
                    if(_.def.options.abscissa) {
                        if(!_.def.options.limitless) {
                            currentX = (currentX < 0) ? currentX : 0;
                            currentX = (negativeMaxWidth < currentX) ?  currentX : negativeMaxWidth;
                        }
                        _.def.draggable.style.setProperty(_.def.variables.currentX, `${currentX}px`);
                        _.def.draggable.style.setProperty(_.def.variables.translateX, "0px");
                    }
                    if(_.def.options.ordinate) {
                        if(!_.def.options.limitless) {
                            currentY = (currentY < 0) ? currentY : 0;
                            currentY = (negativeMaxHeight < currentY) ?  currentY : negativeMaxHeight;
                        }
                        _.def.draggable.style.setProperty(_.def.variables.currentY, `${currentY}px`);
                        _.def.draggable.style.setProperty(_.def.variables.translateY, "0px");
                    }
                    _.def.draggable.classList.toggle(_.def.variables.className, !(locked = false));
                    x0 = null;
                    tx = null;
                }
                console.log("up");
            _.def.container.removeEventListener("mousemove", drag, false);
            _.def.container.removeEventListener("touchmove", drag, false);
            }

            _.def.container.addEventListener("mousedown", lock, false);
            _.def.container.addEventListener("touchstart", lock, false);

            _.def.container.addEventListener("mouseup", move, false);
            _.def.container.addEventListener("touchend", move, false);

            return _;
        }
    } 
    return swiper;
})();
