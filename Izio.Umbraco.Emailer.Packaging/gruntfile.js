/// <binding AfterBuild='build' ProjectOpened='build' />
module.exports = function (grunt) {

    var name = "Izio.Umbraco.Emailer";
    var version = "1.0.1";
    var namespace = "Izio.Umbraco";
    var source = "../Izio.umbraco.Emailer/";
    var destination = "../Izio.umbraco.Emailer.Package/";

    grunt.initConfig({
        copy: {
            package: {
                files: [
                    {
                        expand: true,
                        cwd: source + "bin/",
                        src: [
                            "**/" + namespace + ".*.dll"
                        ],
                        dest: destination + "temp/bin/",
                        flatten: true
                    },
                    {
                        expand: true,
                        cwd: source + "App_Plugins",
                        src: ["**"],
                        dest: destination + "temp/App_Plugins/"
                    },
                   {
                       expand: true,
                       cwd: source + "Scripts",
                       src: ["**"],
                       dest: destination + "temp/Scripts/"
                   },
                   {
                       expand: true,
                       cwd: source + "Views",
                       src: ["**"],
                       dest: destination + "temp/Views/"
                   }
                ]
            }
        },
        umbracoPackage: {
            package: {
                src: destination + "temp/",
                dest: destination,
                options: {
                    name: "Izio.Umbraco.Emailer",
                    version: version,
                    url: "http://www.izio.co.uk",
                    license: "MIT",
                    licenseUrl: "http://www.izio.co.uk/license/mit",
                    author: "Izio",
                    authorUrl: "http://www.izio.co.uk",
                    readme: "Izio Emailer is an email form manager for Umbraco with built in spam protection and an optional configurable auto-responder. Define your emailer forms in the Umbraco back office and then include it anywhere in your website using the supplied macro. To use Izio Emailer you must have configured your SMTP settings in the web.config file and reference verify.js, jquery.js and izio.emailer.js on any page where you will be displaying an emailer form.",
                    outputName: name + "." + version + ".zip",
                    manifest: "package.xml"
                }
            }
        },
        clean: {
            package: {
                files: {
                    src: ["../Izio.umbraco.Emailer.Package/*", "!../Izio.umbraco.Emailer.Package/*.zip"]
                },
                options: {
                    force: true
                }
            }
        }
    });

    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks("grunt-contrib-copy");
    grunt.loadNpmTasks("grunt-umbraco-package");


    grunt.registerTask("build", ["copy", "umbracoPackage", "clean"]);
    grunt.registerTask("default", ["build"]);
};