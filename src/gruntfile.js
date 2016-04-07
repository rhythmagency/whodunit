module.exports = function(grunt) {

    // Variables.
    var appProject = "Whodunit.app";
    var website = "Whodunit.test";
    var binaries = ["Whodunit.app.dll", "Whodunit.app.pdb"];
    var temp = "./WhodunitTemp/package/";

    // Initialize Grunt tasks.
    grunt.initConfig({
        "pkg": grunt.file.readJSON("package.json"),
        copy: {
            // Main is used to copy files to the sample website.
            main: {
                files: [
                    {
                        // Frontend files.
                        expand: true,
                        src: ["App_Plugins/**"],
                        dest: website + "/",
                        cwd: appProject + "/"
                    }
                ]
            },
            // Package is used to copy files to create the Umbraco package.
            package: {
                files: [
                    {
                        // Frontend files.
                        expand: true,
                        src: ["App_Plugins/**"],
                        dest: temp,
                        cwd: appProject + "/"
                    }, {
                        // App binaries.
                        expand: true,
                        src: binaries,
                        dest: temp + "bin/",
                        cwd: appProject + "/bin/Release/"
                    }
                ]
            }
        },
        clean: {
            main: {
                src: [
                    // Temporary folder for intermediate build artifacts.
                    "./WhodunitTemp"
                ]
            }
        },
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
        },
        wait: {
            // Useful for creating a slight delay between tasks to confirm they are actually happening.
            second: {
                options: {
                    delay: 1000
                }
            }
        }
    });

    // Load NPM tasks.
    grunt.loadNpmTasks("grunt-wait");
    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks("grunt-contrib-copy");
    grunt.loadNpmTasks("grunt-umbraco-package");

    // Register Grunt tasks.
    grunt.registerTask("default",
        // The "default" task is for general development of Whodunit.
        [ "clean:main", "copy:main", "clean:main" ]);
    grunt.registerTask("package",
        // The "package" task will create an Umbraco package of Whodunit.
        [ "clean:main", "copy:package", "clean:main" ]);

};