/// <binding ProjectOpened='watch' />
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
    watch('SCSS/*.scss', css);
}

exports.watch = watchCSS;
exports.default = css;