"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
const task_1 = require("./task");
const libsCompletetionProvider_1 = require("./libsCompletetionProvider");
let loplaStatusBarItem;
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
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, new libsCompletetionProvider_1.LibsCompletetionProvider(), "."));
    let workspaceRoot = vscode.workspace.rootPath;
    const taskProvider = vscode.tasks.registerTaskProvider("lopla", new task_1.LoplaTaskProvider(workspaceRoot));
    // const myCommandId = 'extension.lopla.run';
    // loplaStatusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
    // loplaStatusBarItem.command = myCommandId;
    // loplaStatusBarItem.text = `Lopla`;
    // loplaStatusBarItem.color = vscode.ThemeColor.name;
    // loplaStatusBarItem.show();
    // context.subscriptions.push(loplaStatusBarItem);
}
exports.activate = activate;
function deactivate() {
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map