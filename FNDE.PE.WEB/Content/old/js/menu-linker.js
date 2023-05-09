let LinkMenu = (function() {
    let linker = function(userSettings) {
        let _ = this;
        _.def = {
            itemParent: $1('section'),
            anchorParent: $1('aside'),
            itemChildren: 'article',
            anchorChildren: 'a',
            activeClass: 'isActive',
            options: {
                childrenSubmited: false,
                singleToggled: true,
                isValidated: false
            }
        }
        $extendObj(_.def, userSettings);
        _.init();
    } 

    linker.prototype.trigger = function() {
        let _ = this;
        if(_.def.itemChildren.length === _.def.anchorChildren.length) {
            for(let i = 0; i < _.def.anchorChildren.length; i++) {
                _.def.anchorChildren[i].addEventListener('click', function(e) {
                    if(_.def.options.singleToggled) {
                        $1('.' + _.def.activeClass, _.def.itemParent)
                            .classList.toggle(_.def.activeClass);

                        $1('.' + _.def.activeClass, _.def.anchorParent)
                            .classList.toggle(_.def.activeClass);
                    }
                    e.currentTarget.classList.toggle(_.def.activeClass);
                    _.def.itemChildren[i].classList.toggle(_.def.activeClass);
                }, false);
            }
        } else {
            console.log('Linker says that there is no enough articles or links');
        }
    }
    linker.prototype.init = function() {
        let _ = this;
        if(!_.def.options.isValidated) {
            if(!$1('.isActive', _.def.itemParent)) {
                console.log('add the class isActive to one of your articles');
            }
            if(!$1('.isActive', _.def.anchorParent)) {
                console.log('add the class isActive to one of your anchors');
            }
        }
        if(!_.def.options.childrenSubmited) {
            _.def.itemChildren = $(_.def.itemChildren, _.def.itemParent);
            _.def.anchorChildren = $(_.def.anchorChildren, _.def.anchorParent);
        }   
        _.trigger();
    }
    return linker;
})();
