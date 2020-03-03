"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
class LibsCompletetionProvider {
    provideCompletionItems(document, position, token, context) {
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
    resolveCompletionItem(item, token) {
        throw new Error("Method not implemented.");
    }
}
exports.LibsCompletetionProvider = LibsCompletetionProvider;
//# sourceMappingURL=libsCompletetionProvider.js.map