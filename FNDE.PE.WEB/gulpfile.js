/// <binding ProjectOpened='scss:watch' />
//gulpfile.js

/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

/*** CONSTANTS AND VARIABLES ***/
const folders = {
    /*** BASE ***/
    root: "Content",
    src: "src",
    dist: "dist",
    /*** SUB FOLDERS ***/
    css: "css",
    js: "js",
    scss: "scss",
    fonts: "fonts",
    images: "img",
    videos: "video",
    vendors: "vendors"
};

var paths = {
    src: {
        scss: `${folders.root}/${folders.src}/${folders.scss}`,
        js: `${folders.root}/${folders.src}/${folders.js}`,
        fonts: `${folders.root}/${folders.src}/${folders.fonts}`,
        images: `${folders.root}/${folders.src}/${folders.images}`,
        videos: `${folders.root}/${folders.src}/${folders.videos}`
    },
    dist: {
        css: `${folders.root}/${folders.dist}/${folders.css}`,
        js: `${folders.root}/${folders.dist}/${folders.js}`,
        fonts: `${folders.root}/${folders.dist}/${folders.fonts}`,
        images: `${folders.root}/${folders.dist}/${folders.images}`,
        videos: `${folders.root}/${folders.dist}/${folders.videos}`
    }
};

var files = {
    src: {
        scss: [`${paths.src.scss}/**/*.scss`], // zero or more directories
        js: [`${paths.src.js}/**/*.js`],
        images: [`${paths.src.images}/*.jpg`, `${paths.src.images}/*.png`]
    },
    dist: {
        css: {
            all: `${paths.dist.css}/**`
        }
    }
};

/*** DEPENDENCIES ***/
var gulp = require("gulp");
var del = require("del"); // replaces gulp-clean on 4.0
var sass = require("gulp-sass");
var sassLint = require("gulp-sass-lint");
var plumber = require("gulp-plumber"); // prevent error on watch
var notify = require("gulp-notify"); // cute notifier
var imagemin = require("gulp-imagemin"); // images
var beeper = require("beeper"); // sound
var jshint = require("jshint-stylish"); // more stylish debugging

gulp.task("scss:watch", () =>
    gulp.watch(paths.src.scss, gulp.series("scss:build"))
);

gulp.task("scss:clean", () =>
    del([`${paths.dist.css}/**`, `!${paths.dist.css}`]) //files.dist.css.all
);

gulp.task("scss:build", () =>
    gulp.src(files.src.scss)
        .pipe(plumber({
            errorHandler: function (err) {
                notify.onError({
                    title: "Gulp error in " + err.plugin,
                    message: "Message: <%= error.message %>", //e.toString(),
                    sound: "Beep"
                })(err);
                this.emit("end");
                //beeper("****-*-*"); // sound :D
            }
        }))
        .pipe(sass())
        .pipe(gulp.dest(paths.dist.css))
);

// gulp.series and gulp.parallel are features from gulp 4.0.0
gulp.task("scss:all", gulp.series("scss:clean", "scss:build"));

// TODO: specify custom plugins for optimizing
// basic img
gulp.task("img:min", () =>
    gulp.src(paths.src.images)
        .pipe(imagemin())
        .pipe(gulp.dest(paths.dist.images))
);

// Task to run SCSS lint code health
gulp.task("scss:lint", () => {
    // TODO: check option rules
    gulp.src(files.src.scss)
    .pipe(sassLint({
        options: {
            formatter: 'stylish',
            'merge-default-rules': true
        },
        rules: {
            'no-ids': 2,
            'no-mergeable-selectors': 2
        }
    }))
    .pipe(sassLint.format())
    .pipe(sassLint.failOnError());
});

gulp.task("default", gulp.parallel("scss:all", "img:min"));
