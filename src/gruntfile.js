module.exports = function(grunt) {

    // Initialize Grunt tasks.
    grunt.initConfig({
        "pkg": grunt.file.readJSON('package.json'),
        umbracoPackage: {
            main: {
                src: "./files",
                dest: "../dist",
                options: {
                    name: "Whodunit",
                    version: "0.1.0",
                    url: "https://github.com/rhythmagency/whodunit",
                    license: "MIT License",
                    licenseUrl: "http://opensource.org/licenses/MIT",
                    author: "Rhythm Agency",
                    authorUrl: "http://rhythmagency.com/",
                    readme: grunt.file.read("templates/inputs/readme.txt"),
                    manifest: "templates/package.template.xml"
                }
            }
        }
    });

    // Load NPM tasks.
    grunt.loadNpmTasks("grunt-umbraco-package");

    // Register Grunt tasks.
    grunt.registerTask("default",
        // The "default" task is for general development of ReadingAge.
        [ "umbracoPackage:main" ]);

};