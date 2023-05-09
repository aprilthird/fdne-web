
const _app = {
    constants: {
        url: {
            root: ""
        }
    },
    parseToUrl: function (urlString) {
        var url = window.location.protocol;
        url += "//";
        url += window.location.host;
        url += this.constants.url.root;
        url += urlString;
        return url;
    }
};