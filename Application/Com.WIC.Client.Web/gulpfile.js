/// <binding ProjectOpened='watch' />
const { src, dest, watch } = require('gulp');
const sass = require('gulp-sass');
const minifyCSS = require('gulp-csso');
const browsersync = require("browser-sync").create();

function css() {
    return src('SCSS/*.scss', { sourcemaps: true })
        .pipe(sass())
        .pipe(minifyCSS())
        .pipe(dest('wwwroot/css'))
        .pipe(browsersync.stream());
}

function watchCSS() {
    return watch('SCSS/*.scss', css);
}

exports.watch = watchCSS;
exports.css = css;
exports.default = css;