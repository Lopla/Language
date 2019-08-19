"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const path = require("path");
const execFile = require("child_process");
let runTaskProvider;
let LoplaSchema = {};
let LoplaKeywords = ['function', 'while', 'if', 'return'];
let myStatusBarItem;
const loplaToolPath = path.resolve(__dirname, '../resources/loplad');
const loplaTool = path.resolve(__filename, loplaToolPath, "loplad.exe");
function pupulateFunctionArgs() {
    for (const key of Object.keys(LoplaSchema)) {
        for (const methods of Object.keys(LoplaSchema[key])) {
            //console.log(key+"."+methods)
        }
    }
}
function getAvailbleFunctions() {
    var loplaScripsPath = path.resolve(__dirname, '../resources/scripts');
    execFile.execFile(loplaTool, [loplaScripsPath, "functions"], {}, (error, stdout, stderr) => {
        var r = new RegExp("([a-zA-Z]+)[.]([a-zA-Z]+)");
        if (stdout) {
            var lines = stdout.split("\n");
            lines.forEach(function (line) {
                let t = line.trim();
                var e = r.exec(t);
                if (e && e.length > 2) {
                    var schema = e[1];
                    var method = e[2];
                    if (!LoplaSchema[schema])
                        LoplaSchema[schema] = {};
                    LoplaSchema[schema][method] = {};
                }
            });
        }
        pupulateFunctionArgs();
    });
}
function activate(context) {
    getAvailbleFunctions();
    context.subscriptions.push(vscode.commands.registerCommand('extension.lopla.run', () => {
        var currentlyOpenTabfilePath = vscode.window.activeTextEditor.document.fileName;
        var currentlyOpenTabfileName = path.dirname(currentlyOpenTabfilePath);
        execFile.execFile(loplaTool, [currentlyOpenTabfileName], {}, (error, stdout, stderr) => {
            if (error || stderr) {
                var e = error || stderr;
                vscode.window.showErrorMessage(e.toString());
            }
            console.log(error, stdout, stderr);
        });
    }));
    var loplaDocumentScheme = { scheme: 'file', language: 'lopla' };
    context.subscriptions.push(vscode.languages.registerHoverProvider(loplaDocumentScheme, {
        provideHover(document, position, token) {
            return {
                contents: ['Lopla file']
            };
        }
    }));
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, {
        provideCompletionItems(document, position) {
            let suggestions = [];
            for (const key of Object.keys(LoplaSchema)) {
                suggestions.push(new vscode.CompletionItem(key, vscode.CompletionItemKind.Module));
            }
            for (const val of LoplaKeywords) {
                suggestions.push(new vscode.CompletionItem(val, vscode.CompletionItemKind.Keyword));
            }
            return suggestions;
        }
    }));
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, {
        provideCompletionItems(document, position) {
            let p = position.translate(0, -position.character);
            let line = document.getText(new vscode.Range(p, position));
            for (const key of Object.keys(LoplaSchema)) {
                let rs = '\\b' + key + '\\.';
                let r = new RegExp(rs);
                if (line && r.test(line)) {
                    var suggestions = [];
                    var methods = Object.keys(LoplaSchema[key]);
                    methods.forEach(element => {
                        suggestions.push(new vscode.CompletionItem(element, vscode.CompletionItemKind.Function));
                    });
                    return suggestions;
                }
            }
            return null;
        }
    }, "."));
    /*
    status bar
    */
    const myCommandId = 'extension.lopla.run';
    myStatusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
    myStatusBarItem.command = myCommandId;
    myStatusBarItem.text = `Run lopla`;
    myStatusBarItem.color = vscode.ThemeColor.name;
    myStatusBarItem.show();
    context.subscriptions.push(myStatusBarItem);
}
exports.activate = activate;
function deactivate() {
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map