﻿cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/com.phonegap.plugins.barcodescanner/www/barcodescanner.js",
        "id": "com.phonegap.plugins.barcodescanner.BarcodeScanner",
        "clobbers": [
            "cordova.plugins.barcodeScanner"
        ]
    },
    {
        "file": "plugins/com.phonegap.plugins.barcodescanner/src/windows8/BarcodeScannerProxy.js",
        "id": "com.phonegap.plugins.barcodescanner.BarcodeScannerProxy",
        "merges": [
            ""
        ]
    },
    {
        "file": "plugins/org.apache.cordova.camera/www/CameraConstants.js",
        "id": "org.apache.cordova.camera.Camera",
        "clobbers": [
            "Camera"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.camera/www/CameraPopoverOptions.js",
        "id": "org.apache.cordova.camera.CameraPopoverOptions",
        "clobbers": [
            "CameraPopoverOptions"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.camera/www/Camera.js",
        "id": "org.apache.cordova.camera.camera",
        "clobbers": [
            "navigator.camera"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.camera/www/CameraPopoverHandle.js",
        "id": "org.apache.cordova.camera.CameraPopoverHandle",
        "clobbers": [
            "CameraPopoverHandle"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.camera/src/windows/CameraProxy.js",
        "id": "org.apache.cordova.camera.CameraProxy",
        "merges": [
            ""
        ]
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "com.phonegap.plugins.barcodescanner": "1.2.0",
    "org.apache.cordova.camera": "0.3.2"
}
// BOTTOM OF METADATA
});