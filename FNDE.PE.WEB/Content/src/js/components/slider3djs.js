function slide3d(className) {
    var slider = document.getElementsByClassName(className);
    if (slider.length > 0) {
        slider[0].checked = true;
        var input = show(slider[0]);
    }
}

function show(e) {
    function changeDisplayProperty(name, num) {
        const maxOffset = 5;
        let id = num;
        console.log('Trigger by Component Id:' + id);
        if (id && id < maxOffset) {
            id = name + num;
            document.getElementById(id).style.display = 'flex';
            console.log('property display changed to block: ' + id);
            for (let i = maxOffset - 1; i > 0; i--) {
                if (i !== num) {
                    id = name + i;
                    document.getElementById(id).style.display = 'none';
                    console.log('property display changed to none: ' + id);
                }
            }
        }
        else {
            console.log('greather than 5');
        }
    };

    const maxOffset = 5;
    let id = e.getAttribute('id');
    id = id ? id[id.length - 1] : null;
    id = id ? parseInt(id, maxOffset) : null;
    changeDisplayProperty('info', id);
};