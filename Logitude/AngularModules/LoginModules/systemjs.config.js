 
(function (global) {
    System.config({
        paths: {
            'npm:': 'node_modules/'
        },

        map: {
            // our app is within the app folder
            Login: 'Login',
            'chartjs': 'npm:chartjs/chart.js',

            // angular bundles
            '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
            '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
            '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
            '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
            '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
            '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
            '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
            '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',

            // other libraries
            'rxjs': 'npm:rxjs',
            'angular2-in-memory-web-api': 'npm:angular2-in-memory-web-api',
            'ng2-charts': 'npm:ng2-charts',
            'core': 'npm:chartjs/core/core.js'
        },

        packages:
            {
                Login: { main: './Bootstraper.js', defaultExtension: 'js' },
                rxjs: { defaultExtension: 'js' },
            }
    });
})(this);