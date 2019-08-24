"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
let runTaskProvider;
let myStatusBarItem;
function activate(context) {
    intelisense_1.getAvailbleFunctions();
    context.subscriptions.push(vscode.commands.registerCommand('extension.lopla.run', () => {
        run();
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
            for (const key of Object.keys(intelisense_1.LoplaSchema)) {
                suggestions.push(new vscode.CompletionItem(key, vscode.CompletionItemKind.Module));
            }
            for (const val of intelisense_1.LoplaKeywords) {
                suggestions.push(new vscode.CompletionItem(val, vscode.CompletionItemKind.Keyword));
            }
            return suggestions;
        }
    }));
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, {
        provideCompletionItems(document, position) {
            let p = position.translate(0, -position.character);
            let line = document.getText(new vscode.Range(p, position));
            for (const key of Object.keys(intelisense_1.LoplaSchema)) {
                let rs = '\\b' + key + '\\.';
                let r = new RegExp(rs);
                if (line && r.test(line)) {
                    var suggestions = [];
                    var methods = Object.keys(intelisense_1.LoplaSchema[key]);
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
    myStatusBarItem.text = `Lopla`;
    myStatusBarItem.color = vscode.ThemeColor.name;
    myStatusBarItem.show();
    context.subscriptions.push(myStatusBarItem);
}
exports.activate = activate;
function deactivate() {
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map