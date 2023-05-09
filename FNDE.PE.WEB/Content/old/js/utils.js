function $(el, context) {
  return (context || document).querySelectorAll(el);
}
function $1(el, context) {
  return (context || document).querySelector(el);
}
function $extendObj(_def, addons) {
  if (typeof addons == "undefined") {
    return;
  }
  for (var prop in _def) {
    if (addons[prop] != undefined) {
      _def[prop] = addons[prop];
    }
  }
}
