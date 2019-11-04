/// <binding ProjectOpened='watch, default' />
const { src, dest, watch } = require('gulp');
const sass = require('gulp-sass');
const minifyCSS = require('gulp-csso');

function css() {
    return src('SCSS/*.scss', { sourcemaps: true })
        .pipe(sass())
        .pipe(minifyCSS())
        .pipe(dest('wwwroot/css'));
}

function watchCSS() {
    return watch('SCSS/*.scss', css);
}

exports.watch = watchCSS;
exports.css = css;
exports.default = css;