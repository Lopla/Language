"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.run = exports.getAvailbleFunctions = exports.loplaToolExists = exports.loplaToolFile = exports.LoplaMethods = exports.LoplaKeywords = exports.LoplaSchema = exports.loplaScripsPath = exports.loplaTool = exports.loplaToolPath = void 0;
const vscode = require("vscode");
const path = require("path");
const execFile = require("child_process");
const extension_1 = require("./extension");
//E:\Work\Language\build\loplac-core\Lopla.Console.exe
//export const loplaToolPath =    path.resolve(__dirname, '../resources/loplac');
exports.loplaToolPath = path.resolve(__dirname, '../../../build/loplac-core');
exports.loplaTool = path.resolve(__filename, exports.loplaToolPath, "Lopla.Console.exe");
exports.loplaScripsPath = path.resolve(__dirname, '../resources/scripts');
exports.LoplaSchema = {};
exports.LoplaKeywords = ['function', 'while', 'if', 'return'];
exports.LoplaMethods = {};
function pupulateFunctionArgs() {
    for (const key of Object.keys(exports.LoplaSchema)) {
        for (const methods of Object.keys(exports.LoplaSchema[key])) {
            var m = key + "." + methods;
            var fileRun = execFile.execFileSync(exports.loplaTool, ["/nogui", exports.loplaScripsPath, "function", m]);
            var rows = fileRun.toString().split("\n");
            exports.LoplaMethods[m] = [];
            if (rows) {
                for (var k = 1; k < rows.length; k++) {
                    var p = rows[k].trim();
                    if (p)
                        exports.LoplaMethods[m].push(p);
                }
            }
        }
    }
}
function loplaToolFile() {
    return exports.loplaTool;
}
exports.loplaToolFile = loplaToolFile;
function loplaToolExists() {
    try {
        execFile.execFileSync(exports.loplaTool, ["/nogui", exports.loplaScripsPath, "functions"]);
        return true;
    }
    catch (error) {
        return false;
    }
}
exports.loplaToolExists = loplaToolExists;
function getAvailbleFunctions() {
    execFile.execFile(exports.loplaTool, ["/nogui", exports.loplaScripsPath, "functions"], {}, (error, stdout, stderr) => {
        var r = new RegExp("([a-zA-Z]+)[.]([a-zA-Z]+)");
        if (stdout) {
            var lines = stdout.split("\n");
            lines.forEach(function (line) {
                let t = line.trim();
                var e = r.exec(t);
                if (e && e.length > 2) {
                    var schema = e[1];
                    var method = e[2];
                    if (!exports.LoplaSchema[schema])
                        exports.LoplaSchema[schema] = {};
                    exports.LoplaSchema[schema][method] = {};
                }
            });
        }
        pupulateFunctionArgs();
    });
}
exports.getAvailbleFunctions = getAvailbleFunctions;
function run() {
    var currentlyOpenTabfilePath = vscode.window.activeTextEditor.document.fileName;
    var currentlyOpenTabfileName = path.dirname(currentlyOpenTabfilePath);
    execFile.execFile(exports.loplaTool, [currentlyOpenTabfileName], {}, (error, stdout, stderr) => {
        if (error || stderr) {
            var e = error || stderr;
            vscode.window.showErrorMessage(e.toString());
        }
        if (stdout)
            extension_1.outputWindow.appendLine(stdout);
        if (stderr)
            extension_1.outputWindow.appendLine(stderr);
    });
}
exports.run = run;
//# sourceMappingURL=intelisense.js.map