'use strict';
(function() {
  let anchors = document.querySelectorAll("a.menu__link");
  for(let i = 0; i < anchors.length; i++) {
    let wrapper = document.createElement("div");
    wrapper.classList.add('menu__highlight');
    wrap(anchors[i], wrapper);
  }
})();
(function() {
  console.log('starting js-submenu event injection');
  const visibleCN = 'menu__dropdown--visible';
  const toggleVisibleyByClassName = function(className, current) {
    let nodes = document.getElementsByClassName(className);
    for(let i = 0; i < nodes.length; i++) {
      if(!current.isEqualNode(nodes[i])) {
        nodes[i].classList.remove(visibleCN);
      }
    }
  }
  let nodes = document.getElementsByClassName('js-submenu');
  for (let i = 0; i < nodes.length ; i++) {
    let wrapper = nodes[i].firstElementChild;
    let link = wrapper.firstElementChild;
    link.addEventListener('click', function () {
        let dropdown = wrapper.nextElementSibling;
        dropdown.classList.toggle(visibleCN);
        toggleVisibleyByClassName(visibleCN, dropdown);
      });
  }
  console.log('processed nodes: ' + nodes.length );
})();

const firstElementByClassName = function(className) {
  let e = document.getElementsByClassName(className)
  if(e.length > 0) {
    console.log(`first #{className} found`);
    return e;
  }
    console.log(`#{className} not found`);
};

(function(){
  const collapsibleCN = 'navbar__collapsible-container';
  const animationIn   = 'collapse-in';
  const animationOut  = 'collapse-out';
  let checkbox  = document.getElementsByClassName('js-toggle')[0];
  let container = document.getElementsByClassName(collapsibleCN)[0];
  // if the input is a checkbox, the element will change the value
  // of its property checked before calling the eventListener
  checkbox.addEventListener('click', function() {
    if(!checkbox.checked) {
      container.classList.add(animationOut);
      if(container.classList.contains(animationIn)){
        container.classList.remove(animationIn);
      }
    }
    else {
      container.classList.add(animationIn);
      if(container.classList.contains(animationOut)){
        container.classList.remove(animationOut);
      }
    }
  });
})();


function $(elem) {
  return document.querySelector(elem);
}
function hasClass(el, className) {
  return el.classList 
    ? el.classList.contains(className) 
    : new RegExp('(^| )' + className
      + '( |$)', 'gi').test(el.className);
}
function addClass(el, className) {
  if (el.classList) {
    el.classList.add(className);
  } else {
    el.className += ' ' + className;
  }
}
function removeClass(el, className) {
  if (el.classList) {
    el.classList.remove(className);
  } else {
    el.className = el.className.replace(new RegExp('(^|\\b)' 
      + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
  }
}
function $extendObj(_def, addons) {
  if (typeof addons !== "undefined") {
    for (var prop in _def) {
      if (addons[prop] != undefined) {
        _def[prop] = addons[prop];
      }
    }
  }
}

function wrap(el, wrapper) {
	    el.parentNode.insertBefore(wrapper, el);
	    wrapper.appendChild(el);
	}
