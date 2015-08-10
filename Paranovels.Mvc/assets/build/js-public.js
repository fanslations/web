({
    mainConfigFile : "../js/requirejs.config.js",   
	//appDir: "../",
	//dir: '../dist',
	baseUrl: "../js", 	
	out: "../js/paranovels.public.min.js",
    name: "paranovels.public",    
	preserveLicenseComments: false,
	//optimize: 'none',
	paths: {
	    requireLib: 'require',
	},
	exclude: [
	],

	include: ['requireLib', 'angular', 'marked', 'selectize', 'colorpicker', 'sortable'],
	optimizeCss: "standard",
	
    removeCombined: true,
    findNestedDependencies: true,
	logLevel: 1
})