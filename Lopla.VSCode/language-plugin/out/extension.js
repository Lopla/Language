"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.deactivate = exports.activate = exports.outputWindow = void 0;
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
const task_1 = require("./task");
const libsCompletetionProvider_1 = require("./libsCompletetionProvider");
const intelisense_2 = require("./intelisense");
const functionCompletetionProvider_1 = require("./functionCompletetionProvider");
let loplaStatusBarItem;
let loplaDocumentScheme = { scheme: 'file', language: 'lopla' };
let pluginEnabled = false;
function activate(context) {
    // check if lplc consoel exists and if the file exist
    if (!intelisense_1.loplaToolExists()) {
        vscode.window.showErrorMessage("Lopla console not found.");
    }
    else {
        pluginEnabled = true;
    }
    intelisense_1.getAvailbleFunctions();
    context.subscriptions.push(vscode.commands.registerCommand('extension.lopla.run', () => {
        intelisense_2.run();
    }));
    context.subscriptions.push(vscode.commands.registerCommand('extension.lopla.diag', () => {
        if (!pluginEnabled) {
            vscode.window.showErrorMessage("Lopla console not found. " + intelisense_1.loplaToolFile());
        }
        else {
            vscode.window.showInformationMessage("Lopla console is available.");
        }
    }));
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
    context.subscriptions.push(vscode.languages.registerSignatureHelpProvider(loplaDocumentScheme, new functionCompletetionProvider_1.FunctionCompletetionProvider(), "("));
    let workspaceRoot = vscode.workspace.rootPath;
    const taskProvider = vscode.tasks.registerTaskProvider("lopla", new task_1.LoplaTaskProvider(workspaceRoot));
    exports.outputWindow = vscode.window.createOutputChannel("Lopla");
    loplaStatusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
    loplaStatusBarItem.command = 'extension.lopla.diag';
    loplaStatusBarItem.text = `Lopla`;
    loplaStatusBarItem.color = vscode.ThemeColor.name;
    loplaStatusBarItem.show();
    context.subscriptions.push(loplaStatusBarItem);
}
exports.activate = activate;
function deactivate() {
}
exports.deactivate = deactivate;
//# sourceMappingURL=extension.js.map