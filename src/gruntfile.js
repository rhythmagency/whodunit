module.exports = function(grunt) {

    // Initialize Grunt tasks.
    grunt.initConfig({
        "pkg": grunt.file.readJSON('package.json'),
/*		uglify: {
			options: {
				// the banner is inserted at the top of the output
				banner: ''
			},
			dist: {
				files: {
					'files/App_Plugins/Whodunit/plugin.min.js': [ 'files/App_Plugins/Whodunit/plugin.js' ]
				}
			}
		},*/
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
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-umbraco-package");

    // Register Grunt tasks.
    grunt.registerTask("default",
        // The "default" task is for general development of ReadingAge.
        [ "uglify:dist", "umbracoPackage:main" ]);

};